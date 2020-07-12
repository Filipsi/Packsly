using System.Collections.Generic;
using Packsly3.Core.Launcher.Adapter;

namespace Packsly3.Core.FileSystem.Impl {

    internal class AdaptersConfig : Dictionary<string, object> {

        internal object GetConfigFor(IAdapter adapter) {
            if (adapter.Id == null || !ContainsKey(adapter.Id)) {
                return null;
            }

            return this[adapter.Id];
        }

        internal void SetConfigFor(IAdapter adapter, object config) {
            SetConfigFor(adapter.Id, config);
        }

        internal void SetConfigFor(string name, object config) {
            this[name] = config;
        }

    }

}
