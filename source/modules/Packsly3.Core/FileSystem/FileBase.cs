using System.IO;

namespace Packsly3.Core.FileSystem {

    public abstract class FileBase {

        public string FileName      => ThisFile.Name;
        public string FilePath      => ThisFile.FullName;
        public string DirectoryPath => Path.GetDirectoryName(FilePath);
        public bool   Exists        => ThisFile.Exists;

        protected readonly FileInfo ThisFile;

        protected FileBase(string path) {
            ThisFile = new FileInfo(path);
        }

        public abstract void Load();

        public abstract void Save();

        public void Delete() {
            if (ThisFile.Exists)
                ThisFile.Delete();
        }

    }

}
