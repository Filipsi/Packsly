using System.IO;

namespace Packsly3.Core.FileSystem {

    public abstract class FileBase {

        #region Properties

        public string FileName
            => File.Name;

        public string FilePath
            => File.FullName;

        public string DirectoryPath
            => Path.GetDirectoryName(FilePath);

        public bool Exists
            => File.Exists;

        #endregion

        protected readonly FileInfo File;

        protected FileBase(string path) {
            File = new FileInfo(path);
        }

        #region IO

        public abstract void Load();

        public abstract void Save();

        public void Delete() {
            if (File.Exists)
                File.Delete();
        }

        #endregion

    }

}
