using Packsly.Curse.Content;
using Packsly.MultiMc.Forge;
using Packsly.MultiMc.Launcher;
using System;
using Packsly.Core.Modpack;
using Packsly.Core.Forge;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using Packsly.Core.Modpack.Provider;
using Packsly.Core.Common;

namespace Packsly.Cli {

    class Cli {

        static void Main(string[] args) {
            // Register modpack providers
            // IModpackProvider creates Modpack instance from string source
            PackslyRegistry.Register(new JsonFileModpackProvider());
            PackslyRegistry.Register(new JsonModpackProvider());
            PackslyRegistry.Register(new CurseLatestModpackProvider());
            PackslyRegistry.Register(new CurseModpackProvider());

            // Register launcher schemas
            // ILauncherSchema creates IMinecraftInstance from provided Modpack instnace
            PackslyRegistry.Register(new MmcLauncherSchema());

            // Register modules
            // IModule applies arbitrary changes to provided IMinecraftInstance
            PackslyRegistry.Register(new MmcForgeTweak());

            // Create MinecraftInstance from source
            // PackslyFactory.MinecraftInstance.BuildFrom("https://minecraft.curseforge.com/projects/invasion");
            CreateTestJson();
            PackslyFactory.MinecraftInstance.BuildFrom("modpack-testxy.json");

            Console.ReadKey();
        }

        static void CreateTestJson() {
            ModpackInfo mi = new ModpackInfo("testxy", "TestXY", "iron", "1.11.2", "0.0.xy",
                new ModInfo("https://minecraft.curseforge.com/projects/just-enough-items-jei/files/2408687/download"),
                new ModInfo("https://minecraft.curseforge.com/projects/iron-chests/files/2389224/download")
            );

            mi.AddTweaks(new ForgeAdapterContext("1.11.2-13.20.0.2284"));
            mi.AddOverrides(string.Empty, @"config\jei\jei.cfg");

            string raw = JsonConvert.SerializeObject(mi, Formatting.Indented);
            using(FileStream writer = File.Open("modpack-testxy.json", FileMode.Create)) {
                byte[] buffer = Encoding.UTF8.GetBytes(raw);
                writer.Write(buffer, 0, buffer.Length);
            }
        }

    }

}
