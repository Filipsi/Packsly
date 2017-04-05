using Packsly.Common;
using Packsly.Core.Configuration;
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Packsly.Core.MultiMc {

    public class MmcInstance : IMinecraftInstance {

        #region Properties

        private string _id;

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

        public MmcConfigFile ConfigFile {
            private set;
            get;
        }

        #endregion

        #region Constructors

        public MmcInstance(string id, string mcversion) {
            _id = id;
            ConfigFile = new MmcConfigFile(id, Path.Combine(Location, "instance.cfg"), mcversion);
        }

        public MmcInstance(string id) {
            _id = id;
            ConfigFile = new MmcConfigFile(Path.Combine(Location, "instance.cfg")).Load();
        }

        #endregion

        #region IMinecraftInstance implementation

        public string Id {
            get {
                return _id;
            }
        }

        public string Name {
            get {
                return ConfigFile.Get("name");
            }
        }

        public string Location {
            get {
                return Path.Combine(LauncherLocation, "instances", Id);
            }
        }

        public string LauncherLocation {
            get {
                return Settings.Instance.MultiMC;
            }
        }

        public string MinecraftVersion {
            get {
                return ConfigFile.Get("IntendedVersion");
            }
        }

        public void Save() {
            ConfigFile.Save();
        }

        public void Delete() {
            Directory.Delete(Location, true);
        }

        #endregion

        #region Util

        private static string GetIconPath(string name) {
            return Path.Combine(Settings.Instance.MultiMC, "icons", name + ".png");
        }

        #endregion

    }

}
