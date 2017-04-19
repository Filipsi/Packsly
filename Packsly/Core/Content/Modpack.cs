using Newtonsoft.Json;
using Packsly.Core.Configuration;
using Packsly.Core.Module;
using System;
using System.Collections.Generic;
using System.IO;

namespace Packsly.Core.Content {

    [JsonObject(MemberSerialization.OptIn)]
    public class Modpack {

        public string Id { private set; get; }

        [JsonProperty("name")]
        public string Name { private set; get; }

        [JsonProperty("icon")]
        public string Icon { private set; get; }

        [JsonProperty("version")]
        public string Version { private set; get; }

        [JsonProperty("minecraft")]
        public string MinecraftVersion { private set; get; }

        [JsonProperty("mods")]
        public Mod[] Mods { private set; get; }

        [JsonProperty("modules", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<IModuleArguments> Modules { private set; get; }

        [JsonProperty("overrideSource", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string OverrideSource { private set; get; }

        [JsonProperty("overrides", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] OverrideFiles { private set; get; }

        public Modpack(string id, string name, string icon, string mcversion, params Mod[] mods) {
            Id = id;
            Name = name;
            Icon = icon;
            MinecraftVersion = mcversion;
            Modules = new List<IModuleArguments>();
            Mods = mods;
        }

        public Modpack AddOverrides(string source, params string[] files) {
            OverrideSource = source;
            OverrideFiles = files;
            return this;
        }

        public Modpack AddModules(params IModuleArguments[] modules) {
            Modules.AddRange(modules);
            return this;
        }

    }

}
