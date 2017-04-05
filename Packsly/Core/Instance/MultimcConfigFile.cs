﻿using Packsly.Core;
using Packsly.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Instance {

    public class MultimcConfigFile : DataPairFile<MultimcConfigFile> {

        #region Constructor

        public MultimcConfigFile(string location) : base(location) {
            if(!_file.Exists) {
                throw new Exception($"MultiMC instance configuration file does not exist at location {Location}");
            }
        }

        public MultimcConfigFile(string name, string location, string mcversion) : base(location) {
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

        #endregion

        #region IO

        public override MultimcConfigFile Load() {
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

        public override MultimcConfigFile Save() {
            if(!IsDirty)
                return this;

            string directoryPath = Path.GetDirectoryName(Location);
            if(!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

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