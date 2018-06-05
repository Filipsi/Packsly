using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Packsly3.Core.Launcher.Adapter;

namespace Packsly3.Core.FileSystem {

    public class PackslyInstanceFile : JsonFile {

        [JsonProperty("adapters")]
        internal Dictionary<string, object> AdaptersConfig { private set; get; } = new Dictionary<string, object>();

        public PackslyInstanceFile(string path) : base(Path.Combine(path, "instnace.packsly")) {
            Load();
        }

        public sealed override void Load() {
            base.Load();
        }

        internal object GetAdapterConfig(IAdapter adapter) {
            if (adapter.Id == null || !AdaptersConfig.ContainsKey(adapter.Id)) {
                return null;
            }

            return AdaptersConfig[adapter.Id];
        }


        internal void SetAdapterConfig(IAdapter adapter, object config)
            => SetAdapterConfig(adapter.Id, config);

        internal void SetAdapterConfig(string name, object config) {
            AdaptersConfig[name ?? throw new InvalidOperationException()] = config;
        }

    }

}
