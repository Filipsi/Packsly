using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using Packsly3.Core.Common.Json;

namespace Packsly3.Core.FileSystem {

    [JsonObject(MemberSerialization.OptIn)]
    public abstract class JsonFile : FileBase {

        #region Properties

        public JsonSerializerSettings SerializerSettings { get; set; }

        #endregion

        #region Fields

        protected static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private static readonly JsonSerializerSettings defaultSerializerSettings = new JsonSerializerSettings {
            ContractResolver = new LowercaseContractResolver(),
            ObjectCreationHandling = ObjectCreationHandling.Replace
        };

        #endregion

        protected JsonFile(string path) : base(path) {
            SetDefaultValues();
        }

        #region Logic

        public abstract void SetDefaultValues();

        protected virtual JsonSerializerSettings GetSerializerSettings() {
            return SerializerSettings ?? defaultSerializerSettings;
        }

        #endregion

        #region IO

        public override void Load() {
            if (!file.Exists) {
                return;
            }

            using (StreamReader reader = file.OpenText()) {
                string content = reader.ReadToEnd();
                logger.Debug($"Loaded JSON file '{file.Name}' with content {JToken.Parse(content)}");
                JsonConvert.PopulateObject(content, this, GetSerializerSettings());
            }
        }

        public override void Save() {
            if (!file.Exists && file.DirectoryName != null) {
                Directory.CreateDirectory(file.DirectoryName);
            }

            using (StreamWriter writer = file.CreateText()) {
                string content = JsonConvert.SerializeObject(this, Formatting.Indented, GetSerializerSettings());
                logger.Debug($"Saved JSON file '{file.Name}' with content {content}");
                writer.Write(content);
            }
        }

        #endregion

    }

}
