using Packsly3.Cli.Verbs;
using Packsly3.Core;
using Packsly3.Core.Launcher.Instance;

namespace Packsly3.Cli.Logic {

    internal static class LifecycleHandler {

        public static void Handle(LifecycleOptions options) {
            IMinecraftInstance instance = Packsly.Launcher.GetInstance(options.InstanceName);
            Packsly.Lifecycle.EventBus.Publish(instance, options.Events);
        }

    }

}
