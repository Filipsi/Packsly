using Packsly.Core.Common.Factory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Modpack.Provider {

    public class JsonFileModpackProvider : IModpackProvider {

        public bool CanUseSource(string source) {
            return File.Exists(source) && source.Contains(".json");
        }

        public ModpackInfo Create(string source) {
            string rawJson = string.Empty;
            using(StreamReader reader = File.OpenText(source))
                rawJson = reader.ReadToEnd();

            return PackslyFactory.Modpack.BuildFrom(rawJson);
        }

    }

}
