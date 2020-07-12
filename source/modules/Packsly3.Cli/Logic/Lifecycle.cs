using NLog;
using Packsly3.Cli.Common;
using Packsly3.Cli.Verbs;
using Packsly3.Core;
using Packsly3.Core.Launcher.Instance;
using System.IO;

namespace Packsly3.Cli.Logic {

    internal static class Lifecycle {

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static void Publish(LifecycleOptions options) {
            Logo.Print();
            ApplySettings(options);

            IMinecraftInstance instance = Packsly.Launcher.GetInstance(options.InstanceId);
            instance.Load();
            logger.Debug($"Dispatching lifecycle events specified by command line arguments: {string.Join(", ", options.Events)}");
            Packsly.Lifecycle.EventBus.Publish(instance, options.Events);
            instance.Save();
        }

        private static void ApplySettings(LifecycleOptions options) {
            if (options.IsWorkspaceSpecified) {
                if (options.IsWorkspaceValid) {
                    Packsly.Launcher.Workspace = new DirectoryInfo(options.Workspace);
                    logger.Info($"Workspace was set to: {Packsly.Launcher.Workspace}");

                } else {
                    throw new DirectoryNotFoundException($"Specified '{options.Workspace}' workspace folder does not exist.");
                }
            } else {
                DirectoryInfo workspaceFolder = new DirectoryInfo(Packsly.Configuration.Workspace);
                if (workspaceFolder.Exists) {
                    Packsly.Launcher.Workspace = workspaceFolder;
                    options.Workspace = Packsly.Launcher.Workspace.FullName;
                    logger.Info($"Using workspace from configuration file: {Packsly.Launcher.Workspace}");
                } else {
                    throw new DirectoryNotFoundException($"Workspace '{workspaceFolder.FullName}' specified in configuration file does not exists.");
                }
            }

            if (options.IsEnvironmentSpecified) {
                Packsly.Launcher.ForceEnviromentUsage(options.Environment);
                logger.Info("Overriding environment auto detection...");
                logger.Info($"Current environment name: {Packsly.Launcher.Name}");

            } else {
                logger.Info($"Current environment name: {Packsly.Launcher.Name}");
            }
        }

    }

}
