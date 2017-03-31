using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Instance.MultiMC {

    public class MultimcConfigFile {

        public string Location { private set; get; }

        public bool isDirty { private set; get; }

        private FileInfo _file;
        private Dictionary<string, string> _data;

        public MultimcConfigFile(string location) {
            Location = location;
            _file = new FileInfo(Path.Combine(Location, "instance.cfg"));
            _data = new Dictionary<string, string>();

            if(!_file.Exists) {
                throw new Exception($"MultiMC instance configuration file does not exist at location {Location}");
            }
        }

        public MultimcConfigFile(string name, string location, string mcversion) {
            Location = location;
            _file = new FileInfo(Path.Combine(Location, "instance.cfg"));
            _data = new Dictionary<string, string>();

            Set("InstanceType", "OneSix");
            Set("IntendedVersion", mcversion);
            Set("OverrideCommands", "false");
            Set("OverrideConsole", "false");
            Set("OverrideJavaArgs", "false");
            Set("OverrideJavaLocation", "false");
            Set("OverrideMemory", "false");
            Set("OverrideWindow", "false");
            Set("iconKey", "default");
            Set("name", name);
            Set("notes", "");
        }

        #region Interaction

        public bool HasKey(string key) {
            return _data.ContainsKey(key);
        }

        public string Get(string key) {
            if(HasKey(key)) {
                return _data[key];
            }

            throw new Exception($"MultiMC instance configuration file at location {Location} does not have '{key}' key.");
        }

        public MultimcConfigFile Set(string key, string value) {
            if(HasKey(key) && _data[key] != value) {
                _data[key] = value;
                isDirty = true;
                return this;
            }

            _data.Add(key, value);
            isDirty = true;
            return this;
        }

        #endregion

        #region IO

        public MultimcConfigFile Load() {
            using(StreamReader reader = _file.OpenText()) {
                while(!reader.EndOfStream) {
                    string line = reader.ReadLine();
                    if(line.IndexOf('=') == -1) {
                        throw new Exception("Error while reading MultiMC instance configuration file. Equal sign is missing.");
                    }

                    string[] parts = line.Split(new char[] { '=' }, 2);
                    _data.Add(parts[0], parts[1]);
                }
            }

            return this;
        }

        public MultimcConfigFile Save() {
            if(!isDirty)
                return this;

            if(!Directory.Exists(Location))
                Directory.CreateDirectory(Location);

            StringBuilder builder = new StringBuilder();

            foreach(KeyValuePair<string, string> entry in _data)
                builder.AppendLine(string.Format("{0}={1}", entry.Key, entry.Value));

            using(FileStream writer = _file.Open(FileMode.Create)) {
                byte[] raw = Encoding.UTF8.GetBytes(builder.ToString());
                writer.Write(raw, 0, raw.Length);
            }

            return this;
        }

        #endregion

    }
}
