using System.IO;
using NLog;
using Packsly3.Cli.Verbs;
using Packsly3.Core;

namespace Packsly3.Cli.Logic {

    internal static class DefaultInstaller {

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static void Run() {
            DirectoryInfo worksapceFolder = new DirectoryInfo(Packsly.Configuration.Workspace);

            if (worksapceFolder.Exists) {
                Packsly.Launcher.Workspace = worksapceFolder;
                logger.Info($"Workspace was set from configuration file to: {worksapceFolder.FullName}");
            }

            logger.Info($"Using modpack source from configuration: {Packsly.Configuration.DefaultModpackSource}");
            Installer.Run(new InstallOptions {
                Source = Packsly.Configuration.DefaultModpackSource,
                Workspace = Packsly.Launcher.Workspace.FullName
            });
        }

    }

}
