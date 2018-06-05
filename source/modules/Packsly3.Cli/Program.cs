using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
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

            Launcher.Workspace = new DirectoryInfo("D:\\Games\\MultiMC");

            Console.WriteLine("Detecting environment...");
            Console.WriteLine($" > Current handler: {Launcher.Current}");
            Console.WriteLine(string.Empty);

            /*
            IMinecraftInstance instance = MinecraftInstanceFactory.CreateFromModpack(
                new FileInfo(
                    Path.Combine(Directory.GetCurrentDirectory(), "modpack.json")
                )
            );
            */

            Lifecycle.Dispatcher.Publish(Launcher.GetInstance("modpack") ,Lifecycle.PreLaunch);

            Console.ReadKey();

        }

    }

}
