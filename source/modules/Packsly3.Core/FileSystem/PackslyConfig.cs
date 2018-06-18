using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Packsly3.Core.Common.Json;
using Packsly3.Core.Modpack;

namespace Packsly3.Core.FileSystem {

    public class PackslyConfig : JsonFile {

        public static readonly PackslyConfig Instnace = new PackslyConfig(
            Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)
        );

        [JsonProperty("workspace")]
        [JsonConverter(typeof(RelativePathConverter))]
        public DirectoryInfo Workspace { private set; get; }

        [JsonProperty("modpack")]
        public string DefaultModpackSource { private set; get; }

        private PackslyConfig(string path) : base(Path.Combine(path, "packsly.json")) {
            if (!Exists) {
                Save();
            }

            Load();
        }

        public sealed override void Save()
            => base.Save();

        public sealed override void Load()
            => base.Load();

    }

}
