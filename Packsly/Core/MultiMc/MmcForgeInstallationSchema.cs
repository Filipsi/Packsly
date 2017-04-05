using Packsly.Core.Forge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Packsly.Common;
using System.IO;

namespace Packsly.Core.MultiMc {

    public class MmcForgeInstallationSchema : IForgeInstallationSchema {

        public Type Type {
            get {
                return typeof(MmcInstance);
            }
        }

        public bool Install(ForgeInstaller installer, IMinecraftInstance mcinstance, string forgeVersion) {
            // Download Forge if needed
            if(!installer.isForgeCached(forgeVersion))
                installer.DownloadForgeUniversal(forgeVersion);

            // Copy Forge to MultiMC libs if missing
            string forgeDestination = Path.Combine(mcinstance.LauncherLocation, @"libraries\net\minecraftforge\forge", forgeVersion, string.Format(installer.ForgeUniversalFormat, forgeVersion));
            if(!File.Exists(forgeDestination)) {
                Directory.CreateDirectory(Path.GetDirectoryName(forgeDestination));
                File.Copy(installer.GetCachedForge(forgeVersion), forgeDestination);
            }

            // Create patch for Forge version if there is not one
            if(!installer.isPatchCached(forgeVersion)) {
                ForgeLibrary[] libs = installer.ExtractLibraries(forgeVersion);
                new ForgePatchFile(installer.Cache.FullName, forgeVersion, libs).Save();
            }

            // Copy Forge patch to MultiMC instance
            string patchDirectory = Path.Combine(mcinstance.Location, "patches");
            if(!Directory.Exists(patchDirectory))
                Directory.CreateDirectory(patchDirectory);
            File.Copy(installer.GetCachedPatch(forgeVersion), Path.Combine(patchDirectory, "net.minecraftforge.json"));

            return true;
        }
    }

}
