using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Packsly3.Core.Launcher.Instance;

namespace Packsly3.Core.Modpack.Model {

    [JsonObject(MemberSerialization.OptIn)]
    public class RemoteResource {

        #region Properties

        [JsonProperty("url")]
        public Uri Url { protected set; get; }

        [JsonProperty("path")]
        public string EnvironmentalPath { protected set; get; }

        [JsonProperty("filename")]
        public string FileName { protected set; get; }

        [JsonProperty("environment", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public EnvironmentSpecific EnvironmentOnly { private set; get; } = new EnvironmentSpecific();

        #endregion

        public bool CanDownload {
            get {
                if (!EnvironmentOnly.IsEnvironmentSpecific) {
                    return true;
                }

                bool isCompatibleEnvironment = EnvironmentOnly.Entries.Any(
                    entry => entry == Packsly.Launcher.Name
                );

                if (EnvironmentOnly.IsWhitelist && !isCompatibleEnvironment) {
                    return false;
                }

                return !EnvironmentOnly.IsBlacklist || !isCompatibleEnvironment;
            }
        }

        #region Logic

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context) {
            HandleOnDeserialized(context);
        }

        protected virtual void HandleOnDeserialized(StreamingContext context) {
            if (Url == null) {
                return;
            }

            if (string.IsNullOrEmpty(EnvironmentalPath)) {
                EnvironmentalPath = Path.Combine("{" + EnvironmentVariables.RootFolder + "}", Path.GetDirectoryName(Url.AbsolutePath));
            }

            if (string.IsNullOrEmpty(FileName)) {
                FileName = Path.GetFileName(Url.AbsolutePath);
            }
        }

        #endregion

        #region Helpers

        public string GetFilePath(EnvironmentVariables environment) {
            return Path.GetFullPath(Path.Combine(environment.FromEnviromentalPath(EnvironmentalPath), FileName));
        }

        #endregion

    }

}
