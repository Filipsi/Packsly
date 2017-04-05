using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.FileSystem {

    [JsonObject(MemberSerialization.OptIn)]
    public abstract class JsonFile<T> : FileTemplate<T> {

        #region Constructor

        public JsonFile(string location) : base(location) {
        }

        #endregion

        #region IO

        public override T Load() {
            if(!_file.Exists) {
                Save();
                return (T)Convert.ChangeType(this, typeof(T));
            }

            using(StreamReader reader = _file.OpenText())
                JsonConvert.PopulateObject(reader.ReadToEnd(), this);

            return (T) Convert.ChangeType(this, typeof(T));
        }

        public override T Save() {
            using(StreamWriter writer = _file.CreateText())
                writer.Write(JsonConvert.SerializeObject(this, Formatting.Indented));

            return (T) Convert.ChangeType(this, typeof(T));
        }

        #endregion

    }

}
