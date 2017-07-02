using Packsly.Core.Launcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Packsly.Core.Modpack;
using System.IO;
using Packsly.Core.Common.Configuration;
using Packsly.Core.Modpack.Model;
using Packsly.Core.Adapter;

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
            string instancesFolder = Path.Combine(location.FullName, "instances");

            if(!Directory.Exists(instancesFolder))
                Directory.CreateDirectory(instancesFolder);

            return Directory.EnumerateDirectories(instancesFolder).ToArray();
        }

        public IMinecraftInstance Create(ModpackInfo modpack) {
            MmcMinecraftInstance instance = new MmcMinecraftInstance(
                modpack.Id,
                modpack.Name,
                modpack.Icon,
                modpack.MinecraftVersion
            );

            instance.Save();

            // Paths
            string mcInstnacePath = Path.Combine(instance.Location, "minecraft");
            string mcModsPath = Path.Combine(mcInstnacePath, "mods");

            // Download mods
            foreach(ModInfo mod in modpack.Mods)
                mod.Download(mcModsPath);

            // Execute adapters
            foreach(IAdapterContext args in modpack.Adapters)
                AdapterRegistry.Execute(instance, args);

            // Remove temp directory
            DirectoryInfo Temp = Settings.Instance.Temp;
            if(Temp.Exists)
                Temp.Delete(true);

            return instance;
        }

    }

}
