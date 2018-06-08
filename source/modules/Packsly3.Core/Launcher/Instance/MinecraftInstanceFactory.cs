using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Packsly3.Core.Launcher.Modloader;
using Packsly3.Core.Modpack;

namespace Packsly3.Core.Launcher.Instance {

    public static class MinecraftInstanceFactory {

        public static IMinecraftInstance CreateFromModpack(FileInfo modpackFile) {
            if (!modpackFile.Exists) {
                throw new FileNotFoundException($"File '{modpackFile.FullName}' does not exist!");
            }

            using (StreamReader reader = modpackFile.OpenText()) {
                return CreateFromModpack(Path.GetFileNameWithoutExtension(modpackFile.FullName), reader.ReadToEnd());
            }
        }

        public static IMinecraftInstance CreateFromModpack(Uri modpackFileUrl) {
            using (WebClient client = new WebClient()) {
                return CreateFromModpack(Path.GetFileNameWithoutExtension(modpackFileUrl.AbsolutePath), client.DownloadString(modpackFileUrl));
            }
        }

        public static IMinecraftInstance CreateFromModpack(string instanceId, string modpackJson) {
            ModpackDefinition modpackDefinition = JsonConvert.DeserializeObject<ModpackDefinition>(modpackJson);
            IMinecraftInstance instance = CreateMinecraftInstnace(instanceId, modpackDefinition);

            Lifecycle.Dispatcher.Publish(instance, Lifecycle.PreInstallation);

            // Install modloaders
            foreach (KeyValuePair<string, string> modloaderEntry in modpackDefinition.ModLoaders) {
                string name = modloaderEntry.Key;
                string version = modloaderEntry.Value;

                instance.ModLoaderManager.Install(name, version);
            }

            // Download mods
            foreach (ModSource mod in modpackDefinition.Mods) {
                Console.WriteLine($"Downloading mod '{mod.FileName}' to '{mod.FilePath}'...");
                instance.Files.Download(mod, FileManager.GroupType.Mod);

                // Download mod resources
                foreach (RemoteResource resource in mod.Resources) {
                    Console.WriteLine($" > Downloading resource '{resource.FileName}' to '{resource.FilePath}'...");
                    instance.Files.Download(resource, FileManager.GroupType.ModResource);
                }
            }
            instance.Files.Save();

            Lifecycle.Dispatcher.Publish(instance, Lifecycle.PostInstallation);

            return instance;
        }

        private static IMinecraftInstance CreateMinecraftInstnace(string instanceId, ModpackDefinition modpackDefinition) {
            IMinecraftInstance instance = Launcher.CreateInstance(instanceId);

            // Set base minecraft instance properties
            instance.Name = modpackDefinition.Name;
            instance.MinecraftVersion = modpackDefinition.MinecraftVersion;
            instance.Icon.Source = modpackDefinition.Icon;

            // Save adapters defined in modpack along with configuration to packsly instnace config file
            foreach (KeyValuePair<string, object> adapterDefinition in modpackDefinition.Adapters) {
                string adapterName = adapterDefinition.Key;
                object adapterSettings = adapterDefinition.Value;

                instance.PackslyConfig.Adapters.SetConfigFor(adapterName, adapterSettings);
                instance.PackslyConfig.Save();
            }

            // Configure instance using compatible enviroment settings
            foreach (KeyValuePair<string, object> environmentEntry in modpackDefinition.Environments) {
                string name = environmentEntry.Key;
                if (name != Launcher.Current.Name)
                    continue;

                string settings = environmentEntry.Value.ToString();
                instance.Configure(settings);
                break;
            }

            instance.Save();
            return instance;
        }

    }

}
