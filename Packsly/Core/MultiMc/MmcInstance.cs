using Core.MmcInstance;
using Packsly.Core.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Packsly.Core.MultiMc {

    public class MmcInstance : McInstanceTemplate {

        #region Public

        public override string Name {
            get {
                return ConfigFile.Get("name");
            }
        }

        public override string Location {
            get {
                return ConfigFile.Location;
            }
        }

        public override string MinecraftVersion {
            get {
                return ConfigFile.Get("IntendedVersion");
            }
        }

        public string Icon {
            set {
                Regex patten = new Regex(@"(?:\/(\w+)\.png)|(?:^(\w+)$)");

                if(patten.IsMatch(value)) {

                    string name = value;

                    if(Uri.IsWellFormedUriString(value, UriKind.Absolute)) {
                        name=  patten.Match(value).Groups[1].ToString();
                        string iconPath = GetIconPath(name);

                        if(!File.Exists(iconPath))
                            using(WebClient client = new WebClient())
                                client.DownloadFile(value, iconPath);
                    }

                    ConfigFile.Set("iconKey", name);

                } else throw new Exception($"'{value}' is not valid value for MultimcInstance icon");
            }
            get {
                return ConfigFile.Get("iconKey");
            }
        }

        public MmcConfigFile ConfigFile { private set; get; }

        #endregion

        #region Constructors

        public MmcInstance(string id, string mcversion) {
            ConfigFile = new MmcConfigFile(id, GetConfigPath(id), mcversion);
        }

        public MmcInstance(string id) {
            ConfigFile = new MmcConfigFile(GetConfigPath(id)).Load();
        }

        #endregion

        #region MinecraftInstance implementation

        public override void Save() {
            ConfigFile.Save();
        }

        public override void Delete() {
            Directory.Delete(Location, true);
        }

        #endregion

        #region Util

        private static string GetInstancePath(string id) {
            return Path.Combine(Config.Instance.MultiMC, "instances", id);
        }

        private static string GetConfigPath(string id) {
            return Path.Combine(GetInstancePath(id), "instance.cfg");
        }

        private static string GetIconPath(string name) {
            return Path.Combine(Config.Instance.MultiMC, "icons", name + ".png");
        }

        #endregion

    }

}
