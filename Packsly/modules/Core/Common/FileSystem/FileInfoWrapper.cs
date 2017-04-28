using System.IO;

namespace Packsly.Core.Common.FileSystem {

    public abstract class FileInfoWrapper {

        #region Properties

        public string Location {
            get {
                return _file.FullName;
            }
        }

        public string FileName {
            get {
                return _file.Name; 
            }
        }

        protected readonly FileInfo _file;

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
            if(_file.Exists)
                _file.Delete();
        }

        #endregion

    }

}
