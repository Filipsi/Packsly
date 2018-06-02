using System;
using Packsly3.Core.Common;
using Packsly3.Core.Launcher.Instance;

namespace Packsly3.Core.Launcher.Adapter {

    public static class AdapterHandler {

        static AdapterHandler() {
            LifecycleDispatcher.LifecycleChanged += OnLifecycleChanged;
        }

        private static void OnLifecycleChanged(object sender, LifecycleEventArgs args) {
            // TODO
        }

    }

}
