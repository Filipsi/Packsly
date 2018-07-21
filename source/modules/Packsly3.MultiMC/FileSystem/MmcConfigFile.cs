using Packsly3.Core.FileSystem;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace Packsly3.MultiMC.FileSystem {

    [JsonObject(MemberSerialization.OptIn)]
    internal sealed class MmcConfigFile : DataPairFile {

        public string Name {
            set => Set("name", value);
            get => Get<string>("name");
        }

        [JsonProperty(PropertyName = "instanceType")]
        public string InstanceType {
            set => Set("InstanceType", value);
            get => Get<string>("InstanceType");
        }

        public string MinecraftVersion {
            set => Set("IntendedVersion", value);
            get => Get<string>("IntendedVersion");
        }

        public string ForgeVersion {
            set => Set("ForgeVersion", value);
            get => Get<string>("ForgeVersion");
        }

        public string LiteloaderVersion {
            set => Set("LiteloaderVersion", value);
            get => Get<string>("LiteloaderVersion");
        }

        [JsonProperty(PropertyName = "mcLaunchMethod")]
        public string McLaunchMethod {
            set => Set("MCLaunchMethod", value);
            get => Get<string>("MCLaunchMethod");
        }

        public bool OverrideCommands {
            private set => Set("OverrideCommands", value);
            get         => Get<bool>("OverrideCommands");
        }

        [JsonProperty(PropertyName = "preLaunchCommand")]
        public string PreLaunchCommand {
            set {
                OverrideCommands = !string.IsNullOrEmpty(WrapperCommand) || !string.IsNullOrEmpty(PostExitCommand) || !string.IsNullOrEmpty(value);
                Set("PreLaunchCommand", value);
            }
            get => Get<string>("PreLaunchCommand");
        }

        [JsonProperty(PropertyName = "postExitCommand")]
        public string PostExitCommand {
            set {
                OverrideCommands = !string.IsNullOrEmpty(WrapperCommand) || !string.IsNullOrEmpty(PreLaunchCommand) || !string.IsNullOrEmpty(value);
                Set("PostExitCommand", value);
            }
            get => Get<string>("PostExitCommand");
        }

        [JsonProperty(PropertyName = "wrapperCommand")]
        public string WrapperCommand {
            set {
                OverrideCommands = !string.IsNullOrEmpty(PostExitCommand) || !string.IsNullOrEmpty(PreLaunchCommand) || !string.IsNullOrEmpty(value);
                Set("WrapperCommand", value);
            }
            get => Get<string>("WrapperCommand");
        }

        public bool OverrideConsole {
            private set => Set("OverrideConsole", value);
            get         => Get<bool>("OverrideConsole");
        }

        [JsonProperty(PropertyName = "autoCloseConsole")]
        public bool AutoCloseConsole {
            set {
                OverrideConsole = ShowConsoleOnError || !ShowConsole || value;
                Set("AutoCloseConsole", value);
            }
            get => Get<bool>("AutoCloseConsole");
        }

        [JsonProperty(PropertyName = "showConsole")]
        public bool ShowConsole {
            set {
                OverrideConsole = ShowConsoleOnError || AutoCloseConsole || !value;
                Set("ShowConsole", value);
            }
            get => Get<bool>("ShowConsole");
        }

        [JsonProperty(PropertyName = "showConsoleOnError")]
        public bool ShowConsoleOnError {
            set {
                OverrideConsole = AutoCloseConsole || !ShowConsole || value;
                Set("ShowConsoleOnError", value);
            }
            get => Get<bool>("ShowConsoleOnError");
        }

        public bool AreJavaArgumentsOverridden {
            private set => Set("OverrideJavaArgs", value);
            get         => Get<bool>("OverrideJavaArgs");
        }

        [JsonProperty(PropertyName = "javaArguments")]
        public string JavaArguments {
            set {
                AreJavaArgumentsOverridden = !string.IsNullOrEmpty(value);
                Set("JvmArgs", value);
            }
            get => Get<string>("JvmArgs");
        }

        public bool IsJavaLocationOverridden {
            private set => Set("OverrideJavaLocation", value);
            get         => Get<bool>("OverrideJavaLocation");
        }

        [JsonProperty(PropertyName = "javaPath")]
        public string JavaPath {
            set {
                IsJavaLocationOverridden = value == null;
                Set("JavaPath", value);
            }
            get => Get<string>("JavaPath");
        }

        public bool IsMemoryAllocationOverridden {
            private set => Set("OverrideMemory", value);
            get         => Get<bool>("OverrideMemory");
        }

        [JsonProperty(PropertyName = "maxMemoryAllocation")]
        public int MaxMemoryAllocation {
            set {
                IsMemoryAllocationOverridden = MaxMemoryAllocation > 0 || value > 0;
                Set("MaxMemAlloc", IsMemoryAllocationOverridden ? value : 0);
            }
            get => Get<int>("MaxMemAlloc");
        }

        [JsonProperty(PropertyName = "minMemoryAllocation")]
        public int MinMemoryAllocation {
            set {
                IsMemoryAllocationOverridden = MaxMemoryAllocation > 0 || value > 0;
                Set("MinMemAlloc", IsMemoryAllocationOverridden ? value : 0);
            }
            get => Get<int>("MinMemAlloc");
        }

        public bool AreWindowSettingsOverridden {
            private set => Set("OverrideWindow", value);
            get => Get<bool>("OverrideWindow");
        }

        [JsonProperty(PropertyName = "launchMaximized")]
        public bool LaunchMaximized {
            set {
                AreWindowSettingsOverridden = value || WindowWidth > 0 || WindowHeight > 0;
                Set("LaunchMaximized", value);
            }
            get => Get<bool>("LaunchMaximized");
        }

        [JsonProperty(PropertyName = "windowHeight")]
        public int WindowHeight {
            set {
                AreWindowSettingsOverridden = LaunchMaximized || WindowWidth > 0 || value > 0;
                Set("MinecraftWinHeight", IsMemoryAllocationOverridden ? value : 0);
            }
            get => Get<int>("MinecraftWinHeight");
        }

        [JsonProperty(PropertyName = "windowWidth")]
        public int WindowWidth {
            set {
                AreWindowSettingsOverridden = LaunchMaximized || WindowHeight > 0 || value > 0;
                Set("MinecraftWinWidth", IsMemoryAllocationOverridden ? value : 0);
            }
            get => Get<int>("MinecraftWinWidth");
        }

        public string IconName {
            set => Set("iconKey", value);
            get => Get<string>("iconKey");
        }

        [JsonProperty(PropertyName = "notes")]
        public string Notes {
            set => Set("notes", value);
            get => Get<string>("notes");
        }

        public bool IsLoaded { private set; get; }

        public MmcConfigFile(string path) : base(Path.Combine(path, "instance.cfg")) {
        }

        public MmcConfigFile WithDefaults() {
            InstanceType = "OneSix";
            McLaunchMethod = "LauncherPart";
            OverrideCommands = false;
            OverrideConsole = false;
            AreJavaArgumentsOverridden = false;
            IsJavaLocationOverridden = false;
            IsMemoryAllocationOverridden = false;
            AreWindowSettingsOverridden = false;
            IconName = "default";
            Notes = string.Empty;
            return this;
        }

        public override void Save() {
            base.Save();
            IsLoaded = true;
        }

        public void SetField(object value, [CallerMemberName] string propertyName = "") {
            PropertyInfo prop = GetType()
                .GetProperties()
                .FirstOrDefault(p => p.Name == propertyName);

            if (prop == null) {
                throw new MissingFieldException($"There is no filed with name {propertyName}!");
            }

            if (Exists && !IsLoaded) {
                IsLoaded = true;
                Load();
            }

            prop.SetValue(this, value);
        }

        public T GetField<T>([CallerMemberName] string propertyName = "") {
            PropertyInfo prop = GetType()
                .GetProperties()
                .FirstOrDefault(p => p.Name == propertyName);

            if (prop == null) {
                throw new MissingFieldException($"There is no filed with name {propertyName}!");
            }

            if (Exists && !IsLoaded) {
                IsLoaded = true;
                Load();
            }

            return (T)prop.GetValue(this);
        }

    }

}
