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

        #region IO

        public override void Load() {
            if (!File.Exists)
                return;

            using (StreamReader reader = File.OpenText())
                JsonConvert.PopulateObject(reader.ReadToEnd(), this, GetSerializerSettings());
        }

        public override void Save() {
            if (!File.Exists && File.DirectoryName != null) {
                Directory.CreateDirectory(File.DirectoryName);
            }

            using (StreamWriter writer = File.CreateText())
                writer.Write(JsonConvert.SerializeObject(this, Formatting.Indented, GetSerializerSettings()));
        }

        #endregion

    }

}
