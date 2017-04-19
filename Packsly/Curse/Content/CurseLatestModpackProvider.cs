using HtmlAgilityPack;
using Packsly.Core.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Packsly.Curse.Content {

    public class CurseLatestModpackProvider : IModpackProvider {

        private static Regex _patten = new Regex(@"(\w+:\/\/minecraft.curseforge.com)\/projects\/(\w+)$");

        #region IModpackProvider

        public bool CanUseSource(string source) {
            return Uri.IsWellFormedUriString(source, UriKind.Absolute) && _patten.IsMatch(source);
        }

        public Modpack Create(string source) {
            HtmlDocument page = new HtmlDocument();

            using(WebClient client = new WebClient())
                page.LoadHtml(client.DownloadString(source + "/files"));

            string latest = page.DocumentNode
                .SelectSingleNode("//tr[contains(@class, 'project-file-list-item')]")
                .SelectSingleNode("//div[contains(@class, 'project-file-name-container')]/a[contains(@class, 'overflow-tip')]")
                .GetAttributeValue("href", null);

            latest = _patten.Match(source).Groups[1] + latest;

            return ModpackFactory.Acquire(latest);
        }

        #endregion

    }

}
