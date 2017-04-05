using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.FileSystem {

    public abstract class FileTemplate<T> {

        #region Properties

        public string Location { private set; get; }

        protected FileInfo _file { private set; get; }

        #endregion

        #region Constructor

        public FileTemplate(string location) {
            Location = location;
            _file = new FileInfo(Location);
        }

        #endregion

        #region IO

        public abstract T Load();

        public abstract T Save();

        #endregion

    }
}
