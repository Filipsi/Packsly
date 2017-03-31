using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Configuration {

    public static partial class Config {

        public static Settings Current { get; private set; }

        private static FileInfo _file;

        public static void Load(string fileName = "config.json") {
            _file = new FileInfo(fileName);

            if(!_file.Exists) {
                Current = new Settings();
                Save();
            } else {
                using(StreamReader reader = _file.OpenText())
                    Current = JsonConvert.DeserializeObject<Settings>(reader.ReadToEnd());
            }
        }

        public static void Save() {
            using(StreamWriter writer = _file.CreateText())
                writer.Write(JsonConvert.SerializeObject(Current, Formatting.Indented));
        }

    }

}
