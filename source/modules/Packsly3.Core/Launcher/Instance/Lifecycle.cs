using System;
using Packsly3.Core.Launcher.Adapter;

namespace Packsly3.Core.Launcher.Instance {

    public static class Lifecycle {

        public static readonly string PreInstallation = "preinstallation";

        public static readonly string PostInstallation = "postinstallation";

        public static readonly string PreLaunch = "prelaunch";

        public static readonly string PostExit = "postexit";

        public static readonly string UpdateStarted = "updatestarted";

        public static readonly string UpdateFinished = "updatefinished";

        public class Changed : EventArgs {

            public readonly IMinecraftInstance Instance;
            public readonly string EventName;

            protected internal Changed(IMinecraftInstance instance, string eventName) {
                Instance = instance;
                EventName = eventName;
            }

        }

        public static class Dispatcher {

            public static readonly EventHandler<Changed> LifecycleEvent;

            static Dispatcher() {
                LifecycleEvent += AdapterHandler.OnLifecycleChanged;
            }

            public static void Publish(IMinecraftInstance instance, string eventName)
                => LifecycleEvent?.Invoke(null, new Changed(instance, eventName));
        }
    }

}

