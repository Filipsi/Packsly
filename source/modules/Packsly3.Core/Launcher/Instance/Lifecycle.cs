using System;
using System.Collections.Generic;
using NLog;
using Packsly3.Core.Launcher.Adapter;

namespace Packsly3.Core.Launcher.Instance {

    public class Lifecycle {

        #region Properties

        public Dispatcher EventBus { get; } = new Dispatcher();

        #endregion

        #region Fields

        public static readonly string PreInstallation = "preinstallation";

        public static readonly string PostInstallation = "postinstallation";

        public static readonly string PreLaunch = "prelaunch";

        public static readonly string PostExit = "postexit";

        public static readonly string UpdateStarted = "updatestarted";

        public static readonly string UpdateFinished = "updatefinished";

        public static readonly string PackslyExit = "packslyexit";

        #endregion

        internal Lifecycle() {
        }

        #region Events

        public class ChangedEventArgs : EventArgs {

            public readonly IMinecraftInstance Instance;
            public readonly string EventName;

            protected internal ChangedEventArgs(IMinecraftInstance instance, string eventName) {
                Instance = instance;
                EventName = eventName;
            }

        }

        public class Dispatcher {

            private static readonly Logger logger = LogManager.GetCurrentClassLogger();

            public event EventHandler<ChangedEventArgs> LifecycleEvent;

            internal Dispatcher() {
                LifecycleEvent += AdapterHandler.HandleLifecycleChange;
            }

            public void Publish(IMinecraftInstance instance, string eventName) {
                logger.Debug($"Publishing lifecycle event '{eventName}' {(instance == null ? "..." : $" for minecraft instance '{instance.Id}'...")}");
                LifecycleEvent?.Invoke(this, new ChangedEventArgs(instance, eventName));
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

