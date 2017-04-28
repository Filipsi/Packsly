using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Common.FileSystem {
    
    public abstract class DataPairFile : FileInfoWrapper {

        #region Properties

        public bool IsDirty { private set; get; }

        protected Dictionary<string, string> _data { private set; get; }

        #endregion

        #region Constructor

        public DataPairFile(string path) : base(path) {
            _data = new Dictionary<string, string>();
        }

        #endregion

        #region Logic

        protected void MarkDirty() {
            IsDirty = true;
        }

        public bool HasKey(string key) {
            return _data.ContainsKey(key);
        }

        public string Get(string key) {
            if(HasKey(key))
                return _data[key];

            throw new Exception($"'{key}' key does not exist");
        }

        public void Set(string key, string value) {
            if(HasKey(key) && _data[key] != value)
                _data[key] = value;
            else 
                _data.Add(key, value);

            MarkDirty();
        }

        #endregion

    }

}
