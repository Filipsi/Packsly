using System.IO;
using Newtonsoft.Json;
using Packsly3.Core.Common.Json;

namespace Packsly3.Core.FileSystem.Impl {

    public class PackslyConfig : JsonFile {

        public static readonly JsonSerializerSettings ConfigSerializerSettings = new JsonSerializerSettings {
            ContractResolver = new LowercaseContractResolver(),
            ObjectCreationHandling = ObjectCreationHandling.Replace,
            Converters = {
                new RelativePathConverter()
            }
        };

        #region Properties

        [JsonProperty("workspace")]
        public DirectoryInfo Workspace { set; get; } = new DirectoryInfo("./..");

        [JsonProperty("modpack")]
        public string DefaultModpackSource { set; get; }

        #endregion

        public PackslyConfig(FileSystemInfo path) : this(path.FullName) {
        }

        public PackslyConfig(string path) : base(Path.Combine(path, "packsly.json")) {
            SerializerSettings = ConfigSerializerSettings;
        }

        #region IO

        public sealed override void Save()
            => base.Save();

        public sealed override void Load()
            => base.Load();

        #endregion

    }

}
