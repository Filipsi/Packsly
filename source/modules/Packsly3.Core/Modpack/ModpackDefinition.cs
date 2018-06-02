using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Packsly3.Core.Modpack {

    public class ModpackDefinition {

        [JsonProperty("name")]
        public string Name { private set; get; }

        [JsonProperty("minecraft")]
        public string MinecraftVersion { private set; get; }

        [JsonProperty("icon")]
        public string Icon { private set; get; }

        [JsonProperty("adapters")]
        public Dictionary<string, object> Adapters { private set; get; } = new Dictionary<string, object>();

        [JsonProperty("modloader")]
        public Dictionary<string, string> ModLoaders { private set; get; } = new Dictionary<string, string>();

        [JsonProperty("environment")]
        public Dictionary<string, object> Environments { private set; get; } = new Dictionary<string, object>();

        [JsonProperty("mods")]
        public ModSource[] Mods { private set; get; } = new ModSource[0];

    }

}
