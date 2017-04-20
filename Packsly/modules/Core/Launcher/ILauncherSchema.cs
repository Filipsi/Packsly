﻿using Packsly.Core.Modpack;
using System.IO;

namespace Packsly.Core.Launcher {

    public interface ILauncherSchema {

        string Name { get; }

        bool IsPresent(DirectoryInfo location);

        string[] GetInstances(DirectoryInfo location);

        ILauncherInstance Create(Modpack.ModpackInfo modpack);

    }

}