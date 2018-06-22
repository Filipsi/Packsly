using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Packsly3.Core.Modpack {

    [JsonObject(MemberSerialization.OptIn)]
    public class EnvironmentSpecific {

        [JsonProperty("on")]
        public string[] Entries { private set; get; } = new string[0];

        public bool IsEnvironmentSpecific
            => Entries.Length > 0;

        [JsonProperty("isBlacklist", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsBlacklist { private set; get; }

        public bool IsWhitelist
            => !IsBlacklist;

    }

}
