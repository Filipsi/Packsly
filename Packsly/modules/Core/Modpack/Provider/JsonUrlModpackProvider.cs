using Packsly.Core.Common;
using System;
using System.Net;
using System.Text.RegularExpressions;

namespace Packsly.Core.Modpack.Provider {

    public class PastebinModpackProvider : IModpackProvider {

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
