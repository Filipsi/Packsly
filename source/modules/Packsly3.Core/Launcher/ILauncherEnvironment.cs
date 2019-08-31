using System.IO;
using Packsly3.Core.Launcher.Instance;

namespace Packsly3.Core.Launcher {

    public interface ILauncherEnvironment {

        string Name { get; }

        bool AllowEmbeding { get; }

        bool IsCompatible(DirectoryInfo workspace);

        IMinecraftInstance[] GetInstances(DirectoryInfo workspace);

        IMinecraftInstance GetInstance(DirectoryInfo workspace, string id);

        IMinecraftInstance CreateInstance(DirectoryInfo workspace, string id);

    }

}
