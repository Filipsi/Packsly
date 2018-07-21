using System.IO;
using Newtonsoft.Json;
using Packsly3.Core.Common.Json;

namespace Packsly3.Core.FileSystem {

    [JsonObject(MemberSerialization.OptIn)]
    public abstract class JsonFile : FileBase {

        private static readonly JsonSerializerSettings DefaultSerializerSettings = new JsonSerializerSettings {
            ContractResolver = new LowercaseContractResolver(),
            ObjectCreationHandling = ObjectCreationHandling.Replace
        };

        protected JsonFile(string path) : base(path) {
        }

        protected virtual JsonSerializerSettings GetSerializerSettings() {
            return DefaultSerializerSettings;
        }

        public override void Load() {
            if (!ThisFile.Exists)
                return;

            using (StreamReader reader = ThisFile.OpenText())
                JsonConvert.PopulateObject(reader.ReadToEnd(), this, GetSerializerSettings());
        }

        public override void Save() {
            if (!ThisFile.Exists && ThisFile.DirectoryName != null) {
                Directory.CreateDirectory(ThisFile.DirectoryName);
            }

            using (StreamWriter writer = ThisFile.CreateText())
                writer.Write(JsonConvert.SerializeObject(this, Formatting.Indented, GetSerializerSettings()));
        }

    }

}
