using System.Collections.Generic;
using System.Linq;
using NLog;
using Packsly3.Core.Launcher.Instance;

namespace Packsly3.Core.Launcher.Modloader.Impl {

    public abstract class MultiModLoaderHandler<T> : BasicModLoaderHandler<T> where T : IMinecraftInstance {

        #region Fields

        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly List<IModLoaderInstallationStrategy<T>> instalationStrategies = new List<IModLoaderInstallationStrategy<T>>();

        #endregion

        #region IModLoaderHandler

        public override bool IsCompatible(string modLoader) {
            return instalationStrategies.Any(strategy => strategy.Name == modLoader);
        }

        public override void Install(T instance, string modLoader, string version) {
            instalationStrategies
                .First(strategy => strategy.Name == modLoader)
                .Install(instance, version);
        }

        public override void Uninstall(T instance, string modLoader) {
            instalationStrategies
                .First(strategy => strategy.Name == modLoader)
                .Uninstall(instance);
        }

        #endregion

        public void RegisterInstalationStrategy(IModLoaderInstallationStrategy<T> installationStrategy) {
            if (instalationStrategies.Contains(installationStrategy)) {
                return;
            }

            if (instalationStrategies.Any(strategy => strategy.Identificator == installationStrategy.Identificator)) {
                logger.Warn($"There already is a registered mod loader installation strategy with id '{installationStrategy.Identificator}'!");
                return;
            }

            instalationStrategies.Add(installationStrategy);
        }

    }

}
