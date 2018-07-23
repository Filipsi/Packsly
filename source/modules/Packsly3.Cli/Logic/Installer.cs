using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NLog;
using Packsly3.Cli.Verbs;
using Packsly3.Core;
using Packsly3.Core.Common.Json;
using Packsly3.Core.FileSystem.Impl;

namespace Packsly3.Cli.Logic {

    internal static class Installer {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Run(InstallOptions options) {
            ApplySettings(options);

            if (!IsPackslyEmbeded(options.Workspace)) {
                Logger.Info("Embedding packsly to launcher directory...");
                EmbedPacksly(options.Workspace);
            }

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

        private static bool IsPackslyEmbeded(string location) {
            DirectoryInfo packslyFolder = new DirectoryInfo(
                Path.Combine(location, "packsly")
            );

            bool hasOldPacksly = File.Exists(
                Path.Combine(packslyFolder.FullName, "Packsly.exe")
            );

            // ReSharper disable once InvertIf
            if (hasOldPacksly) {
                Logger.Debug($"Old packsly version folder detected in workspace {location}, removing.");
                packslyFolder.Delete(true);
            }

            return packslyFolder.Exists;
        }

        private static void EmbedPacksly(string workspace) {
            DirectoryInfo destination = new DirectoryInfo(Path.Combine(workspace, "packsly"));
            Logger.Debug($"Embedding packsly to {destination}");
            destination.Create();

            string[] allowedExtentions = {
                ".dll", ".exe", ".xml", ".config"
            };

            IEnumerable<FileInfo> allowedFiles = Packsly.AplicationDirectory
                .EnumerateFiles()
                .Where((file) => allowedExtentions.Contains(Path.GetExtension(file.FullName)));

            foreach (FileInfo fileToCopy in allowedFiles) {
                Logger.Debug($"Coping file {fileToCopy.Name}..");
                fileToCopy.CopyTo(Path.Combine(destination.FullName, fileToCopy.Name));
            }

            Logger.Debug("Creating configuration file for embedded copy...");
            PackslyConfig embeddConfig = new PackslyConfig(destination) {
                SerializerSettings =  new JsonSerializerSettings {
                    ContractResolver = new LowercaseContractResolver(),
                    ObjectCreationHandling = ObjectCreationHandling.Replace,
                    Converters = {
                        new RelativePathConverter {
                            Root = destination.FullName
                        }
                    }
                },
                Workspace = new DirectoryInfo(workspace)
            };
            embeddConfig.Save();
        }

    }

}
