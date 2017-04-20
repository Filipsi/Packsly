using Newtonsoft.Json;
using Packsly.Core.Common.Configuration;
using Packsly.Core.Launcher;
using Packsly.Core.Tweak;
using System;
using System.Collections.Generic;
using System.IO;

namespace Packsly.Core.Modpack {

    [JsonObject(MemberSerialization.OptIn)]
    public class Modpack {

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
        public Mod[] Mods { private set; get; }

        [JsonProperty("tweaks", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<ITweakArguments> Tweaks { private set; get; }

        [JsonProperty("overrideSource", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string OverrideSource { private set; get; }

        [JsonProperty("overrides", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] OverrideFiles { private set; get; }

        #endregion

        #region Constructors

        public Modpack(string id, string name, string icon, string mcversion, params Mod[] mods) {
            Id = id;
            Name = name;
            Icon = icon;
            MinecraftVersion = mcversion;
            Tweaks = new List<ITweakArguments>();
            Mods = mods;
        }

        #endregion

        #region Logic

        public Modpack AddOverrides(string source, params string[] files) {
            OverrideSource = source;
            OverrideFiles = files;
            return this;
        }

        public Modpack AddTweaks(params ITweakArguments[] tweaks) {
            Tweaks.AddRange(tweaks);
            return this;
        }

        public Modpack ExecuteTweaks(IMinecraftInstance instance) {
            foreach(ITweakArguments args in Tweaks)
                TweakRegistry.Execute(instance, args);

            return this;
        }

        public Modpack DownloadMods(string destination) {
            foreach(Mod mod in Mods)
                mod.Download(destination);

            return this;
        }

        public Modpack ApplyOverrides(string path) {
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
