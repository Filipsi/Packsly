using System;
using System.IO;
using System.Net;
using System.Reflection;
using Packsly3.Core.FileSystem.Impl;
using Packsly3.Core.Launcher;
using Packsly3.Core.Launcher.Instance.Logic;

namespace Packsly3.Core {

    public static class Packsly {

        public static class Constants {

            public const string ForgeModlaoder = "forge";
            public const string LiteloaderModlaoder = "liteloader";

        }

        public static bool IsLinux {
            get {
                int p = (int)Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }

        public static readonly DirectoryInfo AplicationDirectory = new DirectoryInfo(
            Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)
        );

        public static readonly PackslyConfig Configuration = new PackslyConfig(AplicationDirectory);
        public static readonly MinecraftLauncher Launcher = new MinecraftLauncher();
        public static readonly Lifecycle Lifecycle = new Lifecycle();

    }

}
