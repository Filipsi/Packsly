using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mime;

namespace Packsly.Core.Modpack {

    [JsonObject(MemberSerialization.OptIn)]
    public class ModInfo {

        [JsonProperty("url")]
        public string Url { set; get; }

        [JsonProperty("filename", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string FileName { set; get; }

        public ModInfo() {
        }

        public ModInfo(string url, string filename = null) {
            Url = url;
            FileName = filename;
        }

        public ModInfo Download(string path) {
            string location = Url;

            do {
                HttpWebRequest client = WebRequest.Create(location) as HttpWebRequest;
                client.AllowAutoRedirect = false;
                client.UserAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Ubuntu Chromium/53.0.2785.143 Chrome/53.0.2785.143 Safari/537.36";

                try {
                    using(HttpWebResponse response = client.GetResponse() as HttpWebResponse) {
                        location = response.Headers["Location"];

                        if(location == null) {
                            if(FileName == null)
                                FileName = Path.GetFileName(Uri.UnescapeDataString(client.Address.AbsoluteUri));

                            string destination = Path.Combine(path, FileName);
                            Directory.CreateDirectory(Path.GetDirectoryName(destination));

                            Stream source = response.GetResponseStream();
                            using(var fileStream = new FileStream(destination, FileMode.Create, FileAccess.Write)) {
                                source.CopyTo(fileStream);
                            }

                        } else if(location.StartsWith("/"))
                            location = client.Address.Scheme + "://" + client.Host + location + client.Address.Fragment;
                    }
                } catch {
                    // TODO: Log this
                }
            } while(location != null);

            return this;
        }

    }

}
