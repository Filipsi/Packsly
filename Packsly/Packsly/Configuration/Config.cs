using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Configuration {

    public static partial class Config {

        public static readonly FileInfo ConfigFile = new FileInfo("config.json");

        public static Settings Current { get; private set; }

        public static void Load() {
            if(!ConfigFile.Exists) {
                Current = new Settings();
                Save();
            } else {
                using(StreamReader reader = ConfigFile.OpenText())
                    Current = JsonConvert.DeserializeObject<Settings>(reader.ReadToEnd());
            }
        }

        public static void Save() {
            using(StreamWriter writer = ConfigFile.CreateText())
                writer.Write(JsonConvert.SerializeObject(Current, Formatting.Indented));
        }

    }

}
