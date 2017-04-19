using Packsly.Core.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Launcher {

    public interface ILauncherSchema {

        string Name { get; }

        Type MinecraftInstanceType { get; }

        bool Check(DirectoryInfo location);

        string[] GetInstances(DirectoryInfo location);

        IMinecraftInstance Create(Modpack modpack);

    }

}
