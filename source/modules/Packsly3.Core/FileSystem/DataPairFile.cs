using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using NLog;

namespace Packsly3.Core.FileSystem {

    public abstract class DataPairFile : FileBase {

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #region Properties

        public bool IsDirty { private set; get; }

        #endregion

        #region Fields

        private readonly char[] delimiters;
        private readonly string commentSign;
        private readonly Dictionary<string, string> data;

        #endregion

        protected DataPairFile(string path, string commentSign, params char[] delimiters) : base(path) {
            this.delimiters = delimiters;
            this.commentSign = commentSign;
            data = new Dictionary<string, string>();
        }

        #region Helpers

        protected void MarkDirty() {
            IsDirty = true;
        }

        public bool HasKey(string key) {
            return data.ContainsKey(key);
        }

        #endregion

        #region Getters & Setters

        protected T Get<T>(string key) {
            return HasKey(key) ?
                (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(data[key])
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
                if (data[key] == strValue)
                    return;

                data[key] = strValue;
            } else {
                data.Add(key, strValue);
            }

            MarkDirty();
        }

        #endregion

        #region IO

        public override void Load() {
            if (!Exists) {
                return;
            }

            data.Clear();

            using (StreamReader reader = file.OpenText()) {
                int lineIndex = 0;

                while (!reader.EndOfStream) {
                    string line = reader.ReadLine();
                    lineIndex++;

                    // Skip empty lines
                    if (string.IsNullOrWhiteSpace(line)) {
                        continue;
                    }

                    // Skip comments
                    if (line.StartsWith(commentSign)) {
                        continue;
                    }

                    // Skip lines without delimiter
                    if (!line.Any(ch => delimiters.Contains(ch))) {
                        logger.Warn($"Unable to parse line {lineIndex} since it does not have any valid delimiter '{string.Join(", ", delimiters)}'. Skipping.");
                        continue;
                    }

                    string[] parts = line.Split(delimiters, 2);
                    data.Add(parts[0], parts[1]);
                }
            }

            logger.Debug($"Loaded data pair file '{file.Name}' with content {data}");
        }

        public override void Save() {
            if (!IsDirty)
                return;

            if (!Directory.Exists(DirectoryPath))
                Directory.CreateDirectory(DirectoryPath);

            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, string> entry in data)
                builder.AppendLine($"{entry.Key}={entry.Value}");

            using (FileStream writer = file.Open(FileMode.Create)) {
                byte[] raw = Encoding.UTF8.GetBytes(builder.ToString());
                writer.Write(raw, 0, raw.Length);
            }

            logger.Debug($"Saved data pair file '{file.Name}' with content {builder}");
        }

        #endregion

    }

}
