using Packsly.Core.Modpack.Model;
using Packsly.Core.Adapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Packsly.Core.Adapter.Forge;
using Packsly.Core.Adapter.Override;

namespace Packsly.Core.Modpack {

    public class ModpackBuilder {

        internal readonly ModpackInfo Instace = new ModpackInfo();
        private readonly List<ModInfo> _mods = new List<ModInfo>();

        private ModpackBuilder() {
        }

        public static ModpackBuilder Create(string id, string name, string icon, string mcVersion) {
            ModpackBuilder builder = new ModpackBuilder();
            builder.Instace.Id = id;
            builder.Instace.Name = name;
            builder.Instace.Icon = icon;
            builder.Instace.MinecraftVersion = mcVersion;
            return builder;
        }

        public static ModpackBuilder Create(string name, string icon, string mcVersion) {
            return Create(name.ToLower(), name, icon, mcVersion);
        }

        public ModpackBuilder SetVersion(string version) {
            Instace.Version = version;
            return this;
        }

        public ModpackBuilder AddMods(params ModInfo[] mods) {
            _mods.AddRange(mods);
            return this;
        }

        public ModpackBuilder AddMods(params string[] urls) {
            ModInfo[] mods = new ModInfo[urls.Length];
            for(int i = 0; i < mods.Length; i++)
                mods[i] = new ModInfo(urls[i]);

            _mods.AddRange(mods);
            return this;
        }

        public ModpackBuilder AddAdapters(params IAdapterContext[] adapters) {
            Instace.Adapters.AddRange(adapters);
            return this;
        }

        public ModpackBuilder AddForge(string version) {
            Instace.Adapters.Add(new ForgeAdapterContext(version));
            return this;
        }

        public ModpackBuilder AddOverrides(params string[] overrides) {
            Instace.Adapters.Add(new OverrideAdapterContext(overrides));
            return this;
        }

        public ModpackBuilder AddOverrides(string source, params string[] overrides) {
            Instace.Adapters.Add(new OverrideAdapterContext(source, overrides));
            return this;
        }

        public ModpackInfo Build() {
            Instace.Mods = _mods.ToArray();
            return Instace;
        }

    }

}
