using System.IO;
using Newtonsoft.Json;
using NLog;
using Packsly3.Core.Common.Json;

namespace Packsly3.Core.FileSystem {

    [JsonObject(MemberSerialization.OptIn)]
    public abstract class JsonFile : FileBase {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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


            using (StreamReader reader = File.OpenText()) {
                string content = reader.ReadToEnd();
                Logger.Debug($"Loaded JSON file '{File.Name}' with content {content}");
                JsonConvert.PopulateObject(content, this, GetSerializerSettings());
            }
        }

        public override void Save() {
            if (!File.Exists && File.DirectoryName != null) {
                Directory.CreateDirectory(File.DirectoryName);
            }

            using (StreamWriter writer = File.CreateText()) {
                string content = JsonConvert.SerializeObject(this, Formatting.Indented, GetSerializerSettings());
                Logger.Debug($"Saved JSON file '{File.Name}' with content {content}");
                writer.Write(content);
            }
        }

        #endregion

    }

}
