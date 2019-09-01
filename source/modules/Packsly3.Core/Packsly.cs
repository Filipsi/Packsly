using System;
using System.IO;
using System.Net;
using System.Reflection;
using NLog;
using Packsly3.Core.FileSystem.Impl;
using Packsly3.Core.Launcher;
using Packsly3.Core.Launcher.Instance;

namespace Packsly3.Core {

    public static class Packsly {

        public static class Constants {

            public const string ForgeModloader = "forge";
            public const string LiteloaderModloader = "liteloader";
            public const string FabricModloader = "fabric";

        }

        public static bool IsLinux {
            get {
                int p = (int)Environment.OSVersion.Platform;
                return p == 4 || p == 6 || p == 128;
            }
        }

        public static readonly DirectoryInfo AplicationDirectory = new DirectoryInfo(
            Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? throw new InvalidOperationException()
        );

        public static readonly PackslyConfig Configuration = new PackslyConfig(AplicationDirectory);
        public static readonly MinecraftLauncher Launcher = new MinecraftLauncher();
        public static readonly Lifecycle Lifecycle = new Lifecycle();

        static Packsly() {
            Configuration.Load();
        }

    }

}
