using System.IO;
using Newtonsoft.Json;
using Packsly3.Core.Common;
using Packsly3.Core.Common.Json;

namespace Packsly3.Core.FileSystem {

    [JsonObject(MemberSerialization.OptIn)]
    public abstract class JsonFile : FileBase {

        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings {
            ContractResolver = new LowercaseContractResolver(),
            ObjectCreationHandling = ObjectCreationHandling.Replace,
            Converters = {
                new RelativePathConverter {
                    Root = Launcher.Launcher.Workspace.FullName
                }
            }
        };

        protected JsonFile(string path) : base(path) {
        }

        public override void Load() {
            if (!ThisFile.Exists)
                return;

            using (StreamReader reader = ThisFile.OpenText())
                JsonConvert.PopulateObject(reader.ReadToEnd(), this, SerializerSettings);
        }

        public override void Save() {
            if (!ThisFile.Exists && ThisFile.DirectoryName != null) {
                Directory.CreateDirectory(ThisFile.DirectoryName);
            }

            using (StreamWriter writer = ThisFile.CreateText())
                writer.Write(JsonConvert.SerializeObject(this, Formatting.Indented, SerializerSettings));
        }

    }

}
