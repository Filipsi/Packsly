using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Adapter.Override {

    public class OverrideAdapterContext : IAdapterContext {

        public readonly string      SourceFilesPath;
        public readonly string[]    Overrides;

        public OverrideAdapterContext(string source, params string[] overrides) {
            SourceFilesPath = source;
            Overrides = overrides;
        }

    }

}
