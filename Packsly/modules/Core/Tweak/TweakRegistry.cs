using Packsly.Core.Common.Registry;
using Packsly.Core.Launcher;
using System.Collections.Generic;
using System.Linq;

namespace Packsly.Core.Tweak {

    public class TweakRegistry : SingleTypeRegistry<ITweak> {

        public static void Execute(ILauncherInstance instance, ITweakArguments args) {
            modules
                .Where(m => m.MinecraftInstanceType.Equals(instance.GetType()))
                .Where(m => args.IsCompatible(m))
                .ToList()
                .ForEach(e => e.Execute(instance, args));
        }

    }

}
