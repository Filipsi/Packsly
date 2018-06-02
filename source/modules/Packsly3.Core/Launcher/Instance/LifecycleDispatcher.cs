using System;
using Packsly3.Core.Modpack;

namespace Packsly3.Core.Launcher.Instance {

    public static class LifecycleDispatcher {

        public static EventHandler<LifecycleEventArgs> LifecycleChanged;

        public static void Dispatch(IMinecraftInstance instance, Lifecycle type)
            => LifecycleChanged?.Invoke(null, new LifecycleEventArgs(instance, type));

    }

    public class LifecycleEventArgs : EventArgs {

        public readonly IMinecraftInstance Instance;
        public readonly Lifecycle Type;

        public LifecycleEventArgs(IMinecraftInstance instance, Lifecycle type) {
            Instance = instance;
            Type = type;
        }

    }

}
