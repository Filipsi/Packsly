using System.Collections.Generic;
using System.Linq;
using Packsly3.Core.Common.Register;
using Packsly3.Core.Launcher.Modloader;
using Packsly3.Core.Launcher.Modloader.Impl;
using Packsly3.MultiMC.FileSystem;

namespace Packsly3.MultiMC.Launcher.Modloader {

    [Register]
    public class MmcModLoaderHandler : MultiModLoaderHandler<MmcMinecraftInstance> {

        public MmcModLoaderHandler() {
            RegisterInstalationStrategy(new ForgeInstallationStrategy());
            RegisterInstalationStrategy(new LiteloaderInstallationStrategy());
            RegisterInstalationStrategy(new FabricInstallationStrategy());
        }

        #region IModLoaderHandler

        public override void DetectModLoaders(MmcMinecraftInstance instance, List<ModLoaderInfo> modLoaders) {
            if (!instance.PackFile.Exists) {
                return;
            }

            string[] ignored = {
                "org.lwjgl",
                "net.minecraft",
                "net.fabricmc.intermediary"
            };

            IEnumerable<MmcPackFile.Component> compatibleComponents =
                instance.PackFile.Components.Where(c => !ignored.Contains(c.Uid.ToLower()));

            modLoaders.AddRange(
                compatibleComponents.Select(component =>
                    new ModLoaderInfo(component.Name.ToLower(), component.Version)
                )
            );
        }

        #endregion

    }

}
