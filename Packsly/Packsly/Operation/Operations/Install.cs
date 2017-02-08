using Newtonsoft.Json.Linq;
using Packsly.Configuration;
using Packsly.MultiMC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Operation.Operations {

    internal static partial class OperationCollection {

        public static void InstallFromUrl(Dictionary<string, string> args) {
            string url = args["url"];

            if(!url.Contains("http://www."))
                url = "http://www." + url;

            Install(url, args["packname"]);
        }

        public static void InstallFromSeek(Dictionary<string, string> args) {
            if(Config.Current.LastSeekUrl == string.Empty)
                throw new Exception("Failed to install modpack. There is no seek result stored. Use --seek command and try again");

            Install(Config.Current.LastSeekUrl, args["packname"]);
        }

        private static void Install(string url, string packname) {
            Uri address;

            if(!Uri.TryCreate(url + "/" + packname + ".json", UriKind.Absolute, out address))
                throw new Exception("Failed to install modpack. Url is not valid");

            Console.WriteLine(" - Seeking for modpack info at {0}", address.ToString());

            string result;

            using(HttpClient client = new HttpClient())
                result = client.GetStringAsync(address).Result;

            JObject modpack;

            try {
                modpack = JObject.Parse(result);
            } catch {
                throw new Exception("Failed to seek collection. Url does not contain modpack info");
            }

            Console.WriteLine("   > Name: {0}", packname);
            Console.WriteLine("   > Revision: {0}", modpack.Value<string>("revision"));
            Console.WriteLine("   > Mods: {0}", modpack.Value<JArray>("mods").Count);

            Console.WriteLine(" - Creating instance handler");
            MultiMCInstance instance = new MultiMCInstance(packname, modpack, url);

            instance.Install();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Installation complete!");
            Console.ResetColor();
        }

    }
}
