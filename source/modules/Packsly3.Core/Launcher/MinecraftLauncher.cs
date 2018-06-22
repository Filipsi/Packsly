using System;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Reflection;
using Packsly3.Core.Common;
using Packsly3.Core.Launcher.Instance;

namespace Packsly3.Core.Launcher {

    public static class MinecraftLauncher {

        private static readonly ILauncherEnvironment[] Enviroments =
            RegisterAttribute.GetOccurrencesFor<ILauncherEnvironment>();

        private static readonly DirectoryInfo Root = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) ?? throw new InvalidOperationException());

        public static DirectoryInfo Workspace { get; set; } = Root;

        public static ILauncherEnvironment CurrentEnvironment
            => _currentEnviroment ?? (_currentEnviroment = GetCurrentEnvironment());

        private static ILauncherEnvironment _currentEnviroment;

        private static ILauncherEnvironment GetCurrentEnvironment() {
            if (Enviroments.Length == 0) {
                throw new InstanceNotFoundException("There are no enviroments registered.");
            }

            ILauncherEnvironment env = Enviroments.FirstOrDefault(e => e.IsCompatible(Workspace));
            if (env == null) {
                throw new Exception($"No compatible environment found in workspace '{Workspace.FullName}'!");
            }

            return env;
        }

        public static IMinecraftInstance[] GetInstances()
            => CurrentEnvironment.GetInstances(Workspace).ToArray();

        public static IMinecraftInstance GetInstance(string id)
            => CurrentEnvironment.GetInstance(Workspace, id);

        public static IMinecraftInstance CreateInstance(string id)
            => CurrentEnvironment.CreateInstance(Workspace, id);
    }

}
