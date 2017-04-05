﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Packsly.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Forge {

    public class ForgePatchFile : JsonFile<ForgePatchFile> {

        #region Properties

        [JsonProperty("+libraries")]
        public ForgeLibrary[] Libraries { private set; get; }

        [JsonProperty("+tweakers")]
        public string[] Tweakers { private set; get; }

        [JsonProperty("fileId")]
        public readonly string FileId = "net.minecraftforge";

        [JsonProperty("mainClass")]
        public readonly string MainClass = "net.minecraft.launchwrapper.Launch";

        [JsonProperty("mcVersion")]
        public string MinecraftVersion { private set; get; }

        [JsonProperty("name")]
        public readonly string Name = "Forge";

        [JsonProperty("order")]
        public readonly int Order = 5;

        [JsonProperty("version")]
        public string Version { private set; get; }

        #endregion

        #region Constants

        public const string FileFormat = "forge-{0}-patch.json";

        #endregion

        #region Constructor

        public ForgePatchFile(string location, string version, ForgeLibrary[] libs) : base(Path.Combine(location, string.Format(FileFormat, version))) {
            Version = version;
            MinecraftVersion = Version.Split('-')[0];
            Libraries = libs;
            Tweakers = new string[] {
                "net.minecraftforge.fml.common.launcher.FMLTweaker"
            };
        }

        #endregion

    }

}
