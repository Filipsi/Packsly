using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using CommandLine;
using NLog;
using Packsly3.Cli.Common;
using Packsly3.Cli.Logic;
using Packsly3.Cli.Verbs;
using Packsly3.Core;

namespace Packsly3.Cli {

    internal static class CommandLine {

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static bool pauseWhenFinished;

        private static void Main(string[] args) {
            try {
                // Fix current directory path to make it same on Windows and Linux
                string currentAssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                logger.Debug($"Root path of executing assembly is: {currentAssemblyPath}");
                logger.Debug("Overriding current folder with executing assembly path...");
                Directory.SetCurrentDirectory(currentAssemblyPath ?? throw new InvalidOperationException("Unable get executing assembly location!"));
                logger.Debug($"Current directory is: {Directory.GetCurrentDirectory()}");

                Run(args);

            } catch (Exception exception) {
                logger.Fatal(exception);

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
            if (args.Count > 0) {
                // Allows for drag & drop of the launcher onto Packsly executable to initiate installation
                FileInfo launcher = new FileInfo(args[0]);

                if (launcher.Exists) {
                    logger.Debug("Running in drag & drop mode!");
                    pauseWhenFinished = true;

                    DirectoryInfo workspace = string.Equals(launcher.Extension, ".lnk", StringComparison.OrdinalIgnoreCase)
                        ? ShortcutHelper.GetShortcutTarget(launcher).Directory
                        : launcher.Directory;

                    Installer.Run(new InstallOptions {
                        Source = Packsly.Configuration.DefaultModpackSource,
                        Workspace = workspace.FullName
                    });
                }
                // Run usage as a CLI tool
                else {
                    logger.Debug("Running in command line mode!");
                    Parser.Default.ParseArguments<InstallOptions, LifecycleOptions>(args)
                        .WithParsed<InstallOptions>(Installer.Run)
                        .WithParsed<LifecycleOptions>(Lifecycle.Publish);
                }
            }
            // Run running as a application
            else {
                logger.Debug("Running in application mode!");
                pauseWhenFinished = true;
                Installer.Run(new InstallOptions {
                    Source = Packsly.Configuration.DefaultModpackSource
                });
            }
        }

    }

}
