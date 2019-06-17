using System.Linq;
using NLog;
using Packsly3.Core.Launcher.Modloader;
using Packsly3.MultiMC.FileSystem;

namespace Packsly3.MultiMC.Launcher.Modloader {

    internal abstract class MmcModLoaderInstallationStrategy : IModLoaderInstallationStrategy<MmcMinecraftInstance> {

        #region Fields

        protected readonly Logger logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region IModLoaderInstallationStrategy

        public abstract string Name { get; }

        public abstract string Identificator { get; }

        public virtual string VersionConfigProperty { get; } = null;

        public void Install(MmcMinecraftInstance instance, string version) {
            instance.PackFile.Load();

            MmcPackFile.Component modloaderComponent = instance.PackFile.Components.FirstOrDefault(
                c => c.Uid == Identificator
            );

            if (modloaderComponent == null) {
                InstallModloader(instance, version);

            } else {
                // Update modloader version
                // modloaderComponent.CachedVersion = version;
                modloaderComponent.Version = version;
            }

            // Create or update version entry in instance.cfg file
            // ReSharper disable once InvertIf
            if (!string.IsNullOrEmpty(VersionConfigProperty)) {
                MmcConfigFile config = instance.MmcConfig;
                config.GetType().GetProperty(VersionConfigProperty)?.SetValue(config, version);
            }
        }

        public void Uninstall(MmcMinecraftInstance instance) {
            // Remove config component from mmc-pack.jsonfile
            if (instance.PackFile.Exists) {
                UninstallModloader(instance);
            }

            // Remove entry from Packsly config file
            MmcConfigFile config = instance.MmcConfig;
            config.GetType().GetProperty(VersionConfigProperty)?.SetValue(config, string.Empty);
        }

        #endregion

        #region MmcModLoaderInstallationStrategy

        protected abstract void InstallModloader(MmcMinecraftInstance instance, string version);

        protected virtual void UninstallModloader(MmcMinecraftInstance instance) {
            MmcPackFile.Component modloaderComponent = instance.PackFile.Components.FirstOrDefault(
                c => c.Uid == Identificator
            );

            if (modloaderComponent == null) {
                logger.Error($"Failed to uninstall modloader with id '${Identificator}'. There is no such component in mmc-pack file!");
            } else {
                instance.PackFile.Components.Remove(modloaderComponent);
            }
        }

        #endregion

    }
}
