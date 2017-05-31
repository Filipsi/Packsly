using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Packsly.Core.Modpack;
using Packsly.Core.Common.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using Packsly.Core.Modpack.Model;

namespace Packsly.Curse.Content {

    public class CurseModpackManifestFile : JsonFile {

        #region Properties

        [JsonProperty("name")]
        public string Name { private set; get; }

        [JsonProperty("version")]
        public string Version { private set; get; }

        [JsonProperty("author")]
        public string Author { private set; get; }

        [JsonProperty("minecraft")]
        public string MinecraftVersion { private set; get; }

        [JsonProperty("forge")]
        public string ForgeVersion { private set; get; }

        [JsonProperty("files")]
        public string[] Files { private set; get; }

        [JsonProperty("overrides")]
        public string Overrides { private set; get; }

        #endregion

        #region Constructors

        public CurseModpackManifestFile(string location) : base(location) {
            Load();
        }

        #endregion

        #region JsonFile

        public override void Load() {
            string raw = string.Empty;

            using(StreamReader reader = _file.OpenText())
                raw = reader.ReadToEnd();

            JObject root = JObject.Parse(raw);
            string type = root.Value<string>("manifestType");

            if(!type.Equals("minecraftModpack"))
                throw new Exception($"Unexpected manifest type. 'minecraftModpack' was expected, got {type}");

            sbyte version = root.Value<sbyte>("manifestVersion");

            switch(version) {
                case 1: ParseVersion1(root); break;
                default: throw new Exception($"Unexpected manifest version {version}");
            }
        }

        public override void Save() {
            throw new NotImplementedException();
        }

        #endregion

        #region Versions

        private void ParseVersion1(JObject root) {
            Name = root.Value<string>("name");
            Version = root.Value<string>("version");
            Author = root.Value<string>("author");
            MinecraftVersion = root.GetValue("minecraft").Value<string>("version");

            string flag = "forge";
            foreach(JObject entry in root.GetValue("minecraft").Value<JArray>("modLoaders")) {
                string id = entry.Value<string>("id");
                if(id.Contains(flag)) {
                    ForgeVersion = id.Replace(flag, MinecraftVersion);
                    break;
                }
            }

            List<string> files = new List<string>();
            foreach(JObject entry in root.Value<JArray>("files"))
                files.Add($"projects/{entry.Value<uint>("projectID")}/files/{entry.Value<uint>("fileID")}");
            Files = files.ToArray();

            Overrides = root.Value<string>("overrides");
        }

        #endregion

        #region Utils

        public ModInfo[] BuildModInfo() {
            ModInfo[] mods = new ModInfo[Files.Length];

            for(ushort i = 0; i < mods.Length; i++)
                mods[i] = new ModInfo(
                    string.Format("http://minecraft.curseforge.com/{0}/download", Files[i])
                );

            return mods;
        }

        #endregion
    }

}
