using Packsly.Core.Common.Registry;
using Packsly.Core.Launcher;
using System.Linq;

namespace Packsly.Core.Tweaker {

    public class AdapterRegistry : SingleTypeRegistry<Adapter> {

        public static void Execute(IMinecraftInstance instance, IExecutionContext context) {
            modules
                .Where(m => m.MinecraftInstaceType.Equals(instance.GetType()))
                .Where(m => m.ExecutionContextType.Equals(context.GetType()))
                .ToList()
                .ForEach(e => e.Execute(instance, context));
        }

    }

}
