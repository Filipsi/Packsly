using Packsly3.Core;
using Packsly3.Core.Common.Register;
using Packsly3.Core.Launcher.Instance;
using Packsly3.Core.Launcher.Modloader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Packsly3.Server.Launcher.Modloader {

    [Register]
    public class ServerModLoaderHandler : InstanceModLoaderHandler<ServerMinecraftInstance> {

        #region Fields

        private readonly Regex forgeJarFilePattern = new Regex(
            pattern: @"forge-(\d{1,2}\.\d{1,2}\.\d{1,2}-\d{1,2}\.\d{1,2}\.\d{1,2}\.\d{1,4})-\w+\.jar",
            options: RegexOptions.Compiled
        );

        #endregion

        #region IModLoaderHandler

        public override bool IsCompatible(string modLoader)
           => modLoader == Packsly.Constants.ForgeModlaoder;

        public override void DetectModLoaders(ServerMinecraftInstance instance, List<ModLoaderInfo> modLoaders) {
            FileInfo forgeJar = GetForgeJarFile(instance);

            if (forgeJar != null) {
                modLoaders.Add(
                    new ModLoaderInfo(
                        manager: instance.ModLoaderManager,
                        name: Packsly.Constants.ForgeModlaoder,
                        version: forgeJarFilePattern.Match(forgeJar.Name).Groups[1].Value
                    )
                );
            }
        }

        public override void Install(ServerMinecraftInstance instance, string modLoader, string version) {
            if (modLoader != Packsly.Constants.ForgeModlaoder) {
                return;
            }

            string installerPath = Path.Combine(
                instance.Location.FullName,
                $"forge-{version}-installer.jar"
            );

            using (WebClient client = new WebClient()) {
                client.DownloadFile(
                    address: GetForgeInstallerUrl(instance.MinecraftVersion, version),
                    fileName: installerPath
                );
            }

            Process terminalProcess = new Process {
                StartInfo = new ProcessStartInfo {
                    WindowStyle = ProcessWindowStyle.Normal,
                    WorkingDirectory = Packsly.Launcher.Workspace.FullName,
                    FileName = "cmd.exe",
                    Arguments = $"/c java -jar \"{installerPath}\" --installServer nogui"
                }
            };

            terminalProcess.Start();
            terminalProcess.WaitForExit();

            // TODO: Remove installer and forge-{version}-installer.jar.log
        }

        public override void Uninstall(ServerMinecraftInstance instance, string modLoader) {
            if (modLoader != Packsly.Constants.ForgeModlaoder) {
                return;
            }

            FileInfo forgeJar = GetForgeJarFile(instance);
            if (forgeJar != null && forgeJar.Exists) {
                forgeJar.Delete();
            }
        }

        #endregion

        #region Helpers

        private FileInfo GetForgeJarFile(IMinecraftInstance instance) {
            return instance.Location
                .EnumerateFiles()
                .FirstOrDefault(file => forgeJarFilePattern.IsMatch(file.Name));
        }

        private string GetForgeInstallerUrl(string minecraftVersion, string forgeVersion) {
            string version = $"{minecraftVersion}-{forgeVersion}";
            return $"https://files.minecraftforge.net/maven/net/minecraftforge/forge/{version}/forge-{version}-installer.jar";
        }

        #endregion

    }

}
