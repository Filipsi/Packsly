using Newtonsoft.Json;

namespace Packsly.Core.Adapter.Forge {

    public class ForgeAdapterContext : IAdapterContext {

        [JsonProperty("version")]
        public readonly string Version;

        [JsonProperty("url", DefaultValueHandling = DefaultValueHandling.Populate)]
        public readonly string Url = "http://files.minecraftforge.net/maven/net/minecraftforge/forge";

        public ForgeAdapterContext(string version) {
            Version = version;
        }

    }

}
