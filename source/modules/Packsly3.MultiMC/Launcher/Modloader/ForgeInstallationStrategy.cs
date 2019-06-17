using Packsly3.Core;
using Packsly3.MultiMC.FileSystem;

namespace Packsly3.MultiMC.Launcher.Modloader {

    internal class ForgeInstallationStrategy : MmcModLoaderInstallationStrategy {

        public override string Name
            => Packsly.Constants.ForgeModloader;

        public override string Identificator
            => "net.minecraftforge";

        public override string VersionConfigProperty
            => "ForgeVersion";

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
