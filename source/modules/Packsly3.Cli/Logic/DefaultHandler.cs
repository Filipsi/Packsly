using System;
using Packsly3.Cli.Verbs;
using Packsly3.Core;

namespace Packsly3.Cli.Logic {

    internal static class DefaultHandler {

        public static void Handle() {
            if (Packsly.Configuration.Workspace != null && Packsly.Configuration.Workspace.Exists) {
                Packsly.Launcher.Workspace = Packsly.Configuration.Workspace;
                Console.WriteLine($"Workspace was set from configuration file to: {Packsly.Launcher.Workspace}");
                Console.WriteLine();
            }

            Console.WriteLine("Modpack source was not specified, using default location from configuration...");
            Console.WriteLine($" > Modpack source: {Packsly.Configuration.DefaultModpackSource}");
            Console.WriteLine();

            InstallationHandler.Handle(new InstallOptions {
                Source = Packsly.Configuration.DefaultModpackSource
            });
        }

    }

}
