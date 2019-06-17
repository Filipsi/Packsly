using System.Collections.Generic;
using Packsly3.Core.Launcher.Instance;

namespace Packsly3.Core.Launcher.Modloader {

    public interface IModLoaderHandler {

        bool IsCompatible(string modLoader);

        bool IsCompatible(IMinecraftInstance instance);

        void DetectModLoaders(IMinecraftInstance instance, List<ModLoaderInfo> modLoaders);

        void Install(IMinecraftInstance instance, string modLoader, string version);

        void Uninstall(IMinecraftInstance instance, string modLoader);

    }

}
