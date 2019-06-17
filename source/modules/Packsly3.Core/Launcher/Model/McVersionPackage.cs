using System;
using Newtonsoft.Json;

namespace Packsly3.Core.Launcher.Model {

    public class McVersionPackage {

        [JsonProperty("id")]
        public string Id { get; private set; }

        [JsonProperty("libraries")]
        public PackageLibrary[] Libraries { get; private set; } = new PackageLibrary[0];

    }

    public class PackageLibrary {

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("downloads")]
        public PackageDownloadInfo Downloads { get; private set; }

    }

    public class PackageDownloadInfo {

        [JsonProperty("artifact")]
        public DownloadArtifact Artifact { get; private set; }

    }

    public class DownloadArtifact {

        [JsonProperty("path")]
        public string Path { get; private set; }

        [JsonProperty("sha1")]
        public string Sha1 { get; private set; }

        [JsonProperty("size")]
        public int Size { get; private set; }

        [JsonProperty("artifact")]
        public Uri Url { get; private set; }

    }

}
