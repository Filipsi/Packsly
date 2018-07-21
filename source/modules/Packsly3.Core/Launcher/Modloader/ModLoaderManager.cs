using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Packsly3.Core.Common.Register;
using Packsly3.Core.Launcher.Instance;

namespace Packsly3.Core.Launcher.Modloader {

    public class ModLoaderManager {

        public static readonly IModLoaderHandler[] InstalationSchemas =
            RegisterAttribute.GetOccurrencesFor<IModLoaderHandler>();

        public readonly ReadOnlyCollection<ModLoaderInfo> ModLoaders;

        private readonly List<ModLoaderInfo> _modLoaders;
        private readonly IMinecraftInstance _mcInstance;
        private readonly IModLoaderHandler[] _compatibleSchemata;

        public ModLoaderManager(IMinecraftInstance mcInstnace) {
            _mcInstance = mcInstnace;
            _modLoaders = new List<ModLoaderInfo>();
            ModLoaders = _modLoaders.AsReadOnly();

            _compatibleSchemata = InstalationSchemas.Where(s => s.IsCompatible(mcInstnace)).ToArray();
            foreach (IModLoaderHandler handler in _compatibleSchemata) {
                handler.DetectModLoaders(mcInstnace, _modLoaders);
            }
        }

        public void Install(string name, string version) {
            IModLoaderHandler schema = _compatibleSchemata.FirstOrDefault(s => s.IsCompatible(name));

            if (schema == null) {
                throw new InvalidOperationException($"No modloader installation schema compatible with modloader with name '{name}' for minecraft instance type '{_mcInstance.GetType().FullName}' found!");
            }

            schema.Install(_mcInstance, name, version);
            _modLoaders.Add(new ModLoaderInfo(this, name, version));
        }

        public void Uninstall(string name) {
            ModLoaderInfo modLoaderInfo = ModLoaders.FirstOrDefault(ml => ml.Name == name);

            if (modLoaderInfo == null) {
                throw new KeyNotFoundException($"This instance does not have any modloader with name '{name}'.");
            }

            IModLoaderHandler schema = _compatibleSchemata.FirstOrDefault(s => s.IsCompatible(name));
            if (schema == null) {
                throw new InvalidOperationException($"No modloader installation schema compatible with modloader with name '{name}' for minecraft instance type '{_mcInstance.GetType().FullName}' found!");
            }

            schema.Uninstall(_mcInstance, name);
            _modLoaders.Remove(modLoaderInfo);
        }

    }

}
