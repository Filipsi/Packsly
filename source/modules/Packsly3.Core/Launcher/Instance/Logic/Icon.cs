using System;
using System.IO;
using System.Net;

namespace Packsly3.Core.Launcher.Instance.Logic {

    public class Icon {

        #region Properties

        public string Source {
            get => source;
            set {
                bool isUri = Uri.IsWellFormedUriString(value, UriKind.Absolute);
                source = isUri
                    ? Path.GetFileNameWithoutExtension(value)
                    : value;

                IconFile = new FileInfo(
                    Path.Combine(iconFolder.FullName, $"{nameOverride ?? source}.png")
                );

                if (!IconFile.Exists && isUri) {
                    Download(value);
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

            using (WebClient clinet = new WebClient())
                clinet.DownloadFile(url, IconFile.FullName);
        }

        #endregion

    }

}
