using System;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using Newtonsoft.Json;
using Packsly3.Core;
using Packsly3.Core.FileSystem;
using Packsly3.Core.FileSystem.Impl;
using Packsly3.Core.Launcher;
using Packsly3.Core.Launcher.Adapter;
using Packsly3.Core.Launcher.Adapter.Impl;
using Packsly3.Core.Launcher.Instance;
using Packsly3.Core.Modpack;
using Packsly3.MultiMC.Launcher;
using Packsly3.MultiMC.FileSystem;

namespace Packsly3.Cli {

    internal class Program {

        private static readonly string[] PackslyLogo = {
            @" ____                 __               ___               __     ",
            @"/\  _`\              /\ \             /\_ \            /'__`\   ",
            @"\ \ \L\ \ __      ___\ \ \/'\     ____\//\ \    __  __/\_\L\ \ ",
            @" \ \ ,__/'__`\   /'___\ \ , <    /',__\ \ \ \  /\ \/\ \/_/_\_<_ ",
            @"  \ \ \/\ \L\.\_/\ \__/\ \ \\`\ /\__, `\ \_\ \_\ \ \_\ \/\ \L\ \",
            @"   \ \_\ \__/.\_\ \____\\ \_\ \_\/\____/ /\____\\/`____ \ \____/",
            @"    \/_/\/__/\/_/\/____/ \/_/\/_/\/___/  \/____/ `/___/> \/___/ ",
            @"                                                    /\___/      ",
            @"                                                    \/__/       ",
        };

        private static void PrintLogo() {
            for (int i = 0; i < PackslyLogo.Length; i++) {
                string line = PackslyLogo[i];
                Console.SetCursorPosition(Console.WindowWidth / 2 - line.Length / 2, i + 1);
                Console.WriteLine(line);
            }
        }

        private static void Main(string[] args) {
            PrintLogo();

            try {
                RunPacksly();
            }
            catch (Exception ex) {
                Console.WriteLine(FlattenException(ex));
            }

            Console.ReadKey();
        }

        private static void RunPacksly() {
            Console.WriteLine("Welcome to Packsly3!");
            Console.WriteLine();

            if (Packsly.Configuration.Workspace != null && Packsly.Configuration.Workspace.Exists) {
                Packsly.Launcher.Workspace = Packsly.Configuration.Workspace;
                Console.WriteLine($"Workspace was set from configuration file to: {Packsly.Launcher.Workspace}");
                Console.WriteLine();
            }

            Console.WriteLine("Detecting environment...");
            Console.WriteLine($" > Current environment name: {Packsly.Launcher.Name}");
            Console.WriteLine();

            string modpackSource = Packsly.Configuration.DefaultModpackSource;

            if (!string.IsNullOrEmpty(modpackSource)) {
                Console.WriteLine($"Gathering modpack definition from source '{modpackSource}' specified in configuration file...");

                if (File.Exists(modpackSource)) {
                    Console.WriteLine("Beginning installation from local modpack definition file...");

                    Packsly.Launcher.CreateInstanceFromModpack(
                        new FileInfo(modpackSource)
                    );
                } else if (Uri.IsWellFormedUriString(modpackSource, UriKind.Absolute)) {
                    Console.WriteLine("Beginning installation from remote modpack definition source...");

                    Packsly.Launcher.CreateInstanceFromModpack(
                        new Uri(modpackSource)
                    );
                } else {
                    Console.WriteLine("Modpack source specified in configuration file is invalid.");
                }
            }
        }

        public static string FlattenException(Exception exception) {
            StringBuilder builder = new StringBuilder();

            while (exception != null) {
                builder.AppendLine(exception.Message);
                builder.AppendLine(exception.StackTrace);

                exception = exception.InnerException;
            }

            return builder.ToString();
        }

    }

}
