﻿using Newtonsoft.Json;
using Packsly.Core.Common.Configuration;
using Packsly.Core.Launcher;
using Packsly.Core.Adapter;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Packsly.Core.Modpack.Model {

    [JsonObject(MemberSerialization.OptIn)]
    public class ModpackInfo {

        #region Properties

        public string Id { internal set; get; }

        [JsonProperty("name")]
        public string Name { internal set; get; }

        [JsonProperty("icon")]
        public string Icon { internal set; get; }

        [JsonProperty("version", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Version { internal set; get; }

        [JsonProperty("minecraft")]
        public string MinecraftVersion { internal set; get; }

        [JsonProperty("mods")]
        public ModInfo[] Mods { internal set; get; }

        [JsonProperty("adapters", ItemTypeNameHandling = TypeNameHandling.All, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<IAdapterContext> Adapters { internal set; get; } = new List<IAdapterContext>();

        #endregion

        #region Constructors

        internal ModpackInfo() {
        }

        [JsonConstructor]
        private ModpackInfo(string name) {
            Id = name.ToLower();
            Name = name;
        }

        #endregion

        #region IO

        public void Save(string path) {
            byte[] buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this, Formatting.Indented));
            using(FileStream writer = File.Open(path, FileMode.Create))
                writer.Write(buffer, 0, buffer.Length);
        }

        #endregion

    }

}
