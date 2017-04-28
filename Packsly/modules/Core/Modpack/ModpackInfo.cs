using Newtonsoft.Json;
using Packsly.Core.Common.Configuration;
using Packsly.Core.Launcher;
using Packsly.Core.Tweaker;
using System.Collections.Generic;
using System.IO;

namespace Packsly.Core.Modpack {

    [JsonObject(MemberSerialization.OptIn)]
    public class ModpackInfo {

        #region Properties

        public string Id { private set; get; }

        [JsonProperty("name")]
        public string Name { private set; get; }

        [JsonProperty("icon")]
        public string Icon { private set; get; }

        [JsonProperty("version")]
        public string Version { private set; get; }

        [JsonProperty("minecraft")]
        public string MinecraftVersion { private set; get; }

        [JsonProperty("mods")]
        public ModInfo[] Mods { private set; get; }

        [JsonProperty("tweaks", ItemTypeNameHandling = TypeNameHandling.All, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<IExecutionContext> Tweaks { private set; get; } = new List<IExecutionContext>();

        [JsonProperty("overrideSource", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string OverrideSource { private set; get; } = string.Empty;

        [JsonProperty("overrides", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] OverrideFiles { private set; get; }

        #endregion

        #region Constructors

        public ModpackInfo(string id, string name, string icon, string mcVersion, string modpackVersion, params ModInfo[] mods) {
            Id = id;
            Name = name;
            Icon = icon;
            MinecraftVersion = mcVersion;
            Version = modpackVersion;
            Mods = mods;
        }

        [JsonConstructor]
        private ModpackInfo(string name) {
            Id = name.ToLower();
            Name = name;
        }

        #endregion

        #region Logic

        public ModpackInfo AddOverrides(string source, params string[] files) {
            OverrideSource = source;
            OverrideFiles = files;
            return this;
        }

        public ModpackInfo AddTweaks(params IExecutionContext[] tweaks) {
            Tweaks.AddRange(tweaks);
            return this;
        }

        public ModpackInfo ExecuteTweaks(IMinecraftInstance instance) {
            foreach(IExecutionContext args in Tweaks)
                AdapterRegistry.Execute(instance, args);

            return this;
        }

        public ModpackInfo DownloadMods(string destination) {
            foreach(ModInfo mod in Mods)
                mod.Download(destination);

            return this;
        }

        public ModpackInfo ApplyOverrides(string path) {
            if(OverrideFiles == null)
                return this;

            foreach(string file in OverrideFiles) {
                string destination = Path.Combine(path, file.Replace(Settings.Instance.Temp.FullName + @"\", string.Empty));
                Directory.CreateDirectory(Path.GetDirectoryName(destination));
                File.Copy(Path.Combine(OverrideSource, file), destination);
            }

            return this;
        }

        #endregion

    }

}
