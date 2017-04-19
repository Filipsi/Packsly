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
            ModpackFactory.RegisterProvider(new CurseLatestModpackProvider());
            ModpackFactory.RegisterProvider(new CurseModpackProvider());

            // Register modules
            ModuleRegistry.Register(new MmcForgeModule());

            // Register launcher schemas
            MinecraftInstanceFactory.RegisterSchema(new MmcLauncherSchema());

            MinecraftInstanceFactory.CreateFrom("https://minecraft.curseforge.com/projects/invasion");

            Console.ReadKey();
        }

    }

}
