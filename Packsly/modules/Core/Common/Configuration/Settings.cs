﻿using Newtonsoft.Json;
using Packsly.Core.Common.FileSystem;
using Packsly.Core.Common.JsonConverters;
using System.IO;

namespace Packsly.Core.Common.Configuration {

    public class Settings : JsonFile<Settings> {

        public static readonly Settings Instance = new Settings().Load();

        #region Settings

        [JsonProperty]
        [JsonConverter(typeof(DirectoryInfoConverter))]
        public DirectoryInfo Launcher { get; set; } = new DirectoryInfo("launcher");

        [JsonProperty]
        [JsonConverter(typeof(DirectoryInfoConverter))]
        public DirectoryInfo Temp { get; set; } = new DirectoryInfo("temp");

        [JsonProperty]
        [JsonConverter(typeof(DirectoryInfoConverter))]
        public DirectoryInfo Cache { get; set; } = new DirectoryInfo("cache");

        #endregion

        #region Constructor

        private Settings(string filename = "config.json") : base(filename) {
        }

        #endregion

    }

}