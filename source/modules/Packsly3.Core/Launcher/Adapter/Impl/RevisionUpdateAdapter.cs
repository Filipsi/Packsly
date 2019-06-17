using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using Packsly3.Core.Common.Register;
using Packsly3.Core.Launcher.Instance;
using Packsly3.Core.Launcher.Modloader;
using Packsly3.Core.Modpack;
using Packsly3.Core.Modpack.Model;

namespace Packsly3.Core.Launcher.Adapter.Impl {

    [Register]
    public class RevisionUpdateAdapter : Adapter<RevisionUpdateSchemaConfig> {

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #region Adapter

        public override string Id { get; }
            = "core.updater.revision";

        public override bool IsCompatible(IMinecraftInstance instance)
            => true;

        public override bool IsCompatible(string lifecycleEvent)
            => lifecycleEvent == Lifecycle.PreLaunch;

        public override void Execute(RevisionUpdateSchemaConfig config, string lifecycleEvent, IMinecraftInstance instance) {
            if (!Uri.IsWellFormedUriString(config.UpdateUrl, UriKind.Absolute)) {
                throw new FormatException($"Revision based updater '{GetType().FullName}' could not resolve update url '{config.UpdateUrl}' provided by configuration.");
            }

            logger.Info("Checking for modpack updates...");

            using (WebClient client = new WebClient()) {
                ModpackDefinition remoteModpack = JsonConvert.DeserializeObject<ModpackDefinition>(client.DownloadString(config.UpdateUrl));

                // Check if remote modpack has configuration for this adapter type
                // Without this we can't check for revision number changes and perform the update
                if (!remoteModpack.Adapters.ContainsKey(Id)) {
                    throw new InvalidOperationException(
                        $"Revision based updater '{GetType().FullName}' failed to obtain revision number from update source because adapter configuration is missing.");
                }

                RevisionUpdateSchemaConfig remoteConfig =
                    JObject.FromObject(remoteModpack.Adapters[Id]).ToObject<RevisionUpdateSchemaConfig>();

                // If there is an update available
                if (config.Revision != remoteConfig.Revision) {
                    Packsly.Lifecycle.EventBus.Publish(instance, Lifecycle.UpdateStarted);
                    logger.Info($"Updating modpack from revision {config.Revision} to {remoteConfig.Revision}!");

                    // Update instance config
                    // TODO: Add config option to the update adapter for this
                    /*
                    foreach (KeyValuePair<string, object> environmentEntry in remoteModpack.Environments) {
                        if (environmentEntry.Key != Packsly.Launcher.Name) {
                            continue;
                        }

                        string settings = environmentEntry.Value.ToString();
                        Logger.Debug($"Updating configuration of minecraft instance {instance.Id} from modpack environment settings: {settings}");
                        instance.Configure(settings);
                        break;
                    }
                    */

                    // Update modloaders and mods
                    UpdateModloaders(instance, remoteModpack);
                    UpdateMods(instance, remoteModpack);

                    // Update adapter settings saved in instnace.packsly file, because revision number has changed
                    Save(instance, remoteConfig);
                    Packsly.Lifecycle.EventBus.Publish(instance, Lifecycle.UpdateFinished);
                }
            }

            logger.Info("Modpack is up to date.");
        }

        #endregion

        private static void UpdateModloaders(IMinecraftInstance instance, ModpackDefinition modpack) {
            // Install or update modloaders
            foreach (KeyValuePair<string, string> modloaderEntry in modpack.ModLoaders) {
                string name = modloaderEntry.Key;
                string version = modloaderEntry.Value;

                if (!instance.ModLoaderManager.ModLoaders.Any(ml => ml.Name == name && ml.Version != version))
                    continue;

                logger.Info($"Installing modloader '{name}' version '{version}'...");
                instance.ModLoaderManager.Install(name, version);
            }

            // Remove unused modloaders
            IEnumerable<ModLoaderInfo> oldModLoaders = instance.ModLoaderManager.ModLoaders.Where(ml => !modpack.ModLoaders.ContainsKey(ml.Name));
            foreach (ModLoaderInfo modLoader in oldModLoaders) {
                logger.Info($"Uninstalling modloader '{modLoader.Name}' version '{modLoader.Version}'");
                instance.ModLoaderManager.Uninstall(modLoader.Name);
            }
        }

        private static void UpdateMods(IMinecraftInstance instance, ModpackDefinition modpack) {
            foreach (FileInfo modFile in instance.Files.GetGroup(FileManager.GroupType.Mod)) {
                if (modpack.Mods.Any(mm => mm.FileName == modFile.Name && mm.ShouldDownload)) {
                    continue;
                }

                logger.Info($"Removing mod {modFile.Name}...");
                instance.Files.Remove(modFile, FileManager.GroupType.Mod);
                modFile.Delete();
            }

            RemoteResource[] modResourceBlob = modpack.Mods.SelectMany(m => m.Resources).ToArray();
            foreach (FileInfo modResourceFile in instance.Files.GetGroup(FileManager.GroupType.ModResource)) {
                if (modResourceBlob.Any(mr => mr.FileName == modResourceFile.Name && mr.ShouldDownload)) {
                    continue;
                }

                logger.Info($"Removing mod resource {modResourceFile.Name}...");
                instance.Files.Remove(modResourceFile, FileManager.GroupType.ModResource);
                modResourceFile.Delete();
            }

            foreach (ModSource modpackMod in modpack.Mods.Where(mod => mod.ShouldDownload)) {
                if (!instance.Files.GroupContains(FileManager.GroupType.Mod, modpackMod)) {
                    logger.Info($"Downloading mod {modpackMod.FileName}...");
                    instance.Files.Download(modpackMod, FileManager.GroupType.Mod);
                }

                foreach (RemoteResource modpackModResource in modpackMod.Resources.Where(resource => resource.ShouldDownload)) {
                    logger.Info($"Downloading mod resource {modpackModResource.FileName}...");
                    instance.Files.Download(modpackModResource, FileManager.GroupType.ModResource);
                }
            }
        }

    }

    [JsonObject(MemberSerialization.OptIn)]
    public class RevisionUpdateSchemaConfig {

        [JsonProperty("url")]
        public string UpdateUrl { private set; get; }

        [JsonProperty("revision")]
        public uint Revision { private set; get; }

    }

}
