using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Packsly3.Core.FileSystem;

namespace Packsly3.MultiMC.FileSystem {

    internal class MmcPackFile : JsonFile {

        [JsonProperty("formatVersion")]
        public int FormatVersion { private set; get; } = 1;

        [JsonProperty("components")]
        public List<Component> Components { private set; get; } = new List<Component>();

        public MmcPackFile(string path) : base(Path.Combine(path, "mmc-pack.json")) {
        }

        internal class Component {

            [JsonProperty("cachedName")]
            public string Name { set; get; }

            [JsonProperty("cachedVersion")]
            public string CachedVersion { set; get; }

            [JsonProperty("cachedRequires", DefaultValueHandling = DefaultValueHandling.Ignore)]
            public object[] Requirements { set; get; }

            [JsonProperty("important", DefaultValueHandling = DefaultValueHandling.Ignore)]
            public bool Important { set; get; }

            [JsonProperty("cachedVolatile", DefaultValueHandling = DefaultValueHandling.Ignore)]
            public bool CachedVolatile { set; get; }

            [JsonProperty("dependencyOnly", DefaultValueHandling = DefaultValueHandling.Ignore)]
            public bool DependencyOnly { set; get; }

            [JsonProperty("uid")]
            public string Uid { set; get; }

            [JsonProperty("version")]
            public string Version { set; get; }

        }

        internal class ComponentRequirement {

            [JsonProperty("uid")]
            public string Uid { set; get; }

        }

        internal class ComponentSpecificRequirement : ComponentRequirement {

            [JsonProperty("equals")]
            public string EquivalentTo { set; get; }

        }

    }



}
