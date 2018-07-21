using System.IO;
using Newtonsoft.Json;
using Packsly3.Core.Common.Json;

namespace Packsly3.Core.FileSystem.Impl {

    public class PackslyConfig : JsonFile {

        #region Properties

        [JsonProperty("workspace")]
        [JsonConverter(typeof(RelativePathConverter))]
        public DirectoryInfo Workspace { private set; get; } = Packsly.AplicationDirectory;

        [JsonProperty("modpack")]
        public string DefaultModpackSource { private set; get; }

        #endregion

        internal PackslyConfig(FileSystemInfo path) : this(path.FullName) {
        }

        internal PackslyConfig(string path) : base(Path.Combine(path, "packsly.json")) {
            if (Exists) {
                Load();
            }
            else {
                Save();
            }
        }

        #region IO

        public sealed override void Save()
            => base.Save();

        public sealed override void Load()
            => base.Load();

        #endregion

    }

}
