using System;
using System.Collections.Generic;
using NLog;
using Packsly3.Core.Launcher.Adapter;

namespace Packsly3.Core.Launcher.Instance.Logic {

    public class Lifecycle {

        #region Constants

        public static readonly string PreInstallation = "preinstallation";

        public static readonly string PostInstallation = "postinstallation";

        public static readonly string PreLaunch = "prelaunch";

        public static readonly string PostExit = "postexit";

        public static readonly string UpdateStarted = "updatestarted";

        public static readonly string UpdateFinished = "updatefinished";

        #endregion

        public readonly Dispatcher EventBus = new Dispatcher();

        internal Lifecycle() {
        }

        #region Events

        public class Changed : EventArgs {

            public readonly IMinecraftInstance Instance;
            public readonly string EventName;

            protected internal Changed(IMinecraftInstance instance, string eventName) {
                Instance = instance;
                EventName = eventName;
            }

        }

        public class Dispatcher {

            private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

            public readonly EventHandler<Changed> LifecycleEvent;

            internal Dispatcher() {
                LifecycleEvent += AdapterHandler.OnLifecycleChanged;
            }

            public void Publish(IMinecraftInstance instance, string eventName) {
                Logger.Debug($"Publishing lifecycle event '{eventName}' for minecraft instance '{instance.Id}'...");
                LifecycleEvent?.Invoke(null, new Changed(instance, eventName));
            }

            public void Publish(IMinecraftInstance instance, IEnumerable<string> eventNames) {
                foreach (string eventName in eventNames) {
                    Publish(instance, eventName);
                }
            }
        }

        #endregion

    }

}

