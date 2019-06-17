using System;
using Newtonsoft.Json;

namespace Packsly3.Core.Launcher.Model {

    public class LauncherMetadata {

        [JsonProperty("latest")]
        public LatestVersion Latest { get; private set; }

        [JsonProperty("versions")]
        public McVersionInfo[] VersionsInfo { get; private set; }

    }

    public class LatestVersion {

        [JsonProperty("release")]
        public string Release { get; private set; }

        [JsonProperty("snapshot")]
        public string Snapshot { get; private set; }

    }

    public class McVersionInfo {

        [JsonProperty("id")]
        public string Id { get; private set; }

        [JsonProperty("type")]
        public string Type { get; private set; }

        [JsonProperty("url")]
        public Uri Url { get; private set; }

        [JsonProperty("time")]
        public DateTime Time { get; private set; }

        [JsonProperty("releaseTime")]
        public DateTime ReleaseTime { get; private set; }

    }

}
