using System;
using NLog;
using Packsly3.Cli.Verbs;
using Packsly3.Core;

namespace Packsly3.Cli.Logic {

    internal static class DefaultHandler {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Handle() {
            if (Packsly.Configuration.Workspace != null && Packsly.Configuration.Workspace.Exists) {
                Packsly.Launcher.Workspace = Packsly.Configuration.Workspace;
                Logger.Info($"Workspace was set from configuration file to: {Packsly.Launcher.Workspace.FullName}");
            }

            Logger.Info("Modpack source was not specified, using default location from configuration...");
            Logger.Info($"Modpack source: {Packsly.Configuration.DefaultModpackSource}");

            InstallationHandler.Handle(new InstallOptions {
                Source = Packsly.Configuration.DefaultModpackSource
            });
        }

    }

}
