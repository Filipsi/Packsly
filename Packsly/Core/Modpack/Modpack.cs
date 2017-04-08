using Newtonsoft.Json;
using Packsly.Core.Module;

namespace Packsly.Core.Modpack {

    [JsonObject(MemberSerialization.OptIn)]
    public class Modpack {

        public string Id { private set; get; }

        [JsonProperty("name")]
        public string Name { private set; get; }

        [JsonProperty("minecraft")]
        public string MinecraftVersion { private set; get; }

        [JsonProperty("modules")]
        public IModuleArguments[] Modules { private set; get; }

        [JsonProperty("mods")]
        public Mod[] Mods { private set; get; }

    }

}
