using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Packsly3.Core.Launcher;
using Packsly3.Core.Launcher.Instance;

namespace Packsly3.Core.Modpack {

    [JsonObject(MemberSerialization.OptIn)]
    public class RemoteResource {

        [JsonProperty("url")]
        public Uri Url { protected set; get; }

        [JsonProperty("path")]
        public string FilePath { protected set; get; }

        [JsonProperty("filename")]
        public string FileName { protected set; get; }

        [JsonProperty("environment", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public EnvironmentSpecific EnvironmentOnly { private set; get; } = new EnvironmentSpecific();

        public bool ShouldDownload {
            get {
                if (!EnvironmentOnly.IsEnvironmentSpecific)
                    return true;

                bool containsEntry = EnvironmentOnly.Entries.Any(entry => entry == MinecraftLauncher.CurrentEnvironment.Name);
                if (EnvironmentOnly.IsWhitelist && !containsEntry)
                    return false;

                return !EnvironmentOnly.IsBlacklist || !containsEntry;
            }
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context) {
            HandleOnDeserialized(context);
        }

        protected virtual void HandleOnDeserialized(StreamingContext context) {
            if (Url == null)
                return;

            if (string.IsNullOrEmpty(FilePath)) {
                FilePath = "{" + EnvironmentVariables.MinecraftFolder + "}\\" + Path.GetDirectoryName(Url.AbsolutePath);
            }

            if (string.IsNullOrEmpty(FileName)) {
                FileName = Path.GetFileName(Url.AbsolutePath);
            }
        }

    }

}
