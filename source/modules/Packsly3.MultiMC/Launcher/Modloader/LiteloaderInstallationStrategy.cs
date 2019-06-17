using Packsly3.Core;
using Packsly3.MultiMC.FileSystem;

namespace Packsly3.MultiMC.Launcher.Modloader {

    internal class LiteloaderInstallationStrategy : MmcModLoaderInstallationStrategy {

        public override string Name
            => Packsly.Constants.LiteloaderModloader;

        public override string Identificator
            => "com.mumfrey.liteloader";

        public override string VersionConfigProperty
            => "LiteloaderVersion";

        protected override void InstallModloader(MmcMinecraftInstance instance, string version) {
            instance.PackFile.Components.Add(
                new MmcPackFile.Component {
                    Name = Name,
                    Uid = Identificator,
                    Version = version,
                    CachedVersion = version,
                    Requirements = new MmcPackFile.ComponentRequirement[] {
                        new MmcPackFile.ComponentSpecificRequirement {
                            Uid = "net.minecraft",
                            EquivalentTo = instance.MinecraftVersion
                        }
                    }
                }
            );
        }

    }

}
