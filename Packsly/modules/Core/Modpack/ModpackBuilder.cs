using Packsly.Core.Modpack.Model;
using Packsly.Core.Tweaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Modpack {

    public class ModpackBuilder {

        private ModpackInfo _intance = new ModpackInfo();
        private List<ModInfo> _mods = new List<ModInfo>();

        private ModpackBuilder() {
        }

        public static ModpackBuilder Create(string id, string name, string icon, string mcVersion) {
            ModpackBuilder builder = new ModpackBuilder();
            builder._intance.Id = id;
            builder._intance.Name = name;
            builder._intance.Icon = icon;
            builder._intance.MinecraftVersion = mcVersion;
            return builder;
        }

        public static ModpackBuilder Create(string name, string icon, string mcVersion) {
            return Create(name.ToLower(), name, icon, mcVersion);
        }

        public ModpackBuilder WithVersion(string version) {
            _intance.Version = version;
            return this;
        }

        public ModpackBuilder WithMods(params ModInfo[] mods) {
            _mods.AddRange(mods);
            return this;
        }

        public ModpackBuilder WithMods(params string[] urls) {
            ModInfo[] mods = new ModInfo[urls.Length];
            for(int i = 0; i < mods.Length; i++)
                mods[i] = new ModInfo(urls[i]);

            _mods.AddRange(mods);
            return this;
        }

        public ModpackBuilder WithAdapters(params IExecutionContext[] adapters) {
            _intance.Adapters.AddRange(adapters);
            return this;
        }

        public ModpackBuilder WithOverridesFrom(string source, params string[] files) {
            if(!string.IsNullOrEmpty(source))
                _intance.OverrideSource = source;

            _intance.OverrideFiles = files;
            return this;
        }

        public ModpackInfo Build() {
            _intance.Mods = _mods.ToArray();
            return _intance;
        }

    }

}
