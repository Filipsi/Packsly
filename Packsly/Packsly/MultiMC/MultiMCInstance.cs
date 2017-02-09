using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Packsly.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.MultiMC {

    [JsonObject(MemberSerialization.OptIn)]
    internal partial class MultiMCInstance {

        [JsonProperty]
        public string Name { private set; get; }

        [JsonProperty]
        public string Icon {
            private set {
                if(Uri.IsWellFormedUriString(value, UriKind.Relative))
                    _icon = UpdateUrl + value;
                else
                    _icon = value;
            }
            get {
                return _icon;
            }
        }

        [JsonProperty]
        public string MinecraftVersion { private set; get; }

        [JsonProperty]
        public string ForgeVersion { private set; get; }

        [JsonProperty]
        public string Revision { private set; get; }

        [JsonProperty]
        public string UpdateUrl { private set; get; }

        public string           InstancePath        { private set; get; }
        public string           MinecraftPath       { private set; get; }
        public FileInfo         CfgFile             { private set; get; }
        public DirectoryInfo    PackslyDirectory    { private set; get; }

        private string _icon;

        [JsonConstructor]
        private MultiMCInstance(string path) {
            InstancePath = path;
            MinecraftPath = Path.Combine(InstancePath, "minecraft");
            CfgFile = new FileInfo(Path.Combine(InstancePath, "instance.cfg"));
            PackslyDirectory = new DirectoryInfo(Path.Combine(InstancePath, "../..", "packsly"));
        }

        public MultiMCInstance(string name, JObject modpackInfo, string updateUrl) {
            if(Config.Current.MultiMC == string.Empty || !Directory.Exists(Config.Current.MultiMC))
                throw new Exception("MultiMC directory is not specified or does not exists, use --set multimc [path] to specify working directory");

            _update = modpackInfo;

            Name = name;
            MinecraftVersion = modpackInfo.Value<string>("minecraft");
            ForgeVersion = modpackInfo.Value<string>("forge");
            Revision = modpackInfo.Value<string>("revision");
            UpdateUrl = updateUrl;

            InstancePath = Path.Combine(Config.Current.MultiMC, "instances", Name);
            MinecraftPath = Path.Combine(InstancePath, "minecraft");
            CfgFile = new FileInfo(Path.Combine(InstancePath, "instance.cfg"));
            PackslyDirectory = new DirectoryInfo(Path.Combine(Config.Current.MultiMC, "packsly"));

            Icon = modpackInfo.Value<string>("icon");
        }

        public void DownloadIcon(string newIcon) {
            string icon = Path.Combine(PackslyDirectory.FullName, "../icons", Name + ".png");

            if(File.Exists(icon))
                File.Delete(icon);

            using(WebClient client = new WebClient())
                client.DownloadFile(newIcon, icon);

            Icon = newIcon;
        }

        public string EnsureDirectory(string path) {
            if(!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }

        public static MultiMCInstance FromFile(string path) {
            if(!File.Exists(path))
                throw new Exception("Packsly's file describing MultiMC instance does not exit");

            string info;

            using(StreamReader reader = File.OpenText(path))
                info = reader.ReadToEnd();

            MultiMCInstance instance = new MultiMCInstance(Path.GetDirectoryName(path));
            JsonConvert.PopulateObject(info, instance);
            return instance;
        }

    }

}
