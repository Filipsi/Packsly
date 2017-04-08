using Newtonsoft.Json;

namespace Packsly.Core.Modpack {

    [JsonObject(MemberSerialization.OptIn)]
    public class Mod {

        public string Name { private set; get; }

        [JsonProperty("url")]
        public string Url { private set; get; }

        [JsonProperty("filename", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string FileName { private set; get; }

    }

}
