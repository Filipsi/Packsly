using Packsly.Core.Configuration;
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using Packsly.Core.Content;
using Packsly.Core.Module;
using Packsly.Core.Launcher;

namespace Packsly.MultiMc.Launcher {

    public class MmcInstance : IMinecraftInstance {

        #region Properties

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
                return Path.Combine(Settings.Instance.Launcher.FullName, "instances", Id);
            }
        }

        public string MinecraftVersion {
            get {
                return ConfigFile.Get("IntendedVersion");
            }
        }

        public string Icon {
            private set {
                SetIcon(value);
            }
            get {
                return ConfigFile.Get("iconKey");
            }
        }

        private readonly MmcConfigFile ConfigFile;

        private readonly string _id;

        #endregion

        #region Constructors

        public MmcInstance(string id) {
            _id = id;
            ConfigFile = new MmcConfigFile(Path.Combine(Location, "instance.cfg")).Load();
        }

        public MmcInstance(string id, string name, string icon, string mcversion) {
            _id = id;
            ConfigFile = new MmcConfigFile(name, Path.Combine(Location, "instance.cfg"), mcversion);
            Icon = icon;
        }

        #endregion

        #region Logic

        public void Save() {
            ConfigFile.Save();
        }

        public void Delete() {
            Directory.Delete(Location, true);
        }

        private void SetIcon(string value) {
            Regex patten = new Regex(@"(?:\/(\w+)\.png)|(?:^(\w+)$)");

            if(patten.IsMatch(value)) {
                string name = value;

                if(Uri.IsWellFormedUriString(value, UriKind.Absolute)) {
                    name = patten.Match(value).Groups[1].ToString();
                    string iconPath = Path.Combine(Settings.Instance.Launcher.FullName, "icons", name + ".png");

                    if(!File.Exists(iconPath))
                        using(WebClient client = new WebClient())
                            client.DownloadFile(value, iconPath);
                }

                ConfigFile.Set("iconKey", name);

            } else
                throw new Exception($"'{value}' is not valid value for MultimcInstance icon");
        }

        #endregion

    }

}
