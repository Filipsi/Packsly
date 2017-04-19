using Packsly.Core.Content;
using Packsly.Core.Launcher;
using Packsly.Core.Module;
using Packsly.Curse.Content;
using Packsly.MultiMc.Forge;
using Packsly.MultiMc.Launcher;
using System;

namespace Packsly.Cli {

    class Cli {

        static void Main(string[] args) {
            // Register modpack providers
            // IModpackProvider creates Modpack instance from string source
            ModpackFactory.RegisterProvider(new CurseLatestModpackProvider());
            ModpackFactory.RegisterProvider(new CurseModpackProvider());

            // Register launcher schemas
            // ILauncherSchema creates IMinecraftInstance from provided Modpack instnace
            MinecraftInstanceFactory.RegisterSchema(new MmcLauncherSchema());

            // Register modules
            // IModule applies arbitrary changes to provided IMinecraftInstance
            ModuleRegistry.Register(new MmcForgeModule());

            // Create MinecraftInstance from source
            MinecraftInstanceFactory.CreateFrom("https://minecraft.curseforge.com/projects/invasion");

            Console.ReadKey();
        }

    }

}
