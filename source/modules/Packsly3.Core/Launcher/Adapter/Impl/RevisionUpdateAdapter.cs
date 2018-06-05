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

        // TODO: Updating launcher settings from enviroment?
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

        private void UpdateModloaders(IMinecraftInstance instance, ModpackDefinition modpack) {
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

        // TODO: This should probably be done with manager
        private void UpdateMods(IMinecraftInstance instance, ModpackDefinition modpack) {
            DirectoryInfo modsFolder = new DirectoryInfo(instance.EnvironmentVariables.GetProperty(EnvironmentVariables.ModsFolder));
            FileInfo[] mods = modsFolder.GetFiles("*.jar");

            // Delete mods that are not in the modpack
            foreach (FileInfo modFile in mods) {
                if (modpack.Mods.Any(mod => mod.FileName == modFile.Name))
                    continue;

                Console.WriteLine($" > Removing mod '{modFile.Name}'...");
                modFile.Delete();
            }

            // Update mods
            using (WebClient client = new WebClient()) {
                foreach (ModSource mod in modpack.Mods) {
                    DirectoryInfo destinationFolder = new DirectoryInfo(instance.EnvironmentVariables.Format(mod.FilePath));

                    if (!destinationFolder.Exists) {
                        destinationFolder.Create();
                    }

                    FileInfo modDestination = new FileInfo(Path.Combine(destinationFolder.FullName, mod.FileName));
                    if (!modDestination.Exists) {
                        Console.WriteLine($" > Downloading mod '{mod.FileName}' to '{destinationFolder.FullName}'...");
                        client.DownloadFile(mod.Url, modDestination.FullName);
                    }

                    // Update mod resources
                    foreach (RemoteResource resource in mod.Resources) {
                        DirectoryInfo envResPath = new DirectoryInfo(instance.EnvironmentVariables.Format(resource.FilePath));

                        if (!envResPath.Exists) {
                            envResPath.Create();
                        }

                        FileInfo resourceDestination = new FileInfo(Path.Combine(destinationFolder.FullName, mod.FileName));
                        if (!resourceDestination.Exists) {
                            Console.WriteLine($"  - Downloading resource '{resource.FileName}' to '{envResPath.FullName}'...");
                            client.DownloadFile(resource.Url, Path.Combine(envResPath.FullName, resource.FileName));
                        }
                    }
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
