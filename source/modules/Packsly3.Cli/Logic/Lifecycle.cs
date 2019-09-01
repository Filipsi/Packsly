using NLog;
using Packsly3.Cli.Common;
using Packsly3.Cli.Verbs;
using Packsly3.Core;
using Packsly3.Core.Launcher.Instance;

namespace Packsly3.Cli.Logic {

    internal static class Lifecycle {

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static void Publish(LifecycleOptions options) {
            Logo.Print();
            logger.Debug($"Dispatching lifecycle events specified by command line arguments: {string.Join(", ", options.Events)}");
            IMinecraftInstance instance = Packsly.Launcher.GetInstance(options.InstanceId);
            instance.Load();
            Packsly.Lifecycle.EventBus.Publish(instance, options.Events);
            instance.Save();
        }

    }

}
