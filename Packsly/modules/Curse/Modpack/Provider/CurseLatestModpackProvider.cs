using HtmlAgilityPack;
using Packsly.Core.Common;
using Packsly.Core.Modpack;
using Packsly.Core.Modpack.Model;
using Packsly.Core.Modpack.Provider;
using System;
using System.Net;
using System.Text.RegularExpressions;

namespace Packsly.Curse.Content.Provider {

    public class LatestCurseModpackProvider : IModpackProvider {

        private static Regex _patten = new Regex(@"(\w+:\/\/minecraft.curseforge.com)\/projects\/([^\/]+)$");

        #region IModpackProvider

        public bool CanUseSource(string source) {
            return Uri.IsWellFormedUriString(source, UriKind.Absolute) && _patten.IsMatch(source);
        }

        public ModpackInfo Create(string source) {
            HtmlDocument page = new HtmlDocument();

            using(WebClient client = new WebClient())
                page.LoadHtml(client.DownloadString(source + "/files"));

            string latest = page.DocumentNode
                .SelectSingleNode("//tr[contains(@class, 'project-file-list-item')]")
                .SelectSingleNode("//div[contains(@class, 'project-file-name-container')]/a[contains(@class, 'overflow-tip')]")
                .GetAttributeValue("href", null);

            latest = _patten.Match(source).Groups[1] + latest;

            return PackslyFactory.Modpack.BuildFrom(latest);
        }

        #endregion

    }

}
