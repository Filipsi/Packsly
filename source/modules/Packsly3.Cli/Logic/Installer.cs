using System;
using System.Configuration;
using System.IO;
using NLog;
using Packsly3.Cli.Verbs;
using Packsly3.Core;

namespace Packsly3.Cli.Logic {

    internal static class Installer {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Run(InstallOptions options) {
            ApplySettings(options);

            Logger.Info($"Using modpack definition from '{options.Source}'...");
            if (options.IsSourceLocalFile) {
                Logger.Info("Beginning installation from local modpack definition file...");
                Packsly.Launcher.CreateInstanceFromModpack(
                    new FileInfo(options.Source)
                );

            } else if (options.IsSourceUrl) {
                Logger.Info("Beginning installation from remote modpack definition source...");
                Packsly.Launcher.CreateInstanceFromModpack(
                    new Uri(options.Source)
                );

            } else {
                throw new ConfigurationErrorsException("Specified Modpack source is not valid file path or url address.");
            }
        }

        private static void ApplySettings(InstallOptions options) {
            if (options.IsWorkspaceSpecified) {
                if (options.IsWorkspaceValid) {
                    Packsly.Launcher.Workspace = new DirectoryInfo(options.Workspace);
                    Logger.Info($"Workspace was set to: {Packsly.Launcher.Workspace}");
                }
                else {
                    throw new DirectoryNotFoundException("Specified workspace folder does not exist.");
                }
            }

            if (options.IsEnvironmentSpecified) {
                Packsly.Launcher.ForceEnviromentUsage(options.Environment);
                Logger.Info("Overriding environment auto detection...");
                Logger.Info($"Current environment name: {Packsly.Launcher.Name}");
            }
            else {
                Logger.Info($"Current environment name: {Packsly.Launcher.Name}");
            }
        }

    }

}
