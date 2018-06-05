using System;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using Packsly3.Core.Common;
using Packsly3.Core.Launcher.Instance;

namespace Packsly3.Core.Launcher {

    public static class Launcher {

        private static readonly ILauncherEnvironment[] Enviroments =
            RegisterAttribute.GetOccurrencesFor<ILauncherEnvironment>();

        private static readonly DirectoryInfo Root = new DirectoryInfo(Directory.GetCurrentDirectory());

        public static DirectoryInfo Workspace { get; set; } = Root;

        public static ILauncherEnvironment Current
            => _currentEnviroment ?? (_currentEnviroment = GetCurrentEnvironment());

        private static ILauncherEnvironment _currentEnviroment;

        private static ILauncherEnvironment GetCurrentEnvironment() {
            if (Enviroments.Length == 0) {
                throw new InstanceNotFoundException("There are no enviroments registered.");
            }

            ILauncherEnvironment env = Enviroments.FirstOrDefault(e => e.IsCompatible(Workspace));
            if (env == null) {
                throw new Exception($"No compatible enviromnet found in workspace '{Workspace.FullName}'!");
            }

            return env;
        }

        public static IMinecraftInstance[] GetInstances()
            => Current.GetInstances(Workspace).ToArray();

        public static IMinecraftInstance GetInstance(string id)
            => Current.GetInstance(Workspace, id);

        public static IMinecraftInstance CreateInstance(string id)
            => Current.CreateInstance(Workspace, id);
    }

}
