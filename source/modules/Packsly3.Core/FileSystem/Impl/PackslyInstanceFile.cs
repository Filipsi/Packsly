using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Packsly3.Core.Common.Json;
using Packsly3.Core.Launcher.Instance.Logic;

namespace Packsly3.Core.FileSystem.Impl {

    public partial class PackslyInstanceFile : JsonFile {

        #region Properties

        [JsonProperty("adapters")]
        internal AdaptersConfig Adapters { private set; get; } = new AdaptersConfig();

        [JsonProperty("files")]
        internal Dictionary<FileManager.GroupType, List<FileInfo>> ManagedFiles { private set; get; } = new Dictionary<FileManager.GroupType, List<FileInfo>>();

        #endregion

        public PackslyInstanceFile(string path) : base(Path.Combine(path, "instnace.packsly")) {
            SerializerSettings = new JsonSerializerSettings {
                ContractResolver = new LowercaseContractResolver(),
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                Converters = {
                    new RelativePathConverter {
                        Root = DirectoryPath
                    }
                }
            };

            Load();
        }

        #region IO

        public sealed override void Load()
            => base.Load();

        #endregion

    }

}
