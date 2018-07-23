using System;
using System.Collections.Generic;
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

        private static void Main(string[] args) {
            Logo.Print();

            try {
                Logger.Info("Welcome to Packsly3!");
                Run(args);
                Logger.Info("Execution finished, goodbye!");
            }
            catch (Exception exception) {
                Logger.Fatal(exception);
            }

            // While in debug mode, prevent closing after everything is finished
            if (System.Diagnostics.Debugger.IsAttached) {
                Console.ReadKey();
            }
        }

        private static void Run(IReadOnlyList<string> args) {
            if (args.Any()) {
                // Allows for drag & drop of the launcher onto Packsly executable to initiate installation
                FileInfo launcher = new FileInfo(args[0]);
                if (launcher.Exists && launcher.Directory != null && launcher.Directory.Exists) {
                    Logger.Debug("Running in drag & drop mode");
                    Packsly.Launcher.Workspace = launcher.Directory;
                    DefaultHandler.Handle();
                }
                // Handle usage as a CLI tool
                else {
                    Logger.Debug("Running in command line mode");
                    Parser.Default.ParseArguments<InstallOptions, LifecycleOptions>(args)
                        .WithParsed<InstallOptions>(InstallationHandler.Handle)
                        .WithParsed<LifecycleOptions>(LifecycleHandler.Handle);
                }
            }
            // Handle running as a application
            else {
                Logger.Debug("Running in application mode");
                Logger.Info("Since no command line arguments were specified, do you want to run default installation? (y/n)");
                string responce = Console.ReadLine();
                if (!string.IsNullOrEmpty(responce) && responce.Equals("y")) {
                    Logger.Info("Running default installation.");
                    DefaultHandler.Handle();
                }
                else {
                    Logger.Info("Default installation was aborted.");
                }
            }
        }

    }

}
