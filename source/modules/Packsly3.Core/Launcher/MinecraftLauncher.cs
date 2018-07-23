using System;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using NLog;
using Packsly3.Core.Common.Register;
using Packsly3.Core.Launcher.Instance;

namespace Packsly3.Core.Launcher {

    public class MinecraftLauncher {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly ILauncherEnvironment[] Enviroments = RegisterAttribute.GetOccurrencesFor<ILauncherEnvironment>();

        public DirectoryInfo Workspace {
            get => _workspace;
            set {
                _workspace = value;
                Logger.Debug($"Launcher workspace was set to {_workspace.FullName}");
            }
        }

        public string Name
            => CurrentEnvironment.Name;

        private ILauncherEnvironment CurrentEnvironment {
            get {
                if (_currentEnviroment != null)
                    return _currentEnviroment;

                _currentEnviroment = GetCurrentEnvironment();
                Logger.Debug($"Launcher environment was set to {_currentEnviroment} by auto-detect.");
                return _currentEnviroment;
            }
        }

        private ILauncherEnvironment _currentEnviroment;
        private DirectoryInfo _workspace = Packsly.Configuration.Workspace;

        internal MinecraftLauncher() {
        }

        public void ForceEnviromentUsage(string name) {
            ILauncherEnvironment environment = Enviroments.FirstOrDefault((env) => env.Name == name);
            _currentEnviroment = environment ?? throw new Exception($"Environment with name '{name}' does not exist.");
            Logger.Debug($"Environment was forcefully changed to {_currentEnviroment}");
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

        public IMinecraftInstance CreateInstance(string id) {
            Logger.Debug($"Launcher using {_currentEnviroment} environment is creating new minecraft instance with id {id}");
            return CurrentEnvironment.CreateInstance(Workspace, id);
        }

        public IMinecraftInstance CreateInstanceFromModpack(FileInfo modpackFile)
            => MinecraftInstanceFactory.CreateFromModpack(modpackFile);

        public IMinecraftInstance CreateInstanceFromModpack(Uri modpackFileUrl)
            => MinecraftInstanceFactory.CreateFromModpack(modpackFileUrl);

        public IMinecraftInstance CreateInstanceFromModpack(string instanceId, string modpackJson)
            => MinecraftInstanceFactory.CreateFromModpack(instanceId, modpackJson);

        #endregion

    }

}
