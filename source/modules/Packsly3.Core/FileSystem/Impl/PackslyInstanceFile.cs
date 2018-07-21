using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Packsly3.Core.Common.Json;
using Packsly3.Core.Launcher.Adapter;
using Packsly3.Core.Launcher.Instance.Logic;

namespace Packsly3.Core.FileSystem.Impl {

    public partial class PackslyInstanceFile : JsonFile {

        private readonly JsonSerializerSettings _serializerSettings;

        [JsonProperty("adapters")]
        internal AdaptersConfig Adapters { private set; get; } = new AdaptersConfig();

        [JsonProperty("files")]
        internal Dictionary<FileManager.GroupType, List<FileInfo>> ManagedFiles { private set; get; } = new Dictionary<FileManager.GroupType, List<FileInfo>>();

        public PackslyInstanceFile(string path) : base(Path.Combine(path, "instnace.packsly")) {
            _serializerSettings = new JsonSerializerSettings {
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

        protected override JsonSerializerSettings GetSerializerSettings()
            => _serializerSettings;

        public sealed override void Load()
            => base.Load();

    }

    public partial class PackslyInstanceFile {

        [JsonObject(MemberSerialization.OptIn)]
        internal class AdaptersConfig : IEnumerable<string> {

            [JsonProperty("entries")]
            protected Dictionary<string, object> Entries { set; get; } = new Dictionary<string, object>();

            internal object GetConfigFor(IAdapter adapter) {
                if (adapter.Id == null || !Entries.ContainsKey(adapter.Id)) {
                    return null;
                }

                return Entries[adapter.Id];
            }

            internal void SetConfigFor(IAdapter adapter, object config)
                => SetConfigFor(adapter.Id, config);

            internal void SetConfigFor(string name, object config) {
                Entries[name ?? throw new InvalidOperationException()] = config;
            }

            #region IEnumerable

            public IEnumerator<string> GetEnumerator()
                => Entries.Keys.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();

            #endregion

        }

    }

}
