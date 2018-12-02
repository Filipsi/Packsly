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
        public int OpPermissionLevel {
            get => Get<int>("op-permission-level");
            set => Set("op-permission-level", value);
        }

        [JsonProperty(PropertyName = "allowNether")]
        public bool AllowNether {
            get => Get<bool>("allow-nether");
            set => Set("allow-nether", value);
        }

        [JsonProperty(PropertyName = "levelName")]
        public string LevelName {
            get => Get<string>("level-name");
            set => Set("level-name", value);
        }

        [JsonProperty(PropertyName = "enableQuery")]
        public bool EnableQuery {
            get => Get<bool>("enable-query");
            set => Set("enable-query", value);
        }

        [JsonProperty(PropertyName = "allowFlight")]
        public bool AllowFlight {
            get => Get<bool>("allow-flight");
            set => Set("allow-flight", value);
        }

        [JsonProperty(PropertyName = "preventProxyConnections")]
        public bool PreventProxyConnections {
            get => Get<bool>("prevent-proxy-connections");
            set => Set("prevent-proxy-connections", value);
        }

        [JsonProperty(PropertyName = "serverPort")]
        public int ServerPort {
            get => Get<int>("server-port");
            set => Set("server-port", value);
        }

        [JsonProperty(PropertyName = "maxWorldSize")]
        public int MaxWorldSize {
            get => Get<int>("max-world-size");
            set => Set("max-world-size", value);
        }

        [JsonProperty(PropertyName = "levelType")]
        public string LevelType {
            get => Get<string>("level-type");
            set => Set("level-type", value);
        }

        [JsonProperty(PropertyName = "enableRcon")]
        public bool EnableRcon {
            get => Get<bool>("enable-rcon");
            set => Set("enable-rcon", value);
        }

        [JsonProperty(PropertyName = "levelSeed")]
        public int LevelSeed {
            get => Get<int>("level-seed");
            set => Set("level-seed", value);
        }

        [JsonProperty(PropertyName = "forceGamemode")]
        public bool ForceGamemode {
            get => Get<bool>("force-gamemode");
            set => Set("force-gamemode", value);
        }

        [JsonProperty(PropertyName = "serverIp")]
        public string ServerIp {
            get => Get<string>("server-ip");
            set => Set("server-ip", value);
        }

        [JsonProperty(PropertyName = "networkCompressionThreshold")]
        public int NetworkCompressionThreshold {
            get => Get<int>("network-compression-threshold");
            set => Set("network-compression-threshold", value);
        }

        [JsonProperty(PropertyName = "maxBuildHeight")]
        public int MaxBuildHeight {
            get => Get<int>("max-build-height");
            set => Set("max-build-height", value);
        }

        [JsonProperty(PropertyName = "spawnNpcs")]
        public bool SpawnNpcs {
            get => Get<bool>("spawn-npcs");
            set => Set("spawn-npcs", value);
        }

        [JsonProperty(PropertyName = "whiteList")]
        public bool WhiteList {
            get => Get<bool>("white-list");
            set => Set("white-list", value);
        }

        [JsonProperty(PropertyName = "spawnAnimals")]
        public bool SpawnAnimals {
            get => Get<bool>("spawn-animals");
            set => Set("spawn-animals", value);
        }

        [JsonProperty(PropertyName = "hardcore")]
        public bool Hardcore {
            get => Get<bool>("hardcore");
            set => Set("hardcore", value);
        }

        [JsonProperty(PropertyName = "snooperEnabled")]
        public bool SnooperEnabled {
            get => Get<bool>("snooper-enabled");
            set => Set("snooper-enabled", value);
        }

        [JsonProperty(PropertyName = "resourcePackSha1")]
        public int ResourcePackSha1 {
            get => Get<int>("resource-pack-sha1");
            set => Set("resource-pack-sha1", value);
        }

        [JsonProperty(PropertyName = "onlineMode")]
        public bool OnlineMode {
            get => Get<bool>("online-mode");
            set => Set("online-mode", value);
        }

        [JsonProperty(PropertyName = "resourcePack")]
        public string ResourcePack {
            get => Get<string>("resource-pack");
            set => Set("resource-pack", value);
        }

        [JsonProperty(PropertyName = "pvp")]
        public bool Pvp {
            get => Get<bool>("pvp");
            set => Set("pvp", value);
        }

        [JsonProperty(PropertyName = "difficulty")]
        public int Difficulty {
            get => Get<int>("difficulty");
            set => Set("difficulty", value);
        }

        [JsonProperty(PropertyName = "enableCommandBlock")]
        public bool EnableCommandBlock {
            get => Get<bool>("enable-command-block");
            set => Set("enable-command-block", value);
        }

        [JsonProperty(PropertyName = "gamemode")]
        public int Gamemode {
            get => Get<int>("gamemode");
            set => Set("gamemode", value);
        }

        [JsonProperty(PropertyName = "playerIdleTimeout")]
        public int PlayerIdleTimeout {
            get => Get<int>("player-idle-timeout");
            set => Set("player-idle-timeout", value);
        }

        [JsonProperty(PropertyName = "maxPlayers")]
        public int MaxPlayers {
            get => Get<int>("max-players");
            set => Set("max-players", value);
        }

        [JsonProperty(PropertyName = "maxTickTime")]
        public int MaxTickTime {
            get => Get<int>("max-tick-time");
            set => Set("max-tick-time", value);
        }

        [JsonProperty(PropertyName = "spawnMonsters")]
        public bool SpawnMonsters {
            get => Get<bool>("spawn-monsters");
            set => Set("spawn-monsters", value);
        }

        [JsonProperty(PropertyName = "viewDistance")]
        public int ViewDistance {
            get => Get<int>("view-distance");
            set => Set("view-distance", value);
        }

        [JsonProperty(PropertyName = "generateStructures")]
        public bool GenerateStructures {
            get => Get<bool>("generate-structures");
            set => Set("generate-structures", value);
        }

        [JsonProperty(PropertyName = "motd")]
        public string Motd {
            get => Get<string>("motd");
            set => Set("motd", value);
        }

        #endregion

        public ServerPropertiesFile(string path) : base(Path.Combine(path, "server.properties")) {
        }

        public ServerPropertiesFile WithDefaults() {
            GeneratorSettings = string.Empty;
            OpPermissionLevel = 4;
            AllowNether = true;
            LevelName = "world";
            EnableQuery = false;
            AllowFlight = false;
            PreventProxyConnections = false;
            ServerPort = 25565;
            MaxWorldSize = 29999984;
            LevelType = "DEFAULT";
            EnableRcon = false;
            ForceGamemode = false;
            ServerIp = string.Empty;
            NetworkCompressionThreshold = 256;
            MaxBuildHeight = 256;
            SpawnNpcs = true;
            WhiteList = false;
            SpawnAnimals = true;
            Hardcore = false;
            SnooperEnabled = true;
            OnlineMode = true;
            ResourcePack = string.Empty;
            Pvp = true;
            Difficulty = 1;
            EnableCommandBlock = false;
            Gamemode = 0;
            PlayerIdleTimeout = 0;
            MaxPlayers = 20;
            MaxTickTime = 60000;
            SpawnMonsters = true;
            ViewDistance = 10;
            GenerateStructures = true;
            Motd = "A Minecraft Server";

            return this;
        }

    }

}
