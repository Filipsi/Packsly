﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Packsly3.Core.FileSystem {

    public abstract class DataPairFile : FileBase {

        #region Properties

        public bool IsDirty { private set; get; }

        #endregion

        private readonly Dictionary<string, string> _data;

        protected DataPairFile(string path) : base(path) {
            _data = new Dictionary<string, string>();
        }

        #region Helpers

        protected void MarkDirty() {
            IsDirty = true;
        }

        public bool HasKey(string key) {
            return _data.ContainsKey(key);
        }

        #endregion

        #region Getters & Setters

        protected T Get<T>(string key) {
            return HasKey(key) ?
                (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(_data[key])
                : default(T);
        }

        protected void Set(string key, object value) {
            if (value == null)
                return;

            string strValue = value.ToString();

            if (value is bool) {
                strValue = strValue.ToLower();
            }

            if (HasKey(key)) {
                if (_data[key] == strValue)
                    return;

                _data[key] = strValue;
            } else {
                _data.Add(key, strValue);
            }

            MarkDirty();
        }

        #endregion

        #region IO

        public override void Load() {
            if (!Exists) {
                return;
            }

            _data.Clear();

            using (StreamReader reader = File.OpenText())
                while (!reader.EndOfStream) {
                    string line = reader.ReadLine();
                    if (line == null)
                        throw new Exception("An error occurred while reading the file.");

                    if (!line.Contains('='))
                        throw new Exception("Error while reading data pair file. Equal sign is missing on line.");

                    string[] parts = line.Split(new[] { '=' }, 2);
                    _data.Add(parts[0], parts[1]);
                }
        }

        public override void Save() {
            if (!IsDirty)
                return;

            if (!Directory.Exists(DirectoryPath))
                Directory.CreateDirectory(DirectoryPath);

            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, string> entry in _data)
                builder.AppendLine($"{entry.Key}={entry.Value}");

            using (FileStream writer = File.Open(FileMode.Create)) {
                byte[] raw = Encoding.UTF8.GetBytes(builder.ToString());
                writer.Write(raw, 0, raw.Length);
            }
        }

        #endregion

    }

}
