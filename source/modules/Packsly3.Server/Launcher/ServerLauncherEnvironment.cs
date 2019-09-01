using System;
using System.Collections.Generic;
using Packsly3.Core.Common.Register;
using Packsly3.Core.Launcher;
using Packsly3.Core.Launcher.Instance;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Packsly3.Core.Modpack;

namespace Packsly3.Server.Launcher {

    [Register]
    public class ServerLauncherEnvironment : ILauncherEnvironment {

        #region Properties

        public string Name { get; } = "server";

        public bool AllowEmbeding { get; } = false;

        #endregion

        #region Fields

        public static readonly Regex ServerJarNamePattern = new Regex(
            pattern: @"minecraft_server\.(\d+\.\d+\.\d+)\.jar",
            options: RegexOptions.Compiled
        );

        #endregion

        #region Logic

        public bool IsCompatible(DirectoryInfo workspace) {
            return workspace
                .EnumerateFiles()
                .Any(file => ServerJarNamePattern.IsMatch(file.Name));
        }

        public IMinecraftInstance CreateInstance(DirectoryInfo workspace, string id, ModpackDefinition modpack) {
            return new ServerMinecraftInstance(workspace);
        }

        public IMinecraftInstance GetInstance(DirectoryInfo workspace, string id) {
            return new ServerMinecraftInstance(workspace);
        }

        public IReadOnlyCollection<IMinecraftInstance> GetInstances(DirectoryInfo workspace) {
            ServerMinecraftInstance[] instnaces = {
                new ServerMinecraftInstance(workspace)
            };

            return Array.AsReadOnly(instnaces);
        }

        #endregion

    }

}
