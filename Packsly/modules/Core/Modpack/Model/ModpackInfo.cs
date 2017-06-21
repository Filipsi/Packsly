using Newtonsoft.Json;
using Packsly.Core.Common.Configuration;
using Packsly.Core.Launcher;
using Packsly.Core.Adapter;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Packsly.Core.Common.FileSystem;

namespace Packsly.Core.Modpack.Model {

    [JsonObject(MemberSerialization.OptIn)]
    public class ModpackInfo : JsonFile {

        #region Properties

        public string Id { internal set; get; }

        [JsonProperty("name")]
        public string Name { internal set; get; }

        [JsonProperty("icon")]
        public string Icon { internal set; get; }

        [JsonProperty("version", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Version { internal set; get; }

        [JsonProperty("minecraft")]
        public string MinecraftVersion { internal set; get; }

        [JsonProperty("mods")]
        public ModInfo[] Mods { internal set; get; }

        [JsonProperty("adapters", ItemTypeNameHandling = TypeNameHandling.All, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<IAdapterContext> Adapters { internal set; get; } = new List<IAdapterContext>();

        #endregion

        #region Constructors

        internal ModpackInfo() : base(string.Empty) {
        }

        internal ModpackInfo(string path) : base(path) {
        }

        #endregion

        #region IO

        public override void Save() {
            byte[] buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings() { PreserveReferencesHandling = PreserveReferencesHandling.Objects }));
            using(FileStream writer = File.Open(FileMode.Create))
                writer.Write(buffer, 0, buffer.Length);
        }

        public override void Load() {
            if(File.Exists) {
                using(StreamReader reader = File.OpenText())
                    JsonConvert.PopulateObject(reader.ReadToEnd(), this);
                Id = Name.ToLower();
            }

        }

        #endregion

    }

}
