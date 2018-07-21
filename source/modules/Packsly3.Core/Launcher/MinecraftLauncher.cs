using System;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using Packsly3.Core.Common.Register;
using Packsly3.Core.Launcher.Instance;

namespace Packsly3.Core.Launcher {

    public class MinecraftLauncher {

        private static readonly ILauncherEnvironment[] Enviroments = RegisterAttribute.GetOccurrencesFor<ILauncherEnvironment>();

        public DirectoryInfo Workspace { get; set; } = Packsly.AplicationDirectory;

        public string Name
            => CurrentEnvironment.Name;

        private ILauncherEnvironment CurrentEnvironment
            => _currentEnviroment ?? (_currentEnviroment = GetCurrentEnvironment());

        private ILauncherEnvironment _currentEnviroment;

        internal MinecraftLauncher() {
        }

        #region Wrapper methods

        private ILauncherEnvironment GetCurrentEnvironment() {
            if (Enviroments.Length == 0) {
                throw new InstanceNotFoundException("There are no environments registered.");
            }

            ILauncherEnvironment env = Enviroments.FirstOrDefault(e => e.IsCompatible(Workspace));
            if (env == null) {
                throw new Exception($"No compatible environment found in workspace '{Workspace.FullName}'!");
            }

            return env;
        }

        public IMinecraftInstance[] GetInstances()
            => CurrentEnvironment.GetInstances(Workspace).ToArray();

        public IMinecraftInstance GetInstance(string id)
            => CurrentEnvironment.GetInstance(Workspace, id);

        public IMinecraftInstance CreateInstance(string id)
            => CurrentEnvironment.CreateInstance(Workspace, id);

        public IMinecraftInstance CreateInstanceFromModpack(FileInfo modpackFile)
            => MinecraftInstanceFactory.CreateFromModpack(modpackFile);

        public IMinecraftInstance CreateInstanceFromModpack(Uri modpackFileUrl)
            => MinecraftInstanceFactory.CreateFromModpack(modpackFileUrl);

        public IMinecraftInstance CreateInstanceFromModpack(string instanceId, string modpackJson)
            => MinecraftInstanceFactory.CreateFromModpack(instanceId, modpackJson);

        #endregion

    }

}
