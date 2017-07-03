﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Packsly.Core.Adapter.Forge {

    [JsonObject(MemberSerialization.OptIn)]
    public class ForgeLibrary {

        [JsonProperty("name")]
        public string Name { private set; get; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { private set; get; }

        [JsonProperty("MMC-hint", NullValueHandling = NullValueHandling.Ignore)]
        public string MmcHint { private set; get; }

        private ForgeLibrary() {
        }

        public ForgeLibrary(string name) {
            Name = name;
        }

        public ForgeLibrary(string name, string url) {
            Name = name;
            Url = url;
        }

        public ForgeLibrary(string name, string url, string mmchint) {
            Name = name;
            Url = url;
            MmcHint = mmchint;
        }

        public static ForgeLibrary FromJson(JObject root) {
            ForgeLibrary lib = new ForgeLibrary();
            JToken clientreq = root.GetValue("clientreq");
            JToken serverreq = root.GetValue("serverreq");

            if(root.GetValue("checksums") != null && clientreq != null && clientreq.Value<bool>() && serverreq != null && serverreq.Value<bool>())
                lib.MmcHint = "forge-pack-xz";

            string name = root.Value<string>("name");
            lib.Name = string.Format("{0}{1}", name, name.Contains("net.minecraftforge:forge") ? ":universal" : string.Empty);

            JToken url = root.GetValue("url");
            if(url != null)
                lib.Url = url.Value<string>();

            return lib;
        }

    }

}
