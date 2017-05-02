using Packsly.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Packsly.Core.Modpack.Provider {

    public class JsonUrlModpackProvider : IModpackProvider {

        private static Regex _pattenPastebin = new Regex(@"\w+:\/\/pastebin.com\/.+");

        public bool CanUseSource(string source) {
            return Uri.IsWellFormedUriString(source, UriKind.Absolute) &&
                   (source.EndsWith(".json") || _pattenPastebin.IsMatch(source));
        }

        public ModpackInfo Create(string source) {
            string raw = string.Empty;

            using(WebClient client = new WebClient())
                raw = client.DownloadString(source);

            return PackslyFactory.Modpack.BuildFrom(raw);
        }

    }

}
