using Packsly3.Core.Common.Register;
using Packsly3.Core.FileSystem.Impl;
using Packsly3.Core.Launcher;
using Packsly3.Core.Launcher.Instance;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Packsly3.Server.Launcher {

    [Register]
    public class ServerLauncherEnvironment : ILauncherEnvironment {

        public string Name { get; } = "server";

        public static readonly Regex ServerJarNamePattern = new Regex(
            pattern: @"minecraft_server\.(\d+\.\d+\.\d+)\.jar",
            options: RegexOptions.Compiled
        );

        public bool IsCompatible(DirectoryInfo workspace)
            => workspace
                .EnumerateFiles()
                .Where(file => ServerJarNamePattern.IsMatch(file.Name))
                .Any();

        public IMinecraftInstance CreateInstance(DirectoryInfo workspace, string id) {
            return new ServerMinecraftInstance(workspace);
        }

        public IMinecraftInstance GetInstance(DirectoryInfo workspace, string id) {
            return new ServerMinecraftInstance(workspace);
        }

        public IMinecraftInstance[] GetInstances(DirectoryInfo workspace) {
            return new IMinecraftInstance[] {
                new ServerMinecraftInstance(workspace)
            };
        }

    }

}
