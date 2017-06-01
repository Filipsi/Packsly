using Newtonsoft.Json;
using Packsly.Core.Adapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Adapter.Forge {

    public class ForgeAdapterContext : IAdapterContext {

        [JsonProperty("version")]
        public string Version { private set; get; }

        [JsonProperty("url", DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Url { private set; get; } = "http://files.minecraftforge.net/maven/net/minecraftforge/forge";

        public ForgeAdapterContext(string version) {
            Version = version;
        }

    }

}
