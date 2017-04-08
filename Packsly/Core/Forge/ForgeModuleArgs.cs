using Newtonsoft.Json;
using Packsly.Core.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Forge {

    public class ForgeModuleArgs : IModuleArguments {

        [JsonProperty("version")]
        public string Version { private set; get; }

        [JsonProperty("url", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Url { private set; get; } = "http://files.minecraftforge.net/maven/net/minecraftforge/forge";

        public ForgeModuleArgs(string version) {
            Version = version;
        }

        public bool IsCompatible(IModule module) {
            return module.Type.Equals("forge");
        }

    }

}
