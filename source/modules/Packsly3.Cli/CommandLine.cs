using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CommandLine;
using NLog;
using Packsly3.Cli.Common;
using Packsly3.Cli.Logic;
using Packsly3.Cli.Verbs;
using Packsly3.Core;

namespace Packsly3.Cli {

    internal class CommandLine {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static bool pauseWhenFinished;

        private static void Main(string[] args) {
            Logo.Print();

            try {
                Logger.Info("Welcome to Packsly3!");
                Run(args);
                Logger.Info("Thank you for using Packsly3!");

            } catch (Exception exception) {
                Logger.Fatal(exception);

                if (pauseWhenFinished) {
                    Console.ReadKey();
                }
            }

            Packsly.Lifecycle.EventBus.Publish(null, Core.Launcher.Instance.Lifecycle.PackslyExit);

            if (pauseWhenFinished || Debugger.IsAttached) {
                Console.ReadKey();
            }
        }

        private static void Run(IReadOnlyList<string> args) {
            if (args.Any()) {
                // Allows for drag & drop of the launcher onto Packsly executable to initiate installation
                FileInfo launcher = new FileInfo(args[0]);
                if (launcher.Exists) {
                    Logger.Debug("Running in drag & drop mode!");
                    pauseWhenFinished = true;

                    DirectoryInfo workspace = launcher.Extension.ToLower() == ".lnk"
                        ? ShortcutHelper.GetShortcutTarget(launcher).Directory
                        : launcher.Directory;

                    Installer.Run(new InstallOptions {
                        Source = Packsly.Configuration.DefaultModpackSource,
                        Workspace = workspace.FullName
                    });
                }
                // Run usage as a CLI tool
                else {
                    Logger.Debug("Running in command line mode!");
                    Parser.Default.ParseArguments<InstallOptions, LifecycleOptions>(args)
                        .WithParsed<InstallOptions>(Installer.Run)
                        .WithParsed<LifecycleOptions>(Lifecycle.Publish);
                }
            }
            // Run running as a application
            else {
                Logger.Debug("Running in application mode!");
                Logger.Info("Run with argument --help to see how to use this tool.");
                Logger.Info("Or you can start default installer by dragging the launcher executable onto this file.");
            }
        }

    }

}
