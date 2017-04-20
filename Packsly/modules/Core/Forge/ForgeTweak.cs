using Packsly.Core.Tweak;
using System.IO;
using System.Net;
using Packsly.Core.Launcher;
using Packsly.Core.Common.Configuration;

namespace Packsly.Core.Forge {

    public abstract class ForgeTweak<T> : Tweak<T, ForgeTweakArgs> where T : IMinecraftInstance {

        #region Constants

        protected readonly DirectoryInfo Cache = Settings.Instance.Cache;

        protected readonly DirectoryInfo Temp = Settings.Instance.Temp;

        protected const string ForgeUniversalFormat = "forge-{0}-universal.jar";

        protected const string ForgeVersionFile = "version.json";

        #endregion

        #region Tweak

        public override string Type {
            get {
                return "forge";
            }
        }

        #endregion

        #region Logic

        public void DownloadForgeUniversal(string url, string version) {
            string jarPath = GetCachedForge(version);
            using(WebClient client = new WebClient())
                client.DownloadFile(Path.Combine(url, version, string.Format(ForgeUniversalFormat, version)).Replace("\\", "/"), jarPath);
        }

        #endregion

        #region Cache

        protected string GetCachedForge(string version) {
            return Path.Combine(Cache.FullName, string.Format(ForgeUniversalFormat, version));
        }

        protected bool isForgeCached(string version) {
            return File.Exists(GetCachedForge(version));
        }

        #endregion

    }

}
