using Newtonsoft.Json;
using Packsly.Core.Adapter.Forge;
using Packsly.Core.Common.FileSystem;
using System.IO;

namespace Packsly.MultiMc.Adapter.Forge {

    public class ForgePatchFile : JsonFile {

        #region Properties

        [JsonProperty("+libraries")]
        public ForgeLibrary[] Libraries { private set; get; }

        [JsonProperty("+tweakers")]
        public string[] Tweakers { private set; get; } = new string[] {
            "net.minecraftforge.fml.common.launcher.FMLTweaker"
        };

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
        }

        #endregion

    }

}
