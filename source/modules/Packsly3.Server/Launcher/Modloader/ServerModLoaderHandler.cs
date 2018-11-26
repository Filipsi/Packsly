using NLog;
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

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Regex forgeJarFilePattern = new Regex(
            pattern: @"forge-(\d{1,2}\.\d{1,2}\.\d{1,2})-(\d{1,2}\.\d{1,2}\.\d{1,2}\.\d{1,4})-\w+\.jar",
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
                        name: Packsly.Constants.ForgeModlaoder,
                        version: forgeJarFilePattern.Match(forgeJar.Name).Groups[2].Value
                    )
                );
            }
        }

        public override void Install(ServerMinecraftInstance instance, string modLoader, string version) {
            if (modLoader != Packsly.Constants.ForgeModlaoder) {
                Logger.Error($"Mod loader handler is not compatible with modloader with name '{modLoader}'");
                return;
            }

            // Download forge installer
            string installerPath = Path.Combine(
                instance.Location.FullName,
                $"forge-{version}-installer.jar"
            );

            using (WebClient client = new WebClient()) {
                Logger.Info($"Downloading forge installer for version {version}...");
                client.DownloadFile(
                    address: GetForgeInstallerUrl(instance.MinecraftVersion, version),
                    fileName: installerPath
                );
            }

            // Run forge installer in separate process
            bool installationError = false;
            using(Process terminalProcess = new Process()) {
                terminalProcess.EnableRaisingEvents = true;
                terminalProcess.StartInfo = new ProcessStartInfo {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    WorkingDirectory = Packsly.Launcher.Workspace.FullName,
                    FileName = Packsly.IsLinux ? "sh" : "cmd.exe",
                    Arguments = (Packsly.IsLinux ? "-c" : "/c") + $"java -jar \"{installerPath}\" --installServer nogui"
                };

                // Add hooks for output messages and errors
                terminalProcess.ErrorDataReceived += (sender, args) => {
                    Logger.Error("There was an error while installing forge", args.Data);
                    installationError = true;
                };

                terminalProcess.OutputDataReceived += (sender, args) => {
                    Logger.Info(args.Data);
                };

                // Wait for installation to finish
                Logger.Info($"Starting installation process...");
                terminalProcess.Start();
                terminalProcess.BeginOutputReadLine();
                terminalProcess.WaitForExit();
            };

            // Remove installer and it's logs
            Logger.Info($"Removing temporally installer files");
            File.Delete(installerPath);
            FileInfo installerLogs = new FileInfo($"forge-{version}-installer.jar.log");
            if (installerLogs.Exists) {
                installerLogs.Delete();
            }

            // Throw an error if installation failed
            if (installationError) {
                throw new Exception("Forge installation failed");
            }
        }

        public override void Uninstall(ServerMinecraftInstance instance, string modLoader) {
            if (modLoader != Packsly.Constants.ForgeModlaoder) {
                Logger.Error($"Mod loader handler is not compatible with modloader with name '{modLoader}'");
                return;
            }

            FileInfo forgeJar = GetForgeJarFile(instance);

            // Uninstall forge if there is any
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
