using System;
using System.IO;
using System.Net;

namespace Packsly3.Core.Launcher.Instance.Logic {

    public class Icon {

        public string Source {
            get => _source;
            set {
                bool isUri = Uri.IsWellFormedUriString(value, UriKind.Absolute);
                _source = isUri
                    ? Path.GetFileNameWithoutExtension(value)
                    : value;

                Location = new FileInfo(Path.Combine(_iconFolder.FullName, $"{Source}.png"));

                if (!Location.Exists && isUri) {
                    Download(value);
                }

                IconChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public FileInfo Location { private set; get; }

        public event EventHandler IconChanged;

        private readonly DirectoryInfo _iconFolder;
        private string _source;

        public Icon(string iconFolder, string iconSource) {
            _iconFolder = new DirectoryInfo(iconFolder);
            Source = iconSource;
        }

        private void Download(string url) {
            if (!_iconFolder.Exists) {
                _iconFolder.Create();
            }

            using (WebClient clinet = new WebClient())
                clinet.DownloadFile(url, Location.FullName);
        }

    }

}
