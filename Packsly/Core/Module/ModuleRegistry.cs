using Packsly.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Module {

    public class ModuleRegistry {

        private List<IModule> _modules;

        public ModuleRegistry(params IModule[] modules) {
            _modules = new List<IModule>(modules);
        }

        public ModuleRegistry Register(IModule module) {
            if(!_modules.Contains(module) || !_modules.Any(m => m.GetType().Equals(module.GetType())))
                _modules.Add(module);

            return this;
        }

        public void Execute(IMinecraftInstance instance, object args) {
            _modules
                .Where(m => m.InstanceType.Equals(instance.GetType()))
                .Where(m => m.ArgumentType.Equals(args.GetType()))
                .ToList()
                .ForEach(e => e.Execute(instance, args));
        }

    }

}
