using Newtonsoft.Json;
using Packsly.Core.Tweak;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Forge {

    public class ForgeTweakArgs : ITweakArguments {

        [JsonProperty("version")]
        public string Version { private set; get; }

        [JsonProperty("url", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Url { private set; get; } = "http://files.minecraftforge.net/maven/net/minecraftforge/forge";

        public ForgeTweakArgs(string version) {
            Version = version;
        }

        public bool IsCompatible(ITweak tweak) {
            return tweak.Type.Equals("forge");
        }

    }

}
