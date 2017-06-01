using Packsly.Core.Common.Registry;
using Packsly.Core.Launcher;
using Packsly.Core.Modpack.Provider;
using Packsly.Core.Adapter;

namespace Packsly.Core.Common {

    public static class PackslyRegistry {

        #region Registries

        private static readonly IRegistry<Adapter.Adapter> _tweakRegistry = new AdapterRegistry();

        #endregion

        #region Logic

        public static void Register(params Adapter.Adapter[] elements) {
            _tweakRegistry.Register(elements);
        }

        public static void Register(params IModpackProvider[] elements) {
            PackslyFactory.Modpack.Register(elements);
        }

        public static void Register(params ILauncherSchema[] elements) {
            PackslyFactory.MinecraftInstance.Register(elements);
        }

        #endregion

    }

}
