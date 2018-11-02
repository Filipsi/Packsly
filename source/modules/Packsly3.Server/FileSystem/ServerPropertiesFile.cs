using Newtonsoft.Json;
using Packsly3.Core.FileSystem;
using System.IO;

namespace Packsly3.Server.FileSystem {

    public class ServerPropertiesFile : DataPairFile {

        #region Exposed Wrappers

        [JsonProperty(PropertyName = "generatorSettings")]
        public string GeneratorSettings {
            get => Get<string>("generator-settings");
            set => Set("generator-settings", value);
        }

        [JsonProperty(PropertyName = "opPermissionLevel")]
        public string OpPermissionLevel {
            get => Get<string>("op-permission-level");
            set => Set("op-permission-level", value);
        }

        [JsonProperty(PropertyName = "allowNether")]
        public string AllowNether {
            get => Get<string>("allow-nether");
            set => Set("allow-nether", value);
        }

        [JsonProperty(PropertyName = "levelName")]
        public string LevelName {
            get => Get<string>("level-name");
            set => Set("level-name", value);
        }

        [JsonProperty(PropertyName = "enableQuery")]
        public string EnableQuery {
            get => Get<string>("enable-query");
            set => Set("enable-query", value);
        }

        [JsonProperty(PropertyName = "allowFlight")]
        public string AllowFlight {
            get => Get<string>("allow-flight");
            set => Set("allow-flight", value);
        }

        [JsonProperty(PropertyName = "preventProxyConnections")]
        public string PreventProxyConnections {
            get => Get<string>("prevent-proxy-connections");
            set => Set("prevent-proxy-connections", value);
        }

        [JsonProperty(PropertyName = "serverPort")]
        public string ServerPort {
            get => Get<string>("server-port");
            set => Set("server-port", value);
        }

        [JsonProperty(PropertyName = "maxWorldSize")]
        public string MaxWorldSize {
            get => Get<string>("max-world-size");
            set => Set("max-world-size", value);
        }

        [JsonProperty(PropertyName = "levelType")]
        public string LevelType {
            get => Get<string>("level-type");
            set => Set("level-type", value);
        }

        [JsonProperty(PropertyName = "enableRcon")]
        public string EnableRcon {
            get => Get<string>("enable-rcon");
            set => Set("enable-rcon", value);
        }

        [JsonProperty(PropertyName = "levelSeed")]
        public string LevelSeed {
            get => Get<string>("level-seed");
            set => Set("level-seed", value);
        }

        [JsonProperty(PropertyName = "forceGamemode")]
        public string ForceGamemode {
            get => Get<string>("force-gamemode");
            set => Set("force-gamemode", value);
        }

        [JsonProperty(PropertyName = "serverIp")]
        public string ServerIp {
            get => Get<string>("server-ip");
            set => Set("server-ip", value);
        }

        [JsonProperty(PropertyName = "networkCompressionThreshold")]
        public string NetworkCompressionThreshold {
            get => Get<string>("network-compression-threshold");
            set => Set("network-compression-threshold", value);
        }

        [JsonProperty(PropertyName = "maxBuildHeight")]
        public string MaxBuildHeight {
            get => Get<string>("max-build-height");
            set => Set("max-build-height", value);
        }

        [JsonProperty(PropertyName = "spawnNpcs")]
        public string SpawnNpcs {
            get => Get<string>("spawn-npcs");
            set => Set("spawn-npcs", value);
        }

        [JsonProperty(PropertyName = "whiteList")]
        public string WhiteList {
            get => Get<string>("white-list");
            set => Set("white-list", value);
        }

        [JsonProperty(PropertyName = "spawnAnimals")]
        public string SpawnAnimals {
            get => Get<string>("spawn-animals");
            set => Set("spawn-animals", value);
        }

        [JsonProperty(PropertyName = "hardcore")]
        public string Hardcore {
            get => Get<string>("hardcore");
            set => Set("hardcore", value);
        }

        [JsonProperty(PropertyName = "snooperEnabled")]
        public string SnooperEnabled {
            get => Get<string>("snooper-enabled");
            set => Set("snooper-enabled", value);
        }

        [JsonProperty(PropertyName = "resourcePackSha1")]
        public string ResourcePackSha1 {
            get => Get<string>("resource-pack-sha1");
            set => Set("resource-pack-sha1", value);
        }

        [JsonProperty(PropertyName = "onlineMode")]
        public string OnlineMode {
            get => Get<string>("online-mode");
            set => Set("online-mode", value);
        }

        [JsonProperty(PropertyName = "resourcePack")]
        public string ResourcePack {
            get => Get<string>("resource-pack");
            set => Set("resource-pack", value);
        }

        [JsonProperty(PropertyName = "pvp")]
        public string PVP {
            get => Get<string>("pvp");
            set => Set("pvp", value);
        }

        [JsonProperty(PropertyName = "difficulty")]
        public string Difficulty {
            get => Get<string>("difficulty");
            set => Set("difficulty", value);
        }

        [JsonProperty(PropertyName = "enableCommandBlock")]
        public string EnableCommandBlock {
            get => Get<string>("enable-command-block");
            set => Set("enable-command-block", value);
        }

        [JsonProperty(PropertyName = "gamemode")]
        public string Gamemode {
            get => Get<string>("gamemode");
            set => Set("gamemode", value);
        }

        [JsonProperty(PropertyName = "playerIdleTimeout")]
        public string PlayerIdleTimeout {
            get => Get<string>("player-idle-timeout");
            set => Set("player-idle-timeout", value);
        }

        [JsonProperty(PropertyName = "maxPlayers")]
        public string MaxPlayers {
            get => Get<string>("max-players");
            set => Set("max-players", value);
        }

        [JsonProperty(PropertyName = "maxTickTime")]
        public string MaxTickTime {
            get => Get<string>("max-tick-time");
            set => Set("max-tick-time", value);
        }

        [JsonProperty(PropertyName = "spawnMonsters")]
        public string SpawnMonsters {
            get => Get<string>("spawn-monsters");
            set => Set("spawn-monsters", value);
        }

        [JsonProperty(PropertyName = "viewDistance")]
        public string ViewDistance {
            get => Get<string>("view-distance");
            set => Set("view-distance", value);
        }

        [JsonProperty(PropertyName = "generateStructures")]
        public string GenerateStructures {
            get => Get<string>("generate-structures");
            set => Set("generate-structures", value);
        }

        [JsonProperty(PropertyName = "motd")]
        public string MessageOfTheDay {
            get => Get<string>("motd");
            set => Set("motd", value);
        }

        #endregion

        public ServerPropertiesFile(string path) : base(Path.Combine(path, "server.properties")) {
        }

    }

}
