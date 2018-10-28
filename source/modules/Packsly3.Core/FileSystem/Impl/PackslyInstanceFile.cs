﻿using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Packsly3.Core.Common.Json;
using Packsly3.Core.Launcher.Instance;
using Packsly3.Core.Launcher.Instance.Logic;

namespace Packsly3.Core.FileSystem.Impl {

    public partial class PackslyInstanceFile : JsonFile {

        #region Properties

        [JsonProperty("data")]
        internal Dictionary<IMinecraftInstance, Dictionary<string, object>> CustomData { private set; get; }

        [JsonProperty("adapters")]
        internal AdaptersConfig Adapters { private set; get; }

        [JsonProperty("files")]
        internal Dictionary<FileManager.GroupType, List<FileInfo>> ManagedFiles { private set; get; }

        #endregion

        public PackslyInstanceFile(string path) : base(Path.Combine(path, "instnace.packsly")) {
            CustomData =  new Dictionary<IMinecraftInstance, Dictionary<string, object>>();
            Adapters = new AdaptersConfig();
            ManagedFiles = new Dictionary<FileManager.GroupType, List<FileInfo>>();

            SerializerSettings = new JsonSerializerSettings {
                ContractResolver = new LowercaseContractResolver(),
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                Converters = {
                    new RelativePathConverter {
                        Root = DirectoryPath
                    }
                }
            };

            Load();
        }

        #region Custom data

        public T Get<T>(IMinecraftInstance owner, string key) {
            if (!CustomData.ContainsKey(owner)) {
                return default(T);
            }

            Dictionary<string, object> storage = CustomData[owner];
            if (!storage.ContainsKey(key)) {
                return default(T);
            }

            return (T)storage[key];
        }

        public void Set<T>(IMinecraftInstance owner, string key, T value) {
            if (!CustomData.ContainsKey(owner)) {
                CustomData.Add(owner, new Dictionary<string, object>());
            }

            Dictionary<string, object> storage = CustomData[owner];
            if (storage.ContainsKey(key)) {
                storage.Add(key, value);
            } else {
                storage[key] = value;
            }
        }

        public void Remove(IMinecraftInstance owner, string key) {
            if (!CustomData.ContainsKey(owner)) {
                return;
            }

            Dictionary<string, object> storage = CustomData[owner];
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
