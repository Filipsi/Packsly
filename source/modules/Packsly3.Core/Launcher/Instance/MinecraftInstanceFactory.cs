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

        public static IMinecraftInstance CreateFromModpack(string path) {
            FileInfo file = new FileInfo(path);

            if (!file.Exists) {
                throw new FileNotFoundException($"File '{file.FullName}' does not exist!");
            }

            using (StreamReader reader = file.OpenText()) {
                ModpackDefinition modpackDefinition = JsonConvert.DeserializeObject<ModpackDefinition>(reader.ReadToEnd());
                IMinecraftInstance instance = LauncherEnvironment.CreateInstance(Path.GetFileNameWithoutExtension(file.Name));

                // Instance properties
                instance.Name = modpackDefinition.Name;
                instance.MinecraftVersion = modpackDefinition.MinecraftVersion;
                instance.Icon.Source = modpackDefinition.Icon;

                // Enviroments
                foreach (KeyValuePair<string, object> environmentEntry in modpackDefinition.Environments) {
                    string name = environmentEntry.Key;
                    if (name != LauncherEnvironment.Current.Name)
                        continue;

                    string config = environmentEntry.Value.ToString();
                    instance.Configure(config);
                }

                instance.Save();

                // Install or update modloaders
                foreach (KeyValuePair<string, string> modloaderEntry in modpackDefinition.ModLoaders) {
                    string name = modloaderEntry.Key;
                    string version = modloaderEntry.Value;

                    instance.ModLoaderManager.Install(name, version);
                }

                // Remove unused modloaders
                IEnumerable<ModLoader> oldModLoaders =
                    instance.ModLoaderManager.ModLoaders.Where(ml => !modpackDefinition.ModLoaders.ContainsKey(ml.Name));
                foreach (ModLoader modLoader in oldModLoaders)
                    modLoader.Uninstall();

                EnvironmentVariables instanceVariables = new EnvironmentVariables(instance);

                // Download mods
                using (WebClient client = new WebClient()) {
                    foreach (ModSource modSource in modpackDefinition.Mods) {
                        DirectoryInfo envModPath =  new DirectoryInfo(instanceVariables.Format(modSource.FilePath));
                        Console.WriteLine($"Downloading mod '{modSource.FileName}' to '{envModPath.FullName}'...");

                        if (!envModPath.Exists) {
                            envModPath.Create();
                        }

                        client.DownloadFile(modSource.Url, Path.Combine(envModPath.FullName, modSource.FileName));

                        // Download mod resources
                        foreach (RemoteResource resource in modSource.Resources) {
                            DirectoryInfo envResPath = new DirectoryInfo(instanceVariables.Format(resource.FilePath));
                            Console.WriteLine($" > Downloading resource '{resource.FileName}' to '{envResPath.FullName}'...");

                            if (!envResPath.Exists) {
                                envResPath.Create();
                            }

                            client.DownloadFile(resource.Url, Path.Combine(envResPath.FullName, resource.FileName));
                        }
                    }
                }

                return instance;
            }
        }

    }

}
