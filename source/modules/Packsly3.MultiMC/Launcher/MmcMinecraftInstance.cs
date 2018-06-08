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
            get => MmcConfig.GetField<string>();
            set => MmcConfig.SetField(value);
        }

        public string MinecraftVersion {
            get => MmcConfig.GetField<string>();
            set => MmcConfig.SetField(value);
        }

        public Icon Icon { get; }

        public ModLoaderManager ModLoaderManager { get; }

        public FileManager Files { get; }

        internal MmcConfigFile MmcConfig { get; }

        internal MmcPackFile PackFile { get; }

        public MmcMinecraftInstance(DirectoryInfo location) {
            Location = location;

            EnvironmentVariables = new EnvironmentVariables(this, new Dictionary<string, string> {
                { EnvironmentVariables.MinecraftFolder, Path.Combine(Location.FullName, ".minecraft")           },
                { EnvironmentVariables.ModsFolder,      Path.Combine(Location.FullName, ".minecraft", "mods")   },
                { EnvironmentVariables.ConfigFolder,    Path.Combine(Location.FullName, ".minecraft", "config") }
            });

            PackslyConfig = new PackslyInstanceFile(Path.Combine(Location.FullName));

            MmcConfig = new MmcConfigFile(Location.FullName);
            if (!MmcConfig.Exists) {
                MmcConfig.WithDefaults();
            }

            PackFile = new MmcPackFile(Location.FullName);
            PackFile.Load();

            Icon = new Icon(Path.Combine(Core.Launcher.Launcher.Workspace.FullName, "icons"), MmcConfig.IconName);
            Icon.IconChanged += (sender, args)
                => MmcConfig.IconName = (sender as Icon)?.Source;

            ModLoaderManager = new ModLoaderManager(this);

            Files = new FileManager(this);
        }

        public void Configure(string json) {
            JsonConvert.PopulateObject(json, MmcConfig);
        }

        public void Save() {
            if (!Location.Exists) {
                Location.Create();
            }

            MmcConfig.Save();
        }

        public void Delete() {
            if (Location.Exists) {
                Location.Delete();
            }
        }

    }

}
