using System.Collections.Generic;
using Packsly3.Core.Launcher.Instance;

namespace Packsly3.Core.Launcher.Modloader.Impl {

    public abstract class BasicModLoaderHandler<T> : IModLoaderHandler where T : IMinecraftInstance {

        #region IModLoaderHandler

        public bool IsCompatible(IMinecraftInstance instance)
            => instance.GetType() == typeof(T);

        public void DetectModLoaders(IMinecraftInstance instance, List<ModLoaderInfo> modLoaders)
            => DetectModLoaders((T)instance, modLoaders);

        public void Install(IMinecraftInstance instance, string modLoader, string version)
            => Install((T)instance, modLoader, version);

        public void Uninstall(IMinecraftInstance instance, string modLoader)
            => Uninstall((T)instance, modLoader);

        #endregion

        public abstract bool IsCompatible(string modLoader);

        public abstract void DetectModLoaders(T instance, List<ModLoaderInfo> modLoaders);

        public abstract void Install(T instance, string modLoader, string version);

        public abstract void Uninstall(T instance, string modLoader);

    }

}
