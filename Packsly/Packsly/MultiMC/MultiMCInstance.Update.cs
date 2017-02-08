using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.MultiMC {

    internal partial class MultiMCInstance {

        private JObject _update;

        public bool IsUpToDate {
            get {
                return UpdateInfo.Value<string>("revision").Equals(Revision);
            }
        }

        private JObject UpdateInfo {
            get {
                if(_update != null)
                    return _update;

                string result;

                using(HttpClient client = new HttpClient())
                    result = client.GetStringAsync(new Uri(UpdateUrl + "/" + Name + ".json")).Result;

                return _update = JObject.Parse(result);
            }
        }

        public void Update() {
            if(!PackslyDirectory.Exists)
                throw new Exception("Cound not update modpack because Packsly's file describing MultiMC instance does not exit");

            bool changed = false;

            string uIcon = UpdateInfo.Value<string>("icon");
            if(Uri.IsWellFormedUriString(uIcon, UriKind.Relative))
                uIcon = UpdateUrl + uIcon;

            if(Icon != uIcon) {
                changed = true;
                Console.WriteLine(" - Updating modpack icon");
                DownloadIcon(uIcon);
            }

            if(ForgeVersion != UpdateInfo.Value<string>("forge")) {
                changed = true;
                Console.WriteLine(" - Updating Forge version");
                InstallForge();
                ForgeVersion = UpdateInfo.Value<string>("forge");
            }

            if(!IsUpToDate) {
                changed = true;
                Console.WriteLine(" - Updating mods from revision {0} to revision {1}", Revision, UpdateInfo.Value<string>("revision"));
                DownloadMods(true);
                Revision = UpdateInfo.Value<string>("revision");
            }

            if(changed) {
                CreateInstanceFile();
                Console.WriteLine("Update finished!");
            } else
                Console.WriteLine("Modpack is up to date");
        }

    }

}