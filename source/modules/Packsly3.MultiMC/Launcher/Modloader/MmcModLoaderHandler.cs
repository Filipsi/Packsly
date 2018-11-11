using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NLog;
using Packsly3.Core;
using Packsly3.Core.Common.Register;
using Packsly3.Core.Launcher.Modloader;
using Packsly3.MultiMC.FileSystem;

namespace Packsly3.MultiMC.Launcher.Modloader {

    [Register]
    public class MmcModLoaderHandler : InstanceModLoaderHandler<MmcMinecraftInstance> {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static readonly Dictionary<string, KeyValuePair<string, string>> ModLoadersMap = new Dictionary<string, KeyValuePair<string, string>> {
            {
                Packsly.Constants.ForgeModlaoder, new KeyValuePair<string, string>(
                    "net.minecraftforge",
                    "ForgeVersion"
                )
            },
            {
                Packsly.Constants.LiteloaderModlaoder, new KeyValuePair<string, string>(
                    "com.mumfrey.liteloader",
                    "LiteloaderVersion"
                )
            }
        };

        public override bool IsCompatible(string modLoader)
            => ModLoadersMap.Keys.Contains(modLoader);

        public override void DetectModLoaders(MmcMinecraftInstance instance, List<ModLoaderInfo> modLoaders) {
            if (!instance.PackFile.Exists) {
                return;
            }

            string[] ignored = {
                "org.lwjgl",
                "net.minecraft"
            };

            IEnumerable<MmcPackFile.Component> compatibleComponents =
                instance.PackFile.Components.Where(c => !ignored.Contains(c.Uid.ToLower()));

            modLoaders.AddRange(
                compatibleComponents.Select(component =>
                    new ModLoaderInfo(component.Name.ToLower(), component.Version))
            );
        }

        public override void Install(MmcMinecraftInstance instance, string modLoader, string version) {
            MmcPackFile pck = instance.PackFile;

            if (pck.Exists) {
                pck.Load();

                string mlUid = ModLoadersMap[modLoader].Key;
                MmcPackFile.Component forgeComponent = pck.Components.FirstOrDefault(c => c.Uid == mlUid);
                if (forgeComponent == null) {
                    // Install
                    pck.Components.Add(new MmcPackFile.Component {
                        Name = modLoader,
                        Uid = mlUid,
                        Version = version,
                        CachedVersion = version,
                        // ReSharper disable once CoVariantArrayConversion
                        Requirements = new MmcPackFile.ComponentRequirement[] {
                            new MmcPackFile.ComponentSpecificRequirement {
                                Uid = "net.minecraft",
                                EquivalentTo = instance.MinecraftVersion
                            }
                        }
                    });
                } else {
                    // Update version
                    forgeComponent.Version = version;
                    forgeComponent.CachedVersion = version;
                }

                pck.Save();
            }

            MmcConfigFile cfg = instance.MmcConfig;
            string mlConfigKey = ModLoadersMap[modLoader].Value;

            cfg.Load();
            PropertyInfo mlProp = cfg.GetType().GetProperty(mlConfigKey);
            if (mlProp == null) {
                Logger.Warn($"Failed to find a modloader property with name {mlConfigKey} inside MultiMC config file.");
                Logger.Error($"Modloader couldn't be updated to version {version} in MultiMC config.");
            }
            else {
                mlProp.SetValue(cfg, version);
            }
            cfg.Save();
        }

        public override void Uninstall(MmcMinecraftInstance instance, string modLoader) {
            MmcPackFile pck = instance.PackFile;

            if (pck.Exists) {
                pck.Load();

                string mlUid = ModLoadersMap[modLoader].Key;
                pck.Components.Remove(instance.PackFile.Components.FirstOrDefault(c => c.Uid == mlUid));
                pck.Save();
            }

            MmcConfigFile cfg = instance.MmcConfig;
            string mlConfigKey = ModLoadersMap[modLoader].Value;

            cfg.Load();
            cfg.GetType().GetProperty(mlConfigKey)?.SetValue(cfg, string.Empty);
            cfg.Save();
        }

    }

}
