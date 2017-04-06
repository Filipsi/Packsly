using Ionic.Zip;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Packsly.Common;
using Packsly.Core.Configuration;
using Packsly.Core.Forge;
using Packsly.Core.Module;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System;

namespace Core.Forge {

    public class ForgeModuleArgs {

        [JsonProperty("version")]
        public string Version { private set; get; }

        public ForgeModuleArgs(string version) {
            Version = version;
        }

    }

    public abstract class ForgeModule<T> : Module<T, ForgeModuleArgs> where T : IMinecraftInstance {

        #region Constants

        protected readonly DirectoryInfo Cache = Settings.Instance.Cache;

        protected readonly DirectoryInfo Temp = Settings.Instance.Temp;

        protected const string ForgeUniversalFormat = "forge-{0}-universal.jar";

        protected const string ForgeVersionFile = "version.json";

        protected const string ForgeFilesUrl = "http://files.minecraftforge.net/maven/net/minecraftforge/forge";

        #endregion

        #region Module

        public override string Id {
            get {
                return "forge";
            }
        }

        #endregion

        #region Logic

        public void DownloadForgeUniversal(string version) {
            string jarPath = GetCachedForge(version);
            using(WebClient client = new WebClient())
                client.DownloadFile(Path.Combine(ForgeFilesUrl, version, string.Format(ForgeUniversalFormat, version)).Replace("\\", "/"), jarPath);
        }

        public ForgeLibrary[] ExtractLibraries(string version) {
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

        #endregion

        #region Cache

        protected string GetCachedForge(string version) {
            return Path.Combine(Cache.FullName, string.Format(ForgeUniversalFormat, version));
        }

        protected string GetCachedPatch(string version) {
            return Path.Combine(Cache.FullName, string.Format(ForgePatchFile.FileFormat, version));
        }

        protected bool isForgeCached(string version) {
            return File.Exists(GetCachedForge(version));
        }

        protected bool isPatchCached(string version) {
            return File.Exists(GetCachedPatch(version));
        }

        #endregion

    }

}
