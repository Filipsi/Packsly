using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Packsly3.Core.Common;
using Packsly3.Core.Launcher.Instance;

namespace Packsly3.Core.Launcher.Adapter {

    internal static class AdapterHandler {

        internal static readonly IAdapter[] Adapters = RegisterAttribute.GetOccurrencesFor<IAdapter>();

        internal static void OnLifecycleChanged(object sender, Lifecycle.Changed args) {
            IEnumerable<IAdapter> usedAdapters = Adapters
                .Where(adapter => args.Instance.PackslyConfig.Adapters.Contains(adapter.Id))
                .Where(adapter => adapter.IsCompatible(args.Instance))
                .Where(adapter => adapter.IsCompatible(args.EventName));

            foreach (IAdapter adapter in usedAdapters) {
                adapter.Execute(JObject.FromObject(args.Instance.PackslyConfig.Adapters.GetConfigFor(adapter)), args.EventName, args.Instance);
            }
        }

    }

}
