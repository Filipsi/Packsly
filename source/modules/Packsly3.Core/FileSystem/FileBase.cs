using System.IO;

namespace Packsly3.Core.FileSystem {

    public abstract class FileBase {

        #region Properties

        public string FileName
            => file.Name;

        public string FilePath
            => file.FullName;

        public string DirectoryPath
            => Path.GetDirectoryName(FilePath);

        public bool Exists
            => file.Exists;

        #endregion

        protected readonly FileInfo file;

        protected FileBase(string path) {
            file = new FileInfo(path);
        }

        #region IO

        public abstract void Load();

        public abstract void Save();

        public void Delete() {
            if (file.Exists) {
                file.Delete();
            }
        }

        #endregion

    }

}
