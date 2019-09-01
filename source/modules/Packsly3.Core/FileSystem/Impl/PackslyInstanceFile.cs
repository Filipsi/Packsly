using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Packsly3.Core.Launcher.Instance;

namespace Packsly3.Core.FileSystem.Impl {

    public partial class PackslyInstanceFile : JsonFile {

        #region Properties

        [JsonProperty("data")]
        internal Dictionary<string, Dictionary<string, object>> CustomData { private set; get; }

        [JsonProperty("adapters")]
        internal AdaptersConfig Adapters { private set; get; }

        [JsonProperty("files")]
        internal Dictionary<FileManager.GroupType, List<string>> ManagedFiles { private set; get; }

        #endregion

        public PackslyInstanceFile(string path) : base(Path.Combine(path, "instance.packsly")) {
            CustomData =  new Dictionary<string, Dictionary<string, object>>();
            Adapters = new AdaptersConfig();
            ManagedFiles = new Dictionary<FileManager.GroupType, List<string>>();

            FixFilenameTypo();
            Load();
        }


        #region Logic

        public override void SetDefaultValues() {
        }

        /// <summary>
        /// Checks and fixes filename typo in older versions of Packsly3
        /// </summary>
        private void FixFilenameTypo() {
            FileInfo typoFile = new FileInfo(Path.Combine(file.DirectoryName ?? throw new InvalidOperationException(), "instnace.packsly"));
            if (!typoFile.Exists) {
                return;
            }

            logger.Info($"Bad instance file detected, fixing naming typo by renaming '{typoFile.Name}' to 'instance.packsly'");
            typoFile.MoveTo(Path.Combine(file.DirectoryName, "instance.packsly"));
        }

        #endregion

        #region Custom data

        public T Get<T>(string group, string key) {
            if (!CustomData.ContainsKey(group)) {
                return default(T);
            }

            Dictionary<string, object> storage = CustomData[group];
            if (!storage.ContainsKey(key)) {
                return default(T);
            }

            return (T)storage[key];
        }

        public void Set<T>(string group, string key, T value) {
            if (!CustomData.ContainsKey(group)) {
                CustomData.Add(group, new Dictionary<string, object>());
            }

            Dictionary<string, object> storage = CustomData[group];
            if (storage.ContainsKey(key)) {
                storage.Add(key, value);
            } else {
                storage[key] = value;
            }
        }

        public void Remove(string group, string key) {
            if (!CustomData.ContainsKey(group)) {
                return;
            }

            Dictionary<string, object> storage = CustomData[group];
            if (!storage.ContainsKey(key)) {
                return;
            }

            storage.Remove(key);
        }

        #endregion

        #region IO

        public sealed override void Load()
            => base.Load();

        #endregion

    }

}
