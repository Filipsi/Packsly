using Newtonsoft.Json;
using Packsly.Core.Common.Configuration;

namespace Packsly.Core.Adapter.Override {

    public class OverrideAdapterContext : IAdapterContext {

        public readonly string      OverrideFilesLocation = Settings.Instance.Temp.FullName;

        [JsonProperty(PropertyName = "overrides")]
        public readonly string[]    Overrides;

        public OverrideAdapterContext(params string[] overrides) {
            Overrides = overrides;
        }

        public OverrideAdapterContext(string source, params string[] overrides) : this(overrides) {
            OverrideFilesLocation = source;
        }

    }

}
