using Core.Forge;
using Packsly.Core.Forge;
using Packsly.MultiMc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackslyMultiMc {

    public class MmcForgeModule : ForgeModule<MmcInstance> {

        public override void Execute(MmcInstance instance, ForgeModuleArgs args) {
            string version = args.Version;

            // Download Forge if needed
            if(!isForgeCached(version))
                DownloadForgeUniversal(version);

            // Copy Forge to MultiMC libs if missing
            string forgeDestination = Path.Combine(instance.LauncherLocation, @"libraries\net\minecraftforge\forge", version, string.Format(ForgeUniversalFormat, version));
            if(!File.Exists(forgeDestination)) {
                Directory.CreateDirectory(Path.GetDirectoryName(forgeDestination));
                File.Copy(GetCachedForge(version), forgeDestination);
            }

            // Create patch for Forge version if there is not one
            if(!isPatchCached(version)) {
                ForgeLibrary[] libs = ExtractLibraries(version);
                new ForgePatchFile(Cache.FullName, version, libs).Save();
            }

            // Copy Forge patch to MultiMC instance
            string patchDirectory = Path.Combine(instance.Location, "patches");
            if(!Directory.Exists(patchDirectory))
                Directory.CreateDirectory(patchDirectory);

            // TODO: Copy if version changed
            File.Copy(GetCachedPatch(version), Path.Combine(patchDirectory, "net.minecraftforge.json"), true);
        }

    }

}
