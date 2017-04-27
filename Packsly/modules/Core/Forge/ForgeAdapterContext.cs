using Newtonsoft.Json;
using Packsly.Core.Tweaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Forge {

    public class ForgeAdapterContext : IExecutionContext {

        [JsonProperty("version")]
        public string Version { private set; get; }

        [JsonProperty("url", DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Url { private set; get; } = "http://files.minecraftforge.net/maven/net/minecraftforge/forge";

        public ForgeAdapterContext(string version) {
            Version = version;
        }

    }

}
