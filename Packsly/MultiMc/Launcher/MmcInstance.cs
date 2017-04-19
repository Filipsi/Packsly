using Packsly.Launcher;
using Packsly.Core.Configuration;
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using Packsly.Core.Content;
using Packsly.Core.Module;

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
                return Path.Combine(LauncherLocation, "instances", Id);
            }
        }

        public string LauncherLocation {
            get {
                return Settings.Instance.Launcher;
            }
        }

        public string MinecraftVersion {
            get {
                return ConfigFile.Get("IntendedVersion");
            }
        }

        public string Icon {
            set {
                SetIcon(value);
            }
            get {
                return ConfigFile.Get("iconKey");
            }
        }

        private MmcConfigFile ConfigFile {
           set; get;
        }

        private string _id;

        #endregion

        #region Constructors

        public MmcInstance(string id) {
            _id = id;
            ConfigFile = new MmcConfigFile(Path.Combine(Location, "instance.cfg")).Load();
        }

        private MmcInstance() {
        }

        public static MmcInstance FromModpack(Modpack modpack) {
            MmcInstance instnace = new MmcInstance();
            instnace._id = modpack.Id;
            instnace.ConfigFile = new MmcConfigFile(modpack.Name, Path.Combine(instnace.Location, "instance.cfg"), modpack.MinecraftVersion);
            instnace.Icon = modpack.Icon;

            DirectoryInfo Temp = Settings.Instance.Temp;
            string mc = Path.Combine(instnace.Location, "minecraft");
            string mcMods = Path.Combine(mc, "mods");

            // Create the instnace
            instnace.Save();

            //Download mods
            foreach(Mod mod in modpack.Mods)
                mod.Download(mcMods);

            // Run modules
            foreach(IModuleArguments args in modpack.Modules)
                ModuleRegistry.Execute(instnace, args);

            // Apply overrides
            foreach(string file in modpack.OverrideFiles) {
                string destination = Path.Combine(mc, file.Replace(Settings.Instance.Temp.FullName + @"\", string.Empty));
                Directory.CreateDirectory(Path.GetDirectoryName(destination));
                File.Copy(Path.Combine(modpack.OverrideSource, file), destination);
            }

            if(Temp.Exists) Temp.Delete(true);
            return instnace;
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
                    string iconPath = Path.Combine(LauncherLocation, "icons", name + ".png");

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
