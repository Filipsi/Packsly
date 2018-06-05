using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Packsly3.Core.Common;
using Packsly3.Core.Launcher.Instance;
using Packsly3.Core.Launcher.Modloader;

namespace Packsly3.Core.Launcher.Adapter {

    public static class AdapterHandler {

        public static readonly IAdapter[] Adapters = RegisterAttribute.GetOccurrencesFor<IAdapter>();

        public static void OnLifecycleChanged(object sender, Lifecycle.Changed args) {
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
