using Newtonsoft.Json;
using Packsly3.Core.FileSystem.Impl;
using Packsly3.Core.Launcher.Instance;
using Packsly3.Core.Launcher.Instance.Logic;
using Packsly3.Core.Launcher.Modloader;
using Packsly3.Server.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Packsly3.Server.Launcher {

    public class ServerMinecraftInstance : IMinecraftInstance {

        #region Properties

        public DirectoryInfo Location { get; }

        public EnvironmentVariables EnvironmentVariables { get; }

        public PackslyInstanceFile PackslyConfig { get; }

        public string Id
            => Location.Name;

        public string Name {
            get => PackslyConfig.Get<string>(this, "name");
            set => PackslyConfig.Set(this, "name", value);
        }

        public string MinecraftVersion {
            get => PackslyConfig.Get<string>(this, "minecraft");
            set => PackslyConfig.Set(this, "minecraft", value);
        }

        public Icon Icon { get; }

        public ModLoaderManager ModLoaderManager { get; }

        public FileManager Files { get; }

        internal ServerPropertiesFile ServerProperties { get; }

        #endregion

        public ServerMinecraftInstance(DirectoryInfo location) {
            Location = location;

            EnvironmentVariables = new EnvironmentVariables(this, new Dictionary<string, string> {
                { EnvironmentVariables.RootFolder,     Location.FullName                         },
                { EnvironmentVariables.InstnaceFolder, Location.FullName                         },
                { EnvironmentVariables.ModsFolder,     Path.Combine(Location.FullName, "mods")   },
                { EnvironmentVariables.ConfigFolder,   Path.Combine(Location.FullName, "config") }
            });

            Icon = new Icon(
                iconFolder: Location.FullName,
                iconSource: "server-icon",
                nameOverride: "server-icon"
            );

            PackslyConfig = new PackslyInstanceFile(Location.FullName);
            ModLoaderManager = new ModLoaderManager(this);
            Files = new FileManager(this);

            ServerProperties = new ServerPropertiesFile(Location.FullName);
            if (!ServerProperties.Exists) {
                ServerProperties.WithDefaults();
            }

            // TODO: Server launch script
        }

        public void Configure(string json) {
            JsonConvert.PopulateObject(json, ServerProperties);
        }

        public void Save() {
            ServerProperties.Save();
        }

        public void Delete() {
            if (Icon.IconFile.Exists) {
                Icon.IconFile.Delete();
            }

            if (ServerProperties.Exists) {
                ServerProperties.Delete();
            }

            string[] foldersToDelete = new[] {
                EnvironmentVariables.GetProperty(EnvironmentVariables.ModsFolder),
                EnvironmentVariables.GetProperty(EnvironmentVariables.ConfigFolder)
            };

            foreach (string folder in foldersToDelete) {
                if (Directory.Exists(folder)) {
                    Directory.Delete(folder);
                }
            }
        }

    }

}
