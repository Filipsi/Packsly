using Packsly.MultiMc.Launcher;
using System;
using Packsly.Core.Modpack;
using Packsly.Core.Adapter.Forge;
using System.IO;
using System.Text;
using Packsly.Core.Modpack.Provider;
using Packsly.Core.Common;
using Packsly.Curse.Content.Provider;
using Packsly.MultiMc.Adapter.Forge;
using Packsly.Core.Adapter.Override;

namespace Packsly.Cli {

    class Cli {

        static void Main(string[] args) {

            // Register modpack providers
            // IModpackProvider creates Modpack instance from string source
            PackslyRegistry.Register(new JsonModpackProvider());
            PackslyRegistry.Register(new JsonFileModpackProvider());
            PackslyRegistry.Register(new PastebinModpackProvider());
            PackslyRegistry.Register(new LatestCurseModpackProvider());
            PackslyRegistry.Register(new CurseModpackProvider());

            // Register launcher schemas
            // ILauncherSchema creates IMinecraftInstance from provided Modpack instnace
            PackslyRegistry.Register(new MmcLauncherSchema());

            // Register modules
            // Adapter applies arbitrary changes to provided IMinecraftInstance
            PackslyRegistry.Register(new OverrideAdapter());
            PackslyRegistry.Register(new MmcForgeAdapter());

            // Create MinecraftInstance from source
            // CreateTestJson();
            // PackslyFactory.MinecraftInstance.BuildFrom("modpack-testxy.json");
            // PackslyFactory.MinecraftInstance.BuildFrom("https://minecraft.curseforge.com/projects/invasion");

            Console.ReadKey();
        }

        static void CreateTestJson() {
            ModpackBuilder
                .Create("TestXY", "iron", "1.11.2")
                .AddMods(
                    "https://minecraft.curseforge.com/projects/just-enough-items-jei/files/2408687/download",
                    "https://minecraft.curseforge.com/projects/iron-chests/files/2389224/download")
                .AddForge("1.11.2-13.20.0.2284")
                .Build()
                .Save("modpack-testxy.json");
        }

    }

}
