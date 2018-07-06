using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Packsly3.Core.Common;
using Packsly3.Core.Launcher.Instance;
using Packsly3.Core.Launcher.Modloader;
using Packsly3.Core.Modpack;

namespace Packsly3.Core.Launcher.Adapter.Impl {

    [Register]
    public class RevisionUpdateAdapter : Adapter<RevisionUpdateSchemaConfig> {

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

            Lifecycle.Dispatcher.Publish(instance, Lifecycle.UpdateStarted);
            Console.WriteLine("Checking for modpack updates...");

            using (WebClient client = new WebClient()) {
                ModpackDefinition modpack = JsonConvert.DeserializeObject<ModpackDefinition>(client.DownloadString(config.UpdateUrl));

                if (!modpack.Adapters.ContainsKey(Id)) {
                    throw new InvalidOperationException(
                        $"Revision based updater '{GetType().FullName}' failed to obtain revision number from update source because adapter configuration is missing.");
                }

                RevisionUpdateSchemaConfig remoteConfig =
                    JObject.FromObject(modpack.Adapters[Id]).ToObject<RevisionUpdateSchemaConfig>();

                if (config.Revision != remoteConfig.Revision) {
                    Console.WriteLine($" > Updating modpack from revision {config.Revision} to {remoteConfig.Revision}!");
                    UpdateModloaders(instance, modpack);
                    UpdateMods(instance, modpack);
                }
            }

            Console.WriteLine("Modpack is up to date.");
            Lifecycle.Dispatcher.Publish(instance, Lifecycle.UpdateFinished);
        }

        #endregion

        private static void UpdateModloaders(IMinecraftInstance instance, ModpackDefinition modpack) {
            // Install or update modloaders
            foreach (KeyValuePair<string, string> modloaderEntry in modpack.ModLoaders) {
                string name = modloaderEntry.Key;
                string version = modloaderEntry.Value;

                if (!instance.ModLoaderManager.ModLoaders.Any(ml => ml.Name == name && ml.Version != version))
                    continue;

                Console.WriteLine($" > Installing modloader '{name}' version '{version}'...");
                instance.ModLoaderManager.Install(name, version);
            }

            // Remove unused modloaders
            IEnumerable<ModLoaderInfo> oldModLoaders = instance.ModLoaderManager.ModLoaders.Where(ml => !modpack.ModLoaders.ContainsKey(ml.Name));
            foreach (ModLoaderInfo modLoader in oldModLoaders) {
                Console.WriteLine($" > Uninstalling modloader '{modLoader.Name}' version '{modLoader.Version}'");
                modLoader.Uninstall();
            }
        }

        private static void UpdateMods(IMinecraftInstance instance, ModpackDefinition modpack) {
            foreach (FileInfo modFile in instance.Files.GetGroup(FileManager.GroupType.Mod)) {
                if (modpack.Mods.Any(mm => mm.FileName == modFile.Name && mm.ShouldDownload)) {
                    continue;
                }

                Console.WriteLine($" > Removing mod {modFile.Name}...");
                instance.Files.Remove(modFile, FileManager.GroupType.Mod);
                modFile.Delete();
            }

            RemoteResource[] modResourceBlob = modpack.Mods.SelectMany(m => m.Resources).ToArray();
            foreach (FileInfo modResourceFile in instance.Files.GetGroup(FileManager.GroupType.ModResource)) {
                if (modResourceBlob.Any(mr => mr.FileName == modResourceFile.Name && mr.ShouldDownload)) {
                    continue;
                }

                Console.WriteLine($" > Removing mod resource {modResourceFile.Name}...");
                instance.Files.Remove(modResourceFile, FileManager.GroupType.ModResource);
                modResourceFile.Delete();
            }

            foreach (ModSource modpackMod in modpack.Mods.Where(mod => mod.ShouldDownload)) {
                if (!instance.Files.GroupContains(FileManager.GroupType.Mod, modpackMod)) {
                    Console.WriteLine($" > Downloading mod {modpackMod.FileName}...");
                    instance.Files.Download(modpackMod, FileManager.GroupType.Mod);
                }

                foreach (RemoteResource modpackModResource in modpackMod.Resources.Where(resource => resource.ShouldDownload)) {
                    Console.WriteLine($" > Downloading mod resource {modpackModResource.FileName}...");
                    instance.Files.Download(modpackModResource, FileManager.GroupType.ModResource);
                }
            }

            instance.Files.Save();
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
