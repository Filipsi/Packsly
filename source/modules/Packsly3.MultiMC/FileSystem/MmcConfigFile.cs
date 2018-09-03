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

        #region Wrappers

        public string Name {
            get => Get<string>("name");
            set => Set("name", value);
        }

        public string MinecraftVersion {
            get => Get<string>("IntendedVersion");
            set => Set("IntendedVersion", value);
        }

        public string ForgeVersion {
            get => Get<string>("ForgeVersion");
            set => Set("ForgeVersion", value);
        }

        public string LiteloaderVersion {
            get => Get<string>("LiteloaderVersion");
            set => Set("LiteloaderVersion", value);
        }

        internal string PreLaunchCommand {
            get => Get<string>("PreLaunchCommand");
            set {
                OverrideCommands = !string.IsNullOrEmpty(WrapperCommand) || !string.IsNullOrEmpty(PostExitCommand) || !string.IsNullOrEmpty(value);
                Set("PreLaunchCommand", value);
            }
        }

        internal string PostExitCommand {
            get => Get<string>("PostExitCommand");
            set {
                OverrideCommands = !string.IsNullOrEmpty(WrapperCommand) || !string.IsNullOrEmpty(PreLaunchCommand) || !string.IsNullOrEmpty(value);
                Set("PostExitCommand", value);
            }
        }

        public bool OverrideConsole {
            get => Get<bool>("OverrideConsole");
            private set => Set("OverrideConsole", value);
        }

        public string IconName {
            get => Get<string>("iconKey");
            set => Set("iconKey", value);
        }

        #endregion

        #region Exposed wrappers

        [JsonProperty(PropertyName = "instanceType")]
        public string InstanceType {
            get => Get<string>("InstanceType");
            set => Set("InstanceType", value);
        }

        [JsonProperty(PropertyName = "mcLaunchMethod")]
        public string McLaunchMethod {
            get => Get<string>("MCLaunchMethod");
            set => Set("MCLaunchMethod", value);
        }

        [JsonProperty(PropertyName = "wrapperCommand")]
        public string WrapperCommand {
            get => Get<string>("WrapperCommand");
            set {
                OverrideCommands = !string.IsNullOrEmpty(PostExitCommand) || !string.IsNullOrEmpty(PreLaunchCommand) || !string.IsNullOrEmpty(value);
                Set("WrapperCommand", value);
            }
        }


        [JsonProperty(PropertyName = "autoCloseConsole")]
        public bool AutoCloseConsole {
            get => Get<bool>("AutoCloseConsole");
            set {
                OverrideConsole = ShowConsoleOnError || !ShowConsole || value;
                Set("AutoCloseConsole", value);
            }
        }

        [JsonProperty(PropertyName = "showConsole")]
        public bool ShowConsole {
            get => Get<bool>("ShowConsole");
            set {
                OverrideConsole = ShowConsoleOnError || AutoCloseConsole || value;
                Set("ShowConsole", value);
            }
        }

        [JsonProperty(PropertyName = "showConsoleOnError")]
        public bool ShowConsoleOnError {
            get => Get<bool>("ShowConsoleOnError");
            set {
                OverrideConsole = AutoCloseConsole || !ShowConsole || value;
                Set("ShowConsoleOnError", value);
            }
        }

        [JsonProperty(PropertyName = "javaArguments")]
        public string JavaArguments {
            get => Get<string>("JvmArgs");
            set {
                AreJavaArgumentsOverridden = !string.IsNullOrEmpty(value);
                Set("JvmArgs", value);
            }
        }

        [JsonProperty(PropertyName = "javaPath")]
        public string JavaPath {
            get => Get<string>("JavaPath");
            set {
                IsJavaLocationOverridden = value == null;
                Set("JavaPath", value);
            }
        }

        [JsonProperty(PropertyName = "maxMemoryAllocation")]
        public int MaxMemoryAllocation {
            get => Get<int>("MaxMemAlloc");
            set {
                IsMemoryAllocationOverridden = MaxMemoryAllocation > 0 || value > 0;
                Set("MaxMemAlloc", IsMemoryAllocationOverridden ? value : 0);
            }
        }

        [JsonProperty(PropertyName = "minMemoryAllocation")]
        public int MinMemoryAllocation {
            get => Get<int>("MinMemAlloc");
            set {
                IsMemoryAllocationOverridden = MaxMemoryAllocation > 0 || value > 0;
                Set("MinMemAlloc", IsMemoryAllocationOverridden ? value : 0);
            }
        }

        [JsonProperty(PropertyName = "launchMaximized")]
        public bool LaunchMaximized {
            get => Get<bool>("LaunchMaximized");
            set {
                AreWindowSettingsOverridden = value || WindowWidth > 0 || WindowHeight > 0;
                Set("LaunchMaximized", value);
            }
        }

        [JsonProperty(PropertyName = "windowHeight")]
        public int WindowHeight {
            get => Get<int>("MinecraftWinHeight");
            set {
                AreWindowSettingsOverridden = LaunchMaximized || WindowWidth > 0 || value > 0;
                Set("MinecraftWinHeight", IsMemoryAllocationOverridden ? value : 0);
            }
        }

        [JsonProperty(PropertyName = "windowWidth")]
        public int WindowWidth {
            get => Get<int>("MinecraftWinWidth");
            set {
                AreWindowSettingsOverridden = LaunchMaximized || WindowHeight > 0 || value > 0;
                Set("MinecraftWinWidth", IsMemoryAllocationOverridden ? value : 0);
            }
        }

        [JsonProperty(PropertyName = "notes")]
        public string Notes {
            get => Get<string>("notes");
            set => Set("notes", value);
        }

        #endregion

        #region Helping wrappers

        public bool OverrideCommands {
            get => Get<bool>("OverrideCommands");
            private set => Set("OverrideCommands", value);
        }

        public bool AreJavaArgumentsOverridden {
            get => Get<bool>("OverrideJavaArgs");
            private set => Set("OverrideJavaArgs", value);
        }

        public bool IsJavaLocationOverridden {
            get => Get<bool>("OverrideJavaLocation");
            private set => Set("OverrideJavaLocation", value);
        }

        public bool IsMemoryAllocationOverridden {
            get => Get<bool>("OverrideMemory");
            private set => Set("OverrideMemory", value);
        }

        public bool AreWindowSettingsOverridden {
            private set => Set("OverrideWindow", value);
            get => Get<bool>("OverrideWindow");
        }

        #endregion

        #region Properties

        public bool IsLoaded { private set; get; }

        #endregion

        public MmcConfigFile(string path) : base(Path.Combine(path, "instance.cfg")) {
        }

        public MmcConfigFile WithDefaults() {
            InstanceType = "OneSix";
            McLaunchMethod = "LauncherPart";
            ShowConsole = true;
            OverrideCommands = false;
            AreJavaArgumentsOverridden = false;
            IsJavaLocationOverridden = false;
            IsMemoryAllocationOverridden = false;
            AreWindowSettingsOverridden = false;
            IconName = "default";
            Notes = string.Empty;
            return this;
        }

        #region Getters & Setters

        public void SetField(object value, [CallerMemberName] string propertyName = "") {
            PropertyInfo prop = GetType()
                .GetProperties()
                .FirstOrDefault(p => p.Name == propertyName);

            if (prop == null) {
                throw new MissingFieldException($"There is no field with name {propertyName}!");
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
                throw new MissingFieldException($"There is no field with name {propertyName}!");
            }

            if (Exists && !IsLoaded) {
                IsLoaded = true;
                Load();
            }

            return (T)prop.GetValue(this);
        }

        #endregion

        #region IO

        public override void Save() {
            base.Save();
            IsLoaded = true;
        }

        #endregion

    }

}
