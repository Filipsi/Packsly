﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using NLog;
using Packsly3.Cli.Common;
using Packsly3.Cli.Verbs;
using Packsly3.Core;
using Packsly3.Core.FileSystem.Impl;

namespace Packsly3.Cli.Logic {

    internal static class Installer {

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static void Run(InstallOptions options) {
            Logo.Print();
            ApplySettings(options);

            if (Packsly.Launcher.CanEmbed && !IsPackslyEmbeded(options.Workspace)) {
                logger.Info("Embedding packsly to launcher directory...");
                EmbedPacksly(options.Workspace);
            }

            if (!options.IsSourceSpecified) {
                logger.Info("Using modpack source from configuration.");
                options.Source = Packsly.Configuration.DefaultModpackSource;
            }

            logger.Info($"Current modpack source: {options.Source}");
            if (options.IsSourceLocalFile) {
                logger.Info("Beginning installation from local modpack definition file...");
                Packsly.Launcher.CreateInstanceFromModpack(new FileInfo(options.Source));

            } else if (options.IsSourceUrl) {
                logger.Info("Beginning installation from remote modpack definition source...");
                Packsly.Launcher.CreateInstanceFromModpack(new Uri(options.Source));

            } else {
                throw new ConfigurationErrorsException($"Specified modpack source '{options.Source}' is not valid file path or URL address.");
            }

            logger.Info("Thank you for using Packsly3!");
        }

        private static void ApplySettings(InstallOptions options) {
            if (options.IsWorkspaceSpecified) {
                if (options.IsWorkspaceValid) {
                    Packsly.Launcher.Workspace = new DirectoryInfo(options.Workspace);
                    logger.Info($"Workspace was set to: {Packsly.Launcher.Workspace}");

                } else {
                    throw new DirectoryNotFoundException($"Specified '{options.Workspace}' workspace folder does not exist.");
                }
            } else {
                DirectoryInfo workspaceFolder = new DirectoryInfo(Packsly.Configuration.Workspace);
                if (workspaceFolder.Exists) {
                    Packsly.Launcher.Workspace = workspaceFolder;
                    options.Workspace = Packsly.Launcher.Workspace.FullName;
                    logger.Info($"Using workspace from configuration file: {Packsly.Launcher.Workspace}");
                } else {
                    throw new DirectoryNotFoundException($"Workspace '{workspaceFolder.FullName}' specified in configuration file does not exists.");
                }
            }

            if (options.IsEnvironmentSpecified) {
                Packsly.Launcher.ForceEnviromentUsage(options.Environment);
                logger.Info("Overriding environment auto detection...");
                logger.Info($"Current environment name: {Packsly.Launcher.Name}");

            } else {
                logger.Info($"Current environment name: {Packsly.Launcher.Name}");
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
                logger.Debug($"Old packsly version folder detected in workspace {location}, removing.");
                packslyFolder.Delete(true);
            }

            return packslyFolder.Exists;
        }

        private static void EmbedPacksly(string workspace) {
            DirectoryInfo destination = new DirectoryInfo(Path.Combine(workspace, "packsly"));
            logger.Debug($"Embedding packsly to {destination}");
            destination.Create();

            string[] allowedExtentions = {
                ".dll", ".exe", ".xml", ".config"
            };

            IEnumerable<FileInfo> allowedFiles = Packsly.AplicationDirectory
                .EnumerateFiles()
                .Where((file) => allowedExtentions.Contains(Path.GetExtension(file.FullName)));

            foreach (FileInfo fileToCopy in allowedFiles) {
                logger.Debug($"Coping file {fileToCopy.Name}..");
                fileToCopy.CopyTo(Path.Combine(destination.FullName, fileToCopy.Name));
            }

            logger.Debug("Creating configuration file for embedded copy...");
            PackslyConfig embeddConfig = new PackslyConfig(destination) {
                Workspace = "./.."
            };

            embeddConfig.Save();
        }

    }

}
