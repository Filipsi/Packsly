using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Packsly3.Core.Launcher.Instance;

namespace Packsly3.Core.Launcher {

    public interface ILauncherEnvironment {

        string Name { get; }

        bool IsCompatible(DirectoryInfo workspace);

        IMinecraftInstance[] GetInstances(DirectoryInfo workspace);

        IMinecraftInstance GetInstance(DirectoryInfo workspace, string id);

        IMinecraftInstance CreateInstance(DirectoryInfo workspace, string id);

    }

}
