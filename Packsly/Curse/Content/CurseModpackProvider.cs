using HtmlAgilityPack;
using Ionic.Zip;
using Packsly.Core.Configuration;
using Packsly.Core.Content;
using Packsly.Core.Forge;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Packsly.Curse.Content {

    public class CurseModpackProvider : IModpackProvider {

        private static Regex _patten = new Regex(@"(\w+:\/\/minecraft.curseforge.com)\/projects\/(\w+)\/files\/\d+$");

        private readonly DirectoryInfo Temp = Settings.Instance.Temp;

        #region IModpackProvider

        public bool CanUseSource(string source) {
            return Uri.IsWellFormedUriString(source, UriKind.Absolute) && _patten.IsMatch(source);
        }

        public Modpack Create(string source) {
            HtmlDocument page = new HtmlDocument();

            using(WebClient client = new WebClient())
                page.LoadHtml(client.DownloadString(source));

            HtmlNode project = page.DocumentNode.SelectSingleNode("//div[contains(@class, 'project-user')]");

            Match pattenMatch = _patten.Match(source);
            string modpackId = pattenMatch.Groups[2].ToString();
            string relativeDownloadUrl = page.DocumentNode.SelectSingleNode("//div[contains(@class, 'project-file-download-button-large')]/a").GetAttributeValue("href", string.Empty);
           
            DownloadModpack(pattenMatch.Groups[1] + relativeDownloadUrl, modpackId);

            CurseModpackManifestFile manifest = new CurseModpackManifestFile(Path.Combine(Temp.FullName, "manifest.json")).Load();

            Modpack modpack = new Modpack(
                modpackId,
                manifest.Name,
                project.SelectSingleNode("//a[contains(@class, 'e-avatar64')]").GetAttributeValue("href", string.Empty),
                manifest.MinecraftVersion,
                manifest.GetMods()
            );

            if(manifest.ForgeVersion != null)
                modpack.AddModules(new ForgeModuleArgs(manifest.ForgeVersion));

            if(manifest.Overrides != null) {
                string overrideSource = Path.Combine(Temp.FullName, manifest.Overrides);
                string[] overrides = Directory.GetFiles(overrideSource, "*", SearchOption.AllDirectories);
                if(overrides.Length > 0) modpack.AddOverrides(overrideSource, overrides.Select(f => f.Replace(overrideSource + @"\", string.Empty)).ToArray());
            }

            return modpack;
        }

        #endregion

        #region Modpack

        private void DownloadModpack(string url, string filename) {
            string file = filename + ".zip";
            string filepath = Path.Combine(Temp.FullName, file);

            if(Temp.Exists)
                Temp.Delete(true);

            Temp.Create();

            using(WebClient client = new WebClient())
                client.DownloadFile(url, filepath);

            using(ZipFile zip = new ZipFile(filepath))
                zip.ExtractAll(Temp.FullName);

            File.Delete(filepath);
        }

        #endregion

    }
}
