using Core.Instance;
using Packsly.Core.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Instance {

    public class MultimcInstance : MinecraftInstance {

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

        public MultimcConfigFile ConfigFile { private set; get; }

        #endregion

        #region Constructors

        public MultimcInstance(string name, string mcversion) {
            string location = GetPath(name);

            if(Directory.Exists(location)) {
                throw new Exception($"MultiMC instance with name '{name}' already exists.");
            }

            ConfigFile = new MultimcConfigFile(name, location, mcversion);
        }

        private MultimcInstance(string location) {
            ConfigFile = new MultimcConfigFile(location).Load();
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

        #region Factory

        public static MultimcInstance FromExisting(string name) {
            return new MultimcInstance(GetPath(name));
        }

        #endregion

        #region Util

        private static string GetPath(string name) {
            return Path.Combine(Config.Current.MultiMC, "instances", name);
        }



        #endregion

    }

}
