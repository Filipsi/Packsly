using HtmlAgilityPack;
using Packsly.Core.Common.Configuration;
using Packsly.Core.Modpack;
using Packsly.Core.Adapter.Forge;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using ICSharpCode.SharpZipLib.Zip;
using Packsly.Core.Modpack.Provider;
using Packsly.Core.Modpack.Model;
using Packsly.Core.Adapter.Override;
using Packsly.Core.Adapter.Update;

namespace Packsly.Curse.Content.Provider {

    public class CurseModpackProvider : IModpackProvider {

        private static Regex _patten = new Regex(@"(\w+:\/\/minecraft.curseforge.com)\/projects\/([^\/]+)\/files\/\d+$");

        private readonly DirectoryInfo Temp = Settings.Instance.Temp;

        #region IModpackProvider

        public bool CanUseSource(string source) {
            return Uri.IsWellFormedUriString(source, UriKind.Absolute) && _patten.IsMatch(source);
        }

        public ModpackInfo Create(string source) {
            // Get website
            HtmlDocument page = new HtmlDocument();
            using(WebClient client = new WebClient())
                page.LoadHtml(client.DownloadString(source));

            // Main node
            HtmlNode project = page.DocumentNode.SelectSingleNode("//div[contains(@class, 'project-user')]");

            // Obtain info
            Match pattenMatch = _patten.Match(source);
            string modpackId = pattenMatch.Groups[2].ToString();
            string relativeDownloadUrl = page.DocumentNode.SelectSingleNode("//div[contains(@class, 'project-file-download-button-large')]/a").GetAttributeValue("href", string.Empty);
           
            // Process modpack manifest
            DownloadModpack(pattenMatch.Groups[1] + relativeDownloadUrl, modpackId);
            CurseModpackManifestFile manifest = new CurseModpackManifestFile(Path.Combine(Temp.FullName, "manifest.json"));

            // Build modpack from manifest
            ModpackBuilder builder = ModpackBuilder
                .Create(
                    modpackId,
                    manifest.Name,
                    project.SelectSingleNode("//a[contains(@class, 'e-avatar64')]").GetAttributeValue("href", string.Empty),
                    manifest.MinecraftVersion)
                .SetVersion(manifest.Version)
                .AddMods(manifest.Mods);

            // Add forge if needed
            if(manifest.ForgeVersion != null)
                builder.AddForge(manifest.ForgeVersion);

            // Override files if needed 
            if(manifest.Overrides != null) {
                string overrideSource = Path.Combine(Temp.FullName, manifest.Overrides);
                string[] overrides = Directory.GetFiles(overrideSource, "*", SearchOption.AllDirectories);

                if(overrides.Length > 0)
                    builder.AddOverrides(overrideSource, overrides.Select(f => f.Replace(overrideSource + @"\", string.Empty)).ToArray());
            }

            // Add update adapter
            builder.AddAdapters(new UpdateAdapterContext(builder, source));

            // Build the modpack
            return builder.Build();
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

            FastZip zip = new FastZip();
            zip.ExtractZip(filepath, Temp.FullName, "");
            File.Delete(filepath);
        }

        #endregion

    }
}
