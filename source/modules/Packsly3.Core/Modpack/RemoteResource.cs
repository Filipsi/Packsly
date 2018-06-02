using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Packsly3.Core.Launcher;
using Packsly3.Core.Launcher.Instance;

namespace Packsly3.Core.Modpack {

    public class RemoteResource {

        [JsonProperty("url")]
        public Uri Url { protected set; get; }

        [JsonProperty("path")]
        public string FilePath { protected set; get; }

        [JsonProperty("filename")]
        public string FileName { protected set; get; }

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
