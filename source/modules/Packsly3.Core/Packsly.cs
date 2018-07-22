using System.Diagnostics;
using System.IO;
using System.Reflection;
using Packsly3.Core.FileSystem.Impl;
using Packsly3.Core.Launcher;
using Packsly3.Core.Launcher.Instance.Logic;

namespace Packsly3.Core {

    public static class Packsly {

        public static readonly DirectoryInfo AplicationDirectory = new DirectoryInfo(
            // ReSharper disable once AssignNullToNotNullAttribute
            Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)
        );

        public static readonly PackslyConfig Configuration = new PackslyConfig(AplicationDirectory);

        public static readonly MinecraftLauncher Launcher = new MinecraftLauncher();

        public static readonly Lifecycle Lifecycle = new Lifecycle();

    }

}
