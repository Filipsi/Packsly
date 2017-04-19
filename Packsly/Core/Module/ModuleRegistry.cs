using Packsly.Core.Launcher;
using System.Collections.Generic;
using System.Linq;

namespace Packsly.Core.Module {

    public static class ModuleRegistry {

        private static List<IModule> _modules = new List<IModule>();

        public static void Register(IModule module) {
            if(!_modules.Contains(module) || !_modules.Any(m => m.GetType().Equals(module.GetType())))
                _modules.Add(module);
        }

        public static void Execute(IMinecraftInstance instance, IModuleArguments args) {
            _modules
                .Where(m => m.MinecraftInstanceType.Equals(instance.GetType()))
                .Where(m => args.IsCompatible(m))
                .ToList()
                .ForEach(e => e.Execute(instance, args));
        }

    }

}
