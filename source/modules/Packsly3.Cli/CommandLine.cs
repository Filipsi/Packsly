using System;
using System.Linq;
using CommandLine;
using Packsly3.Cli.Common;
using Packsly3.Cli.Logic;
using Packsly3.Cli.Verbs;

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
            }

            if (System.Diagnostics.Debugger.IsAttached) {
                Console.ReadKey();
            }
        }

        private static void ParseAndExecute(string[] args) {
            if (args.Any()) {
                Parser.Default.ParseArguments<InstallOptions, LifecycleOptions>(args)
                    .WithParsed<InstallOptions>(InstallationHandler.Handle)
                    .WithParsed<LifecycleOptions>(LifecycleHandler.Handle);
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
