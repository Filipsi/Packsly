using System;
using System.Configuration;
using System.IO;
using Packsly3.Cli.Verbs;
using Packsly3.Core;

namespace Packsly3.Cli.Logic {

    internal static class InstallationHandler {

        public static void Handle(InstallOptions options) {
            ApplySettings(options);

            Console.WriteLine($"Gathering modpack definition from specified source '{options.Source}'...");

            if (options.IsSourceLocalFile) {
                Console.WriteLine("Beginning installation from local modpack definition file...");
                Packsly.Launcher.CreateInstanceFromModpack(
                    new FileInfo(options.Source)
                );

            } else if (options.IsSourceUrl) {
                Console.WriteLine("Beginning installation from remote modpack definition source...");
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
                    Console.WriteLine($"Workspace was set to: {Packsly.Launcher.Workspace}");
                    Console.WriteLine();
                }
                else {
                    throw new DirectoryNotFoundException("Specified workspace folder does not exists.");
                }
            }

            if (options.IsEnvironmentSpecified) {
                Packsly.Launcher.ForceEnviromentUsage(options.Environment);
                Console.WriteLine("Overriding environment auto detection...");
            }
            else {
                Console.WriteLine("Detecting environment...");
            }
            Console.WriteLine($" > Current environment name: {Packsly.Launcher.Name}");
            Console.WriteLine();
        }

    }

}
