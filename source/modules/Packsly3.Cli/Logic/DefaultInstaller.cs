﻿using NLog;
using Packsly3.Cli.Verbs;
using Packsly3.Core;

namespace Packsly3.Cli.Logic {

    internal static class DefaultInstaller {

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static void Run() {
            if (Packsly.Configuration.Workspace != null && Packsly.Configuration.Workspace.Exists) {
                Packsly.Launcher.Workspace = Packsly.Configuration.Workspace;
                logger.Info($"Workspace was set from configuration file to: {Packsly.Launcher.Workspace.FullName}");
            }

            logger.Info($"Using modpack source from configuration: {Packsly.Configuration.DefaultModpackSource}");

            Installer.Run(new InstallOptions {
                Source = Packsly.Configuration.DefaultModpackSource,
                Workspace = Packsly.Launcher.Workspace.FullName
            });
        }

    }

}
