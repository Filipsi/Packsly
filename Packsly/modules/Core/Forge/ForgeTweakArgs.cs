using Newtonsoft.Json;
using Packsly.Core.Tweaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Forge {

    public class ForgeTweakArgs : IExecutionContext {

        [JsonProperty("version")]
        public string Version { private set; get; }

        [JsonProperty("url", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Url { private set; get; } = "http://files.minecraftforge.net/maven/net/minecraftforge/forge";

        public ForgeTweakArgs(string version) {
            Version = version;
        }

    }

}
