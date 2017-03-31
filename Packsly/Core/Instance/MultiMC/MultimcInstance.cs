using Core.Instance.MultiMC;
using Packsly.Core.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Instance.MultiMC {

    public class MultimcInstance : MinecraftInstance {

        public MultimcConfigFile ConfigFile { private set; get; }

        public static readonly string MULTIMC_LOCATION = "";

        public MultimcInstance(string name, string mcversion) : base(name, GetPath(name), mcversion) {
            ConfigFile = new MultimcConfigFile(name, Location, mcversion);
        }

        private MultimcInstance(MultimcConfigFile config) : base(config.Get("name"), config.Location, config.Get("IntendedVersion")) {
            ConfigFile = config;
        }

        public static string GetPath(string name) {
            return Path.Combine(Config.Current.MultiMC, "instances", name);
        }

        public static MultimcInstance FromName(string name) {
            return new MultimcInstance(new MultimcConfigFile(GetPath(name)).Load());
        }

    }

}
