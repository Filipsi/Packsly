using Packsly.Core.Launcher;
using System.Collections.Generic;
using System.Linq;

namespace Packsly.Core.Tweak {

    public static class TweakRegistry {

        private static List<ITweak> _tweaker = new List<ITweak>();

        public static void Register(ITweak tweak) {
            if(!_tweaker.Contains(tweak) || !_tweaker.Any(m => m.GetType().Equals(tweak.GetType())))
                _tweaker.Add(tweak);
        }

        public static void Execute(IMinecraftInstance instance, ITweakArguments args) {
            _tweaker
                .Where(m => m.MinecraftInstanceType.Equals(instance.GetType()))
                .Where(m => args.IsCompatible(m))
                .ToList()
                .ForEach(e => e.Execute(instance, args));
        }

    }

}
