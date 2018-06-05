using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Packsly3.Core;
using Packsly3.Core.Common;
using Packsly3.Core.Launcher;
using Packsly3.Core.Launcher.Modloader;
using Packsly3.MultiMC.FileSystem;

namespace Packsly3.MultiMC.Launcher.Modloader {

    [Register]
    public class MmcModLoaderHandler : InstanceModLoaderHandler<MmcMinecraftInstance> {

        private static readonly Dictionary<string, KeyValuePair<string, string>> ModLoadersMap = new Dictionary<string, KeyValuePair<string, string>> {
            {
                "forge", new KeyValuePair<string, string>(
                    "net.minecraftforge",
                    "ForgeVersion"
                )
            },
            {
                "liteloader", new KeyValuePair<string, string>(
                    "com.mumfrey.liteloader",
                    "LiteloaderVersion"
                )
            }
        };

        public override bool IsCompatible(string modLoader)
            => ModLoadersMap.Keys.Contains(modLoader);

        public override void DetectModLoaders(MmcMinecraftInstance instance, List<ModLoaderInfo> modLoaders) {
            if (!instance.Pack.Exists)
                return;

            string[] ignored = {
                "org.lwjgl",
                "net.minecraft"
            };

            IEnumerable<MmcPackFile.Component> compatibleComponents =
                instance.Pack.Components.Where(c => !ignored.Contains(c.Uid.ToLower()));

            modLoaders.AddRange(
                compatibleComponents.Select(component =>
                    new ModLoaderInfo(instance.ModLoaderManager, component.Name.ToLower(), component.Version))
            );
        }

        public override void Install(MmcMinecraftInstance instance, string modLoader, string version) {
            MmcPackFile pck = instance.Pack;

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

            MmcConfigFile cfg = instance.Config;
            string mlConfigKey = ModLoadersMap[modLoader].Value;

            cfg.Load();
            cfg.GetType().GetProperty(mlConfigKey)?.SetValue(cfg, version);
            cfg.Save();
        }

        public override void Uninstall(MmcMinecraftInstance instance, string modLoader) {
            MmcPackFile pck = instance.Pack;

            if (pck.Exists) {
                pck.Load();

                string mlUid = ModLoadersMap[modLoader].Key;
                pck.Components.Remove(instance.Pack.Components.FirstOrDefault(c => c.Uid == mlUid));
                pck.Save();
            }

            MmcConfigFile cfg = instance.Config;
            string mlConfigKey = ModLoadersMap[modLoader].Value;

            cfg.Load();
            cfg.GetType().GetProperty(mlConfigKey)?.SetValue(cfg, string.Empty);
            cfg.Save();
        }

    }

}
