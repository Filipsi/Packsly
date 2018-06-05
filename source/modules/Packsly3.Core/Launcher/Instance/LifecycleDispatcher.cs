using System;
using System.Reflection;
using Packsly3.Core.Launcher.Adapter;
using Packsly3.Core.Modpack;

namespace Packsly3.Core.Launcher.Instance {

    public static class Lifecycle {

        public static readonly string PreInstallation = "preinstallation";

        public static readonly string PostInstallation = "postinstallation";

        public static readonly string PreLaunch = "prelaunch";

        public static readonly string PostExit = "postexit";

        public class Changed : EventArgs {

            public readonly IMinecraftInstance Instance;
            public readonly string EventName;

            protected internal Changed(IMinecraftInstance instance, string eventName) {
                Instance = instance;
                EventName = eventName;
            }

        }

        public static class LifecycleDispatcher {

            public static EventHandler<Changed> LifecycleEvent;

            static LifecycleDispatcher() {
                LifecycleEvent += AdapterHandler.OnLifecycleChanged;
            }

            public static void Dispatch(IMinecraftInstance instance, string eventName)
                => LifecycleEvent?.Invoke(null, new Changed(instance, eventName));
        }
    }

}

