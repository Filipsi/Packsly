using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Packsly.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Operation.Operations {

    internal static partial class OperationCollection {

        public static void Seek(Dictionary<string, string> args) {
            string rawBaseUrl = args["url"];

            if(!rawBaseUrl.Contains("http://www."))
                rawBaseUrl = "http://www." + rawBaseUrl;

            Uri address;

            if(!Uri.TryCreate(rawBaseUrl + "/collection.json", UriKind.Absolute, out address))
                throw new Exception("Failed to seek collection. Url is not valid");

            Console.WriteLine(" - Seeking for collection at {0}", address.ToString());

            string result;

            using(HttpClient client = new HttpClient())
                result = client.GetStringAsync(address).Result;

            string[] packs;

            try {
                packs = JArray.Parse(result).Values().Select(e => e.Value<string>()).ToArray();
            } catch {
                throw new Exception("Failed to seek collection. Url does not contain collection");
            }

            Console.WriteLine(" - Found {0} modpacks", packs.Length);
            foreach(string pack in packs)
                Console.WriteLine("   > {0}", pack);

            Config.Current.LastSeekUrl = rawBaseUrl;
            Config.Save();

            Console.WriteLine("Use -install seek [packname] in order to intall modpack to MultiMC", packs.Length);
        }

    }

}
