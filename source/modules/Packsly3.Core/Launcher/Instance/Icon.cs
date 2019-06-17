using System;
using System.IO;
using System.Net;
using NLog;

namespace Packsly3.Core.Launcher.Instance {

    public class Icon {

        #region Properties

        public string Source {
            get => source;
            set {
                bool isUri = Uri.IsWellFormedUriString(value, UriKind.Absolute);

                source = isUri
                    ? Path.GetFileNameWithoutExtension(value)
                    : value;

                if (string.IsNullOrEmpty(source)) {
                    return;
                }

                IconFile = new FileInfo(
                    Path.Combine(iconFolder.FullName, $"{nameOverride ?? source}.png")
                );

                if (!IconFile.Exists && isUri) {
                    Download(value);
                } else {
                    logger.Info($"Using local modpack icon '{source}'.");
                }

                IconChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public FileInfo IconFile {
            get;
            private set;
        }

        #endregion

        #region Events

        public event EventHandler IconChanged;

        #endregion

        #region Fields

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly DirectoryInfo iconFolder;
        private readonly string nameOverride;
        private string source;

        #endregion

        public Icon(string iconFolder, string iconSource, string nameOverride = null) {
            this.iconFolder = new DirectoryInfo(iconFolder);
            this.nameOverride = nameOverride;
            Source = iconSource;
        }

        #region Logic

        private void Download(string url) {
            if (!iconFolder.Exists) {
                iconFolder.Create();
            }

            using (WebClient client = new WebClient()) {
                logger.Info($"Downloading modpack icon from '{url}'...");
                client.DownloadFile(url, IconFile.FullName);
            }
        }

        #endregion

    }

}
