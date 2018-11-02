using Packsly3.Core.Common.Register;
using Packsly3.Core.FileSystem.Impl;
using Packsly3.Core.Launcher;
using Packsly3.Core.Launcher.Instance;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly3.Server.Launcher {

    [Register]
    public class ServerLauncherEnvironment : ILauncherEnvironment {

        public string Name { get; } = "server";

        public bool IsCompatible(DirectoryInfo workspace) {
            return false;
        }

        public IMinecraftInstance CreateInstance(DirectoryInfo workspace, string id) {
            // TODO: Finish this
            //       Download forge, run installer, etc..

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
