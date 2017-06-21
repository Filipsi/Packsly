using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Common.FileSystem {

    [JsonObject(MemberSerialization.OptIn)]
    public abstract class JsonFile : FileInfoWrapper {

        #region Constructor

        public JsonFile(string path) : base(path) {
        }

        #endregion

        #region IO

        public override void Load() {
            if(File.Exists)
                using(StreamReader reader = File.OpenText())
                    JsonConvert.PopulateObject(reader.ReadToEnd(), this);
        }

        public override void Save() {
            using(StreamWriter writer = File.CreateText())
                writer.Write(JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        #endregion

    }

}
