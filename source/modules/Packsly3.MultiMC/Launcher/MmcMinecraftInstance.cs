using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Packsly3.Core.FileSystem;
using Packsly3.Core.Launcher;
using Packsly3.Core.Launcher.Instance;
using Packsly3.Core.Launcher.Modloader;
using Packsly3.MultiMC.FileSystem;
using Packsly3.MultiMC.Launcher.Modloader;

namespace Packsly3.MultiMC.Launcher {

    public class MmcMinecraftInstance : IMinecraftInstance {

        public DirectoryInfo Location { get; }

        public EnvironmentVariables EnvironmentVariables { get; }

        public PackslyInstanceFile PackslyConfig { get; }

        public string Id
            => Location.Name;

        public string Name {
            get => Config.GetField<string>();
            set => Config.SetField(value);
        }

        public string MinecraftVersion {
            get => Config.GetField<string>();
            set => Config.SetField(value);
        }

        public Icon Icon { get; }

        public ModLoaderManager ModLoaderManager { get; }

        internal MmcConfigFile Config { get; }
        internal MmcPackFile Pack { get; }

        public MmcMinecraftInstance(DirectoryInfo location) {
            Location = location;

            EnvironmentVariables = new EnvironmentVariables(this, new Dictionary<string, string> {
                { EnvironmentVariables.MinecraftFolder, Path.Combine(Location.FullName, ".minecraft")           },
                { EnvironmentVariables.ModsFolder,      Path.Combine(Location.FullName, ".minecraft", "mods")   },
                { EnvironmentVariables.ConfigFolder,    Path.Combine(Location.FullName, ".minecraft", "config") }
            });

            PackslyConfig = new PackslyInstanceFile(Path.Combine(Location.FullName));

            Config = new MmcConfigFile(Location.FullName);
            if (!Config.Exists) {
                Config.WithDefaults();
            }

            Pack = new MmcPackFile(Location.FullName);
            Pack.Load();

            Icon = new Icon(Path.Combine(LauncherEnvironment.Workspace.FullName, "icons"), Config.IconName);
            Icon.IconChanged += (sender, args)
                => Config.IconName = (sender as Icon)?.Source;

            ModLoaderManager = new ModLoaderManager(this);
        }

        public void Configure(string json) {
            JsonConvert.PopulateObject(json, Config);
        }

        public void Save() {
            if (!Location.Exists) {
                Location.Create();
            }

            Config.Save();
        }

        public void Delete() {
            if (Location.Exists) {
                Location.Delete();
            }
        }

    }

}
