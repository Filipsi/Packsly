using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Modpack.Provider {

    public class JsonModpackProvider : IModpackProvider {

        public bool CanUseSource(string source) {
            return source.StartsWith("{") && source.EndsWith("}");
        }

        public ModpackInfo Create(string source) {
            return JsonConvert.DeserializeObject<ModpackInfo>(source);
        }

    }

}
