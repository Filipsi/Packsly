using Packsly.Core.Launcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Packsly.Core.Content;
using System.IO;
using Packsly.Core.Configuration;

namespace Packsly.MultiMc.Launcher {

    public class MmcLauncherSchema : ILauncherSchema {

        public string Name {
            get {
                return "MultiMc";
            }
        }

        public Type MinecraftInstanceType {
            get {
                return typeof(MmcInstance);
            }
        }

        public bool Check(DirectoryInfo location) {
            return location.EnumerateFiles("MultiMC.exe").Any();
        }

        public string[] GetInstances(DirectoryInfo location) {
            return Directory.EnumerateDirectories(Path.Combine(location.FullName, "instances")).ToArray();
        }

        public IMinecraftInstance Create(Modpack modpack) {
            MmcInstance instance = new MmcInstance(
                modpack.Id,
                modpack.Name,
                modpack.Icon,
                modpack.MinecraftVersion
            );

            instance.Save();

            DirectoryInfo Temp = Settings.Instance.Temp;
            string mcInstnace = Path.Combine(instance.Location, "minecraft");

            modpack.DownloadMods(Path.Combine(mcInstnace, "mods"));
            modpack.ExecuteModules(instance);
            modpack.ApplyOverrides(mcInstnace);

            if(Temp.Exists) Temp.Delete(true);
            return instance;
        }

    }

}
