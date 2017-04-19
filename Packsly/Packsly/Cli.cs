using Packsly.Core.Content;
using Packsly.Core.Module;
using Packsly.Curse.Content;
using Packsly.MultiMc.Forge;
using Packsly.MultiMc.Launcher;
using System;

namespace Packsly.Cli {

    class Cli {

        static void Main(string[] args) {
            ModuleRegistry.Register(new MmcForgeModule());

            ModpackFactory.RegisterProvider(new CurseLatestModpackProvider());
            ModpackFactory.RegisterProvider(new CurseModpackProvider());

            MmcInstance.FromModpack(
                ModpackFactory.Acquire("https://minecraft.curseforge.com/projects/invasion")
            );

            Console.ReadKey();
        }

    }

}
