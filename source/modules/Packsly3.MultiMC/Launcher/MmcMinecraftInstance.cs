using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Packsly3.Core;
using Packsly3.Core.FileSystem.Impl;
using Packsly3.Core.Launcher.Instance;
using Packsly3.Core.Launcher.Modloader;
using Packsly3.MultiMC.FileSystem;

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
                { EnvironmentVariables.RootFolder,   Path.Combine(Location.FullName, ".minecraft")           },
                { EnvironmentVariables.ModsFolder,   Path.Combine(Location.FullName, ".minecraft", "mods")   },
                { EnvironmentVariables.ConfigFolder, Path.Combine(Location.FullName, ".minecraft", "config") }
            });

            PackslyConfig = new PackslyInstanceFile(Path.Combine(Location.FullName));

            MmcConfig = new MmcConfigFile(Location.FullName);

            PackFile = new MmcPackFile(Location.FullName);

            Icon = new Icon(Path.Combine(Packsly.Launcher.Workspace.FullName, "icons"), MmcConfig.IconName);
            Icon.IconChanged += (sender, args) => MmcConfig.IconName = (sender as Icon)?.Source;

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

            PackslyConfig.Save();
            MmcConfig.Save();
            PackFile.Save();
        }

        public void Load() {
            PackslyConfig.Load();

            if (MmcConfig.Exists) {
                MmcConfig.Load();

            } else {
                MmcConfig.WithDefaults();
                MmcConfig.IconName = Icon.Source;

                string runner = (Packsly.IsLinux ? "mono " : string.Empty) + "$INST_DIR/../../packsly/Packsly3.Cli.exe lifecycle $INST_ID";
                MmcConfig.PreLaunchCommand = $"{runner} {Lifecycle.PreLaunch}";
                MmcConfig.PostExitCommand = $"{runner} {Lifecycle.PostExit}";
            }

            if (PackFile.Exists) {
                PackFile.Load();
            } else {
                PackFile.WithDefaults(MinecraftVersion);
            }
        }

        public void Delete() {
            if (Location.Exists) {
                Location.Delete(true);
            }
        }

    }

}
