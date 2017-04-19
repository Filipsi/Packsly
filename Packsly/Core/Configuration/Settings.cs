using Newtonsoft.Json;
using Packsly.Core.FileSystem;
using Packsly.Core.JsonConverters;
using System.IO;

namespace Packsly.Core.Configuration {

    public class Settings : JsonFile<Settings> {

        public static readonly Settings Instance = new Settings().Load();

        #region Settings

        [JsonProperty]
        public string Launcher { get; set; } = string.Empty;

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
