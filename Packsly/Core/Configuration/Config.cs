using Newtonsoft.Json;
using Packsly.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Configuration {

    public class Config : JsonFile<Config> {

        public static readonly Config Instance = new Config().Load();

        #region Settings

        [JsonProperty]
        public string MultiMC { get; set; } = string.Empty;

        #endregion

        #region Constructor

        private Config(string filename = "config.json") : base(filename) {
        }

        #endregion

    }

}
