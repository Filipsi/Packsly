using Packsly.Core.Common.Factory;
using Packsly.Core.Launcher;
using Packsly.Core.Modpack;
using Packsly.Core.Tweak;

namespace Packsly.Core.Common.Registry {

    public static class PackslyRegistry {

        #region Registries

        private static readonly IRegistry<ITweak> _tweakRegistry = new TweakRegistry();

        #endregion

        #region Logic

        public static void Register(params ITweak[] elements) {
            _tweakRegistry.Register(elements);
        }

        public static void Register(params IModpackProvider[] elements) {
            PackslyFactory.Modpack.Register(elements);
        }

        public static void Register(params ILauncherSchema[] elements) {
            PackslyFactory.LauncherInstance.Register(elements);
        }

        #endregion

    }

}
