using Packsly.Core.Common;
using System.IO;

namespace Packsly.Core.Modpack.Provider {

    public class JsonFileModpackProvider : IModpackProvider {

        public bool CanUseSource(string source) {
            return File.Exists(source) && source.EndsWith(".json");
        }

        public ModpackInfo Create(string source) {
            string rawJson = string.Empty;
            using(StreamReader reader = File.OpenText(source))
                rawJson = reader.ReadToEnd();

            return PackslyFactory.Modpack.BuildFrom(rawJson);
        }

    }

}
