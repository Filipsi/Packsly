using Ionic.Zip;
using Newtonsoft.Json.Linq;
using Packsly.Core.Common.Configuration;
using Packsly.Core.Adapter.Forge;
using Packsly.MultiMc.Launcher;
using System.Collections.Generic;
using System.IO;

namespace Packsly.MultiMc.Adapter.Forge {

    public class MmcForgeAdapter : ForgeAdapter<MmcMinecraftInstance> {

        protected override void Execute(MmcMinecraftInstance instance, ForgeAdapterContext args) {
            string version = args.Version;

            // Download Forge if needed
            if(!IsForgeCached(version))
                DownloadForgeUniversal(args.Url, version);

            // Copy Forge to MultiMC libs if missing
            string forgeDestination = Path.Combine(Settings.Instance.Launcher.FullName, @"libraries\net\minecraftforge\forge", version, string.Format(ForgeUniversalFormat, version));
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

            // TODO: Copy only if version changed
            File.Copy(GetCachedPatch(version), Path.Combine(patchDirectory, "net.minecraftforge.json"), true);
        }

        protected ForgeLibrary[] ExtractLibraries(string version) {
            if(Temp.Exists)
                Temp.Delete(true);

            Temp.Create();

            using(ZipFile jar = new ZipFile(GetCachedForge(version)))
                jar.ExtractAll(Temp.FullName);

            string fileContent;
            using(StreamReader reader = File.OpenText(Path.Combine(Temp.FullName, ForgeVersionFile)))
                fileContent = reader.ReadToEnd();

            Temp.Delete(true);

            JArray raw = JObject.Parse(fileContent).Value<JArray>("libraries");

            List<ForgeLibrary> libs = new List<ForgeLibrary>();
            foreach(JObject entry in raw)
                libs.Add(ForgeLibrary.FromJson(entry));

            return libs.ToArray();
        }

        #region Cache

        protected string GetCachedPatch(string version) {
            return Path.Combine(Cache.FullName, string.Format(ForgePatchFile.FileFormat, version));
        }

        protected bool isPatchCached(string version) {
            return File.Exists(GetCachedPatch(version));
        }

        #endregion

    }

}
