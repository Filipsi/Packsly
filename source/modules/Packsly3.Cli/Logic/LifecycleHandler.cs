using System.Linq;
using NLog;
using Packsly3.Cli.Verbs;
using Packsly3.Core;
using Packsly3.Core.Launcher.Instance;

namespace Packsly3.Cli.Logic {

    internal static class LifecycleHandler {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Handle(LifecycleOptions options) {
            Logger.Debug($"Dispatching lifecycle events specified by command line arguments: {string.Join(", ", options.Events)}");
            IMinecraftInstance instance = Packsly.Launcher.GetInstance(options.InstanceName);
            Packsly.Lifecycle.EventBus.Publish(instance, options.Events);
        }

    }

}
