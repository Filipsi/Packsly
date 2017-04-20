using Packsly.Curse.Content;
using Packsly.MultiMc.Forge;
using Packsly.MultiMc.Launcher;
using System;
using Packsly.Core.Common.Registry;
using Packsly.Core.Common.Factory;

namespace Packsly.Cli {

    class Cli {

        static void Main(string[] args) {
            // Register modpack providers
            // IModpackProvider creates Modpack instance from string source
            PackslyRegistry.Register(new CurseLatestModpackProvider());
            PackslyRegistry.Register(new CurseModpackProvider());

            // Register launcher schemas
            // ILauncherSchema creates IMinecraftInstance from provided Modpack instnace
            PackslyRegistry.Register(new MmcLauncherSchema());

            // Register modules
            // IModule applies arbitrary changes to provided IMinecraftInstance
            PackslyRegistry.Register(new MmcForgeTweak());

            // Create MinecraftInstance from source
            PackslyFactory.LauncherInstance.BuildFrom("https://minecraft.curseforge.com/projects/invasion");

            Console.ReadKey();
        }

    }

}
