using Packsly.Core.Modpack;
using Packsly.Core.Launcher;
using Packsly.Core.Tweak;
using Packsly.Curse.Content;
using Packsly.MultiMc.Forge;
using Packsly.MultiMc.Launcher;
using System;
using Packsly.Core.Common;
using Packsly.Core.Common.Configuration;

namespace Packsly.Cli {

    class Cli {

        static void Main(string[] args) {
            Settings.Instance.Save();

            // Register modpack providers
            // IModpackProvider creates Modpack instance from string source
            PackslyManager.Register(new CurseLatestModpackProvider());
            PackslyManager.Register(new CurseModpackProvider());

            // Register launcher schemas
            // ILauncherSchema creates IMinecraftInstance from provided Modpack instnace
            PackslyManager.Register(new MmcLauncherSchema());

            // Register modules
            // IModule applies arbitrary changes to provided IMinecraftInstance
            PackslyManager.Register(new MmcForgeTweak());

            // Create MinecraftInstance from source
            PackslyManager.CreateLauncherInstance("https://minecraft.curseforge.com/projects/invasion");

            Console.ReadKey();
        }

    }

}
