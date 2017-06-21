using System.IO;

namespace Packsly.Core.Common.FileSystem {

    public abstract class FileInfoWrapper {

        #region Properties

        public string Location {
            get {
                return File.FullName;
            }
        }

        public string FileName {
            get {
                return File.Name; 
            }
        }

        public FileInfo File {
            get {
                return _file;
            }
        }

        protected FileInfo _file;

        #endregion

        #region Constructor

        public FileInfoWrapper(string path) {
            _file = new FileInfo(path);
        }

        #endregion

        #region IO

        public abstract void Load();

        public abstract void Save();

        public void Delete() {
            if(File.Exists)
                File.Delete();
        }

        #endregion

    }

}
