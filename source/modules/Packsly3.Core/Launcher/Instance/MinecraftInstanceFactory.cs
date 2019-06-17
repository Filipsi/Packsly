using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using NLog;
using Packsly3.Core.Modpack;
using Packsly3.Core.Modpack.Model;

namespace Packsly3.Core.Launcher.Instance {

    internal static class MinecraftInstanceFactory {

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static IMinecraftInstance CreateFromModpack(FileInfo modpackFile) {
            if (!modpackFile.Exists) {
                throw new FileNotFoundException($"File '{modpackFile.FullName}' does not exist!");
            }

            using (StreamReader reader = modpackFile.OpenText()) {
                logger.Debug($"Reading modpack definition file from {modpackFile.FullName}");
                return CreateFromModpack(Path.GetFileNameWithoutExtension(modpackFile.FullName), reader.ReadToEnd());
            }
        }

        public static IMinecraftInstance CreateFromModpack(Uri modpackFileUrl) {
            using (WebClient client = new WebClient()) {
                client.Encoding = Encoding.UTF8;
                logger.Debug($"Downloading modpack definition from {modpackFileUrl}");
                return CreateFromModpack(Path.GetFileNameWithoutExtension(modpackFileUrl.AbsolutePath), client.DownloadString(modpackFileUrl));
            }
        }

        public static IMinecraftInstance CreateFromModpack(string instanceId, string modpackJson) {
            ModpackDefinition modpackDefinition = JsonConvert.DeserializeObject<ModpackDefinition>(modpackJson);

            IMinecraftInstance instance = CreateMinecraftInstnace(instanceId, modpackDefinition);
            Packsly.Lifecycle.EventBus.Publish(instance, Lifecycle.PreInstallation);

            // Install modloaders
            foreach (KeyValuePair<string, string> modloaderEntry in modpackDefinition.ModLoaders) {
                string name = modloaderEntry.Key;
                string version = modloaderEntry.Value;

                logger.Info($"Installing modloader '{name}' with version '{version}'...");
                instance.ModLoaderManager.Install(name, version);
            }

            // Download mods
            foreach (ModSource mod in modpackDefinition.Mods) {
                if (mod.ShouldDownload) {
                    logger.Info($"Downloading mod '{mod.FileName}' to '{mod.FilePath}'...");
                    instance.Files.Download(mod, FileManager.GroupType.Mod);

                } else {
                    logger.Info($"Skipping downloading of mod '{mod.FileName}' since it is {(mod.EnvironmentOnly.IsBlacklist ? "blacklisted" : "whitelisted")} at '{string.Join(", ", mod.EnvironmentOnly.Entries)}'...");
                }

                // Download mod resources
                foreach (RemoteResource resource in mod.Resources) {
                    if (resource.ShouldDownload) {
                        logger.Info($"Downloading resource '{resource.FileName}' to '{resource.FilePath}'...");
                        instance.Files.Download(resource, FileManager.GroupType.ModResource);

                    } else {
                        logger.Info($"Skipping downloading of resource '{resource.FileName}' since it is {(resource.EnvironmentOnly.IsBlacklist ? "blacklisted" : "whitelisted")} at '{string.Join(", ", resource.EnvironmentOnly.Entries)}'...");
                    }
                }
            }

            instance.Save();
            Packsly.Lifecycle.EventBus.Publish(instance, Lifecycle.PostInstallation);
            return instance;
        }

        private static IMinecraftInstance CreateMinecraftInstnace(string instanceId, ModpackDefinition modpackDefinition) {
            IMinecraftInstance instance = Packsly.Launcher.CreateInstance(instanceId);

            // Set base minecraft instance properties
            instance.Name = modpackDefinition.Name;
            instance.MinecraftVersion = modpackDefinition.MinecraftVersion;
            instance.Icon.Source = modpackDefinition.Icon;
            logger.Debug($"Setting minecraft instance properties id={instance.Id} name={instance.Name} mc={instance.MinecraftVersion} icon={instance.Icon.Source}");

            // Load defaults
            instance.Load();

            // Save adapters defined in modpack along with configuration to packsly instance config file
            foreach (KeyValuePair<string, object> adapterDefinition in modpackDefinition.Adapters) {
                string adapterName = adapterDefinition.Key;
                object adapterSettings = adapterDefinition.Value;

                logger.Debug($"Setting adapter config for {adapterName} for minecraft instance {instance.Id}");
                instance.PackslyConfig.Adapters.SetConfigFor(adapterName, adapterSettings);
            }

            // Configure instance using compatible environment settings
            foreach (KeyValuePair<string, object> environmentEntry in modpackDefinition.Environments) {
                string name = environmentEntry.Key;
                if (name != Packsly.Launcher.Name) {
                    continue;
                }

                string settings = environmentEntry.Value.ToString();
                logger.Debug($"Configuring minecraft instance {instance.Id} from modpack environment settings: {settings}");
                instance.Configure(settings);
                break;
            }

            return instance;
        }

    }

}
