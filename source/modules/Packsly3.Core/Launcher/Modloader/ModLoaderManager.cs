using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using NLog;
using Packsly3.Core.Common.Register;
using Packsly3.Core.Launcher.Instance;

namespace Packsly3.Core.Launcher.Modloader {

    public class ModLoaderManager {

        #region Properties

        public static IModLoaderHandler[] InstalationSchemas { get; } = RegisterAttribute.GetOccurrencesFor<IModLoaderHandler>();

        public ReadOnlyCollection<ModLoaderInfo> ModLoaders { get; }

        #endregion

        #region Fields

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly List<ModLoaderInfo> modLoaders;
        private readonly IMinecraftInstance mcInstance;
        private readonly IModLoaderHandler[] compatibleSchemata;

        #endregion

        public ModLoaderManager(IMinecraftInstance mcInstnace) {
            mcInstance = mcInstnace;
            modLoaders = new List<ModLoaderInfo>();
            ModLoaders = modLoaders.AsReadOnly();

            compatibleSchemata = InstalationSchemas.Where(s => s.IsCompatible(mcInstnace)).ToArray();
            foreach (IModLoaderHandler handler in compatibleSchemata) {
                handler.DetectModLoaders(mcInstnace, modLoaders);
            }

            Logger.Debug($"Detected mod loaders for instance {mcInstnace.GetType()} with name {mcInstance.Name}: {JsonConvert.SerializeObject(ModLoaders, Formatting.Indented)}");
        }

        #region Logic

        public void Install(string name, string version) {
            IModLoaderHandler schema = compatibleSchemata.FirstOrDefault(s => s.IsCompatible(name));

            if (schema == null) {
                throw new InvalidOperationException($"No modloader installation schema compatible with modloader with name '{name}' for minecraft instance type '{mcInstance.GetType().FullName}' found!");
            }

            schema.Install(mcInstance, name, version);
            modLoaders.Add(new ModLoaderInfo(name, version));
        }

        public void Uninstall(string name) {
            ModLoaderInfo modLoaderInfo = ModLoaders.FirstOrDefault(ml => ml.Name == name);

            if (modLoaderInfo == null) {
                Logger.Warn($"This instance does not have any modloader with name '{name}'");
                return;
            }

            IModLoaderHandler schema = Array.Find(compatibleSchemata, s => s.IsCompatible(name));
            if (schema == null) {
                throw new InvalidOperationException($"No modloader installation schema compatible with modloader with name '{name}' for minecraft instance type '{mcInstance.GetType().FullName}' found!");
            }

            schema.Uninstall(mcInstance, name);
            modLoaders.Remove(modLoaderInfo);
        }

        #endregion

    }

}
