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
        }

        internal object GetAdapterConfig(IAdapter adapter) {
            string key = adapter.GetType().FullName?.ToLower();

            if (key == null || !AdaptersConfig.ContainsKey(key)) {
                return null;
            }

            return AdaptersConfig[key];
        }


        internal void SetAdapterConfig(IAdapter adapter, object config)
            => SetAdapterConfig(adapter.GetType().FullName?.ToLower(), config);

        internal void SetAdapterConfig(string name, object config) {
            AdaptersConfig[name ?? throw new InvalidOperationException()] = config;
        }


    }

}
