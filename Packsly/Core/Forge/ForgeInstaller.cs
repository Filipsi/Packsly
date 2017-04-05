using Ionic.Zip;
using Newtonsoft.Json.Linq;
using Packsly.Common;
using Packsly.Core.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Forge {

    public class ForgeInstaller {

        #region Properties

        private IForgeInstallationSchema[] Schemas { set; get; }

        #endregion

        #region Constants

        public readonly DirectoryInfo Cache = Settings.Instance.Cache;

        public readonly DirectoryInfo Temp = Settings.Instance.Temp;

        public string ForgeUniversalFormat = "forge-{0}-universal.jar";

        public string ForgeVersionFile = "version.json";

        public string ForgeFilesUrl = "http://files.minecraftforge.net/maven/net/minecraftforge/forge";

        #endregion

        #region Constructor

        public ForgeInstaller(params IForgeInstallationSchema[] schemas) {
            if(!Cache.Exists)
                Cache.Create();

            Schemas = schemas;
        }

        #endregion

        #region Logic

        public void Install(IMinecraftInstance mcinsntace, string version) {
            IForgeInstallationSchema schema = Schemas.FirstOrDefault(s => s.Type.Equals(mcinsntace.GetType()));

            if(schema == null)
                throw new Exception($"Forge instalation schema for Minecraft instance type '{mcinsntace.GetType()}' does not exist");

            schema.Install(this, mcinsntace, version);
        }

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

            List <ForgeLibrary> libs = new List<ForgeLibrary>();
            foreach(JObject entry in raw)
                libs.Add(ForgeLibrary.FromJson(entry));

            return libs.ToArray();
        }

        #endregion

        #region Cache

        public string GetCachedForge(string version) {
            return Path.Combine(Cache.FullName, string.Format(ForgeUniversalFormat, version));
        }

        public string GetCachedPatch(string version) {
            return Path.Combine(Cache.FullName, string.Format(ForgePatchFile.FileFormat, version));
        }

        public bool isForgeCached(string version) {
            return File.Exists(GetCachedForge(version));
        }

        public bool isPatchCached(string version) {
            return File.Exists(GetCachedPatch(version));
        }

        #endregion

    }

}
