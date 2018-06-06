using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Packsly3.Core.Launcher.Adapter;

namespace Packsly3.Core.FileSystem {

    public partial class PackslyInstanceFile : JsonFile {

        [JsonProperty("adapters")]
        internal AdaptersConfig Adapters { private set; get; } = new AdaptersConfig();

        public PackslyInstanceFile(string path) : base(Path.Combine(path, "instnace.packsly")) {
            Load();
        }

        public sealed override void Load() {
            base.Load();
        }

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
