using System.Linq;
using Packsly3.Core;
using Packsly3.MultiMC.FileSystem;

namespace Packsly3.MultiMC.Launcher.Modloader {

    internal class FabricInstallationStrategy : MmcModLoaderInstallationStrategy {

        public override string Name
            => Packsly.Constants.FabricModloader;

        public override string Identificator
            => "net.fabricmc.fabric-loader";

        protected override void InstallModloader(MmcMinecraftInstance instance, string version) {
            // Modify the instance.cfg file
            // Fabric doesn't have a version property in instance.cfg file like Forge and Liteloader have
            instance.MmcConfig.McLaunchMethod = "LauncherPart";
            instance.MmcConfig.LogPrePostOutput = true;

            // Add mappings for Fabric
            instance.PackFile.Components.Add(
                new MmcPackFile.Component {
                    Name = "Intermediary Mappings",
                    Uid = "net.fabricmc.intermediary",
                    Version = instance.MinecraftVersion,
                    CachedVersion = instance.MinecraftVersion,
                    CachedVolatile = true,
                    DependencyOnly = true,
                    Requirements = new MmcPackFile.ComponentRequirement[] {
                        new MmcPackFile.ComponentSpecificRequirement {
                            Uid = "net.minecraft",
                            EquivalentTo = instance.MinecraftVersion
                        }
                    }
                }
            );

            // Add fabric loader
            instance.PackFile.Components.Add(
                new MmcPackFile.Component {
                    Name = "Fabric Loader",
                    Uid = Identificator,
                    Version = version,
                    CachedVersion = version,
                    Requirements = new MmcPackFile.ComponentRequirement[] {
                        new MmcPackFile.ComponentSpecificRequirement {
                            Uid = "net.fabricmc.intermediary",
                        }
                    }
                }
            );
        }

        protected override void UninstallModloader(MmcMinecraftInstance instance) {
            // Remove changes from the instance.cfg file
            instance.MmcConfig.McLaunchMethod = string.Empty;
            instance.MmcConfig.LogPrePostOutput = false;

            // Remove mappings
            MmcPackFile.Component modloaderComponent = instance.PackFile.Components.FirstOrDefault(
                c => c.Uid == "net.fabricmc.intermediary"
            );

            if (modloaderComponent != null) {
                instance.PackFile.Components.Remove(modloaderComponent);
            }

            // Remove fabric loader component
            base.UninstallModloader(instance);
        }
    }

}
