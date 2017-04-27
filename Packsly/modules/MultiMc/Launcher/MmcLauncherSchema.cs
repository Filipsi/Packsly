using Packsly.Core.Launcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Packsly.Core.Modpack;
using System.IO;
using Packsly.Core.Common.Configuration;

namespace Packsly.MultiMc.Launcher {

    public class MmcLauncherSchema : ILauncherSchema {

        public string Name {
            get {
                return "MultiMc";
            }
        }

        public bool IsPresent(DirectoryInfo location) {
            return location.EnumerateFiles("MultiMC.exe").Any();
        }

        public string[] GetInstances(DirectoryInfo location) {
            return Directory.EnumerateDirectories(Path.Combine(location.FullName, "instances")).ToArray();
        }

        public IMinecraftInstance Create(ModpackInfo modpack) {
            MmcMinecraftInstance instance = new MmcMinecraftInstance(
                modpack.Id,
                modpack.Name,
                modpack.Icon,
                modpack.MinecraftVersion
            );

            instance.Save();

            string mcInstnacePath = Path.Combine(instance.Location, "minecraft");
            string mcModsPath = Path.Combine(mcInstnacePath, "mods");
            modpack.DownloadMods(mcModsPath);
            modpack.ExecuteTweaks(instance);
            modpack.ApplyOverrides(mcInstnacePath);

            DirectoryInfo Temp = Settings.Instance.Temp;
            if(Temp.Exists)
                Temp.Delete(true);

            return instance;
        }

    }

}
