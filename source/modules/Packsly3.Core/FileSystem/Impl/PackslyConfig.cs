using System.IO;
using Newtonsoft.Json;

namespace Packsly3.Core.FileSystem.Impl {

    public class PackslyConfig : JsonFile {

        #region Properties

        [JsonProperty("workspace", NullValueHandling = NullValueHandling.Ignore)]
        public string Workspace { set; get; }

        [JsonProperty("modpack")]
        public string DefaultModpackSource { set; get; }

        #endregion

        public PackslyConfig(FileSystemInfo path) : this(path.FullName) {
        }

        public PackslyConfig(string path) : base(Path.Combine(path, "packsly.json")) {
        }

        #region Logic

        public override void SetDefaultValues() {
            Workspace = "./..";
        }

        #endregion

        #region IO

        public sealed override void Save()
            => base.Save();

        public sealed override void Load()
            => base.Load();

        #endregion

    }

}
