﻿using NLog;
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
using Packsly3.Core.Launcher.Modloader.Impl;

namespace Packsly3.Server.Launcher.Modloader {

    [Register]
    public class ServerModLoaderHandler : BasicModLoaderHandler<ServerMinecraftInstance> {

        #region Fields

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Regex forgeJarFilePattern = new Regex(
            pattern: @"forge-(\d{1,2}\.\d{1,2}\.\d{1,2})-(\d{1,2}\.\d{1,2}\.\d{1,2}\.\d{1,4})-\w+\.jar",
            options: RegexOptions.Compiled
        );

        #endregion

        #region IModLoaderHandler

        public override bool IsCompatible(string modLoader)
           => modLoader == Packsly.Constants.ForgeModloader;

        public override void DetectModLoaders(ServerMinecraftInstance instance, List<ModLoaderInfo> modLoaders) {
            FileInfo forgeJar = GetForgeJarFile(instance);

            if (forgeJar != null) {
                modLoaders.Add(
                    new ModLoaderInfo(
                        name: Packsly.Constants.ForgeModloader,
                        version: forgeJarFilePattern.Match(forgeJar.Name).Groups[2].Value
                    )
                );
            }
        }

        public override void Install(ServerMinecraftInstance instance, string modLoader, string version) {
            if (modLoader != Packsly.Constants.ForgeModloader) {
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

                string forgeInstallerUrl = GetForgeInstallerUrl(instance.MinecraftVersion, version);
                Logger.Info($" - Source url address: {forgeInstallerUrl}");

                client.DownloadFile(
                    address: forgeInstallerUrl,
                    fileName: installerPath
                );

                Logger.Info($"Forge installer was saved to {installerPath}");
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
                    FileName = Packsly.IsLinux ? "/bin/bash" : "cmd.exe",
                    Arguments = (Packsly.IsLinux ? "-c" : "/c") + $" \"java -jar {installerPath} --installServer nogui\""
                };

                // Add hooks for output messages and errors
                terminalProcess.ErrorDataReceived += (sender, args) => {
                    Logger.Error("There was an error while running forge installer: ", args.Data);
                    installationError = true;
                };

                terminalProcess.OutputDataReceived += (sender, args) => {
                    Logger.Info(args.Data);
                };

                // Wait for installation to finish
                Logger.Info($"Starting forge installer process...");
                terminalProcess.Start();
                terminalProcess.BeginOutputReadLine();
                terminalProcess.WaitForExit();
                Logger.Info($"Forge installer process finished.");
            };

            // Remove temporally files
            Logger.Info($"Removing temporally files...");
            Logger.Info($" - Removing {installerPath}");
            File.Delete(installerPath);
            foreach(FileInfo logFile in instance.Location.GetFiles("forge-*-installer.jar.log")) {
                if (logFile.Exists) {
                    Logger.Info($" - Removing {logFile.FullName}");
                    logFile.Delete();
                }
            }

            // Throw an error if installation failed
            // Exception is thrown here to also remove temporally files if there is an error
            if (installationError) {
                throw new Exception("Forge installation failed");
            }
        }

        public override void Uninstall(ServerMinecraftInstance instance, string modLoader) {
            if (modLoader != Packsly.Constants.ForgeModloader) {
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
            return $"http://files.minecraftforge.net/maven/net/minecraftforge/forge/{version}/forge-{version}-installer.jar";
        }

        #endregion

    }

}
