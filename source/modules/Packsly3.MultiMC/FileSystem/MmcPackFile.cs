using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Packsly3.Core;
using Packsly3.Core.FileSystem;

namespace Packsly3.MultiMC.FileSystem {

    internal class MmcPackFile : JsonFile {

        #region Properties

        [JsonProperty("formatVersion")]
        public int FormatVersion { private set; get; }

        [JsonProperty("components")]
        public List<Component> Components { private set; get; }

        #endregion

        #region Fields

        private static readonly JsonSerializerSettings mmcPackSerializerSettings = new JsonSerializerSettings {
            ObjectCreationHandling = ObjectCreationHandling.Replace
        };

        #endregion

        public MmcPackFile(string path) : base(Path.Combine(path, "mmc-pack.json")) {
        }

        #region Logic

        public override void SetDefaultValues() {
            FormatVersion = 1;
            Components = new List<Component>();
        }

        protected override JsonSerializerSettings GetSerializerSettings() {
            return mmcPackSerializerSettings;
        }

        internal void WithDefaults(string minecraftVersion) {
            string lwjglVersion = Packsly.Launcher
                .GetLwjglVersion(minecraftVersion)
                .GetAwaiter()
                .GetResult();

            if (!int.TryParse(lwjglVersion[0].ToString(), out int lwjglMajorVersion)) {
                throw new Exception($"Failed to determinate LWJGL major version number from version string '{lwjglVersion}'!");
            }

            string lwjglUid = $"org.lwjgl{(lwjglMajorVersion >= 3 ? lwjglMajorVersion.ToString() : string.Empty)}";

            Components.Add(
                new Component {
                    Name = $"LWJGL {lwjglMajorVersion}",
                    Uid = lwjglUid,
                    Version = lwjglVersion,
                    CachedVersion = lwjglVersion,
                    CachedVolatile = true,
                    DependencyOnly = true
                }
            );

            Components.Add(
                new Component {
                    Name = "Minecraft",
                    Uid = "net.minecraft",
                    Version = minecraftVersion,
                    CachedVersion = minecraftVersion,
                    Important = true,
                    Requirements = new ComponentRequirement[] {
                        new ComponentSudgestedRequirement {
                            Uid = lwjglUid,
                            Suggests = lwjglVersion
                        }
                    }
                }
            );
        }

        #endregion

        #region Internals

        [JsonObject(MemberSerialization.OptIn)]
        internal class Component {

            [JsonProperty("cachedName", DefaultValueHandling = DefaultValueHandling.Ignore)]
            public string Name { set; get; }

            [JsonProperty("cachedRequires", DefaultValueHandling = DefaultValueHandling.Ignore)]
            public ComponentRequirement[] Requirements { set; get; }

            [JsonProperty("important", DefaultValueHandling = DefaultValueHandling.Ignore)]
            public bool Important { set; get; }

            [JsonProperty("cachedVersion", DefaultValueHandling = DefaultValueHandling.Ignore)]
            public string CachedVersion { set; get; }

            [JsonProperty("cachedVolatile")]
            public bool CachedVolatile { set; get; }

            [JsonProperty("dependencyOnly", DefaultValueHandling = DefaultValueHandling.Ignore)]
            public bool DependencyOnly { set; get; }

            [JsonProperty("uid")]
            public string Uid { set; get; }

            [JsonProperty("version")]
            public string Version { set; get; }

        }

        [JsonObject(MemberSerialization.OptIn)]
        internal class ComponentRequirement {

            [JsonProperty("uid")]
            public string Uid { set; get; }

        }

        [JsonObject(MemberSerialization.OptIn)]
        internal class ComponentSpecificRequirement : ComponentRequirement {

            [JsonProperty("equals")]
            public string EquivalentTo { set; get; }

        }

        internal class ComponentSudgestedRequirement : ComponentRequirement {

            [JsonProperty("suggests")]
            public string Suggests { set; get; }

        }

        #endregion


    }



}
