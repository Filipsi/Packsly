using System;
using System.IO;
using System.Linq;
using CommandLine;
using Packsly3.Cli.Common;
using Packsly3.Cli.Logic;
using Packsly3.Cli.Verbs;
using Packsly3.Core;

namespace Packsly3.Cli {

    internal class CommandLine {

        private static void Main(string[] args) {
            Logo.Print();

            try {
                Console.WriteLine("Welcome to Packsly3!");
                ParseAndExecute(args);
                Console.WriteLine("All done!");
            }
            catch (Exception exception) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(exception.Message);
                Console.ResetColor();
                Console.ReadKey();
            }

            if (System.Diagnostics.Debugger.IsAttached) {
                Console.ReadKey();
            }
        }

        private static void ParseAndExecute(string[] args) {
            Console.ReadKey();
            if (args.Any()) {
                FileInfo launcher = new FileInfo(args[0]);
                if (launcher.Exists && launcher.Directory != null && launcher.Directory.Exists) {
                    Packsly.Launcher.Workspace = launcher.Directory;
                    DefaultHandler.Handle();
                }
                else {
                    Parser.Default.ParseArguments<InstallOptions, LifecycleOptions>(args)
                        .WithParsed<InstallOptions>(InstallationHandler.Handle)
                        .WithParsed<LifecycleOptions>(LifecycleHandler.Handle);
                }
            }
            else {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Well, since no CLI arguments were specified, let's try to run installation with default settings...");
                Console.ResetColor();
                Console.WriteLine();

                DefaultHandler.Handle();
            }
        }

    }

}
