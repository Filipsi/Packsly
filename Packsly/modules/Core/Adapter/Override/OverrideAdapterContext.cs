using Newtonsoft.Json;
using Packsly.Core.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Adapter.Override {

    public class OverrideAdapterContext : IAdapterContext {

        [NonSerialized]
        public readonly string      OverrideFilesLocation = Settings.Instance.Temp.FullName;

        public readonly string[]    Overrides;

        public OverrideAdapterContext(params string[] overrides) {
            Overrides = overrides;
        }

        public OverrideAdapterContext(string source, params string[] overrides) : this(overrides) {
            OverrideFilesLocation = source;
        }

    }

}
