using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.FileSystem {

    public abstract class FileBase<T> {

        #region Properties

        public string Location { private set; get; }

        protected FileInfo _file { private set; get; }

        #endregion

        #region Constructor

        public FileBase(string location) {
            Location = location;
            _file = new FileInfo(Location);
        }

        #endregion

        #region IO

        public abstract T Load();

        public abstract T Save();

        public T Delete() {
            if(_file.Exists) _file.Delete();
            return (T) Convert.ChangeType(this, typeof(T));
        }

        #endregion

    }
}
