using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;
using Packsly3.Core.Common.Register;
using Packsly3.Core.Launcher.Instance;
using Packsly3.Core.Launcher.Model;

namespace Packsly3.Core.Launcher {

    public class MinecraftLauncher {

        #region Properties

        public DirectoryInfo Workspace {
            get => workspace;
            set {
                workspace = value;
                logger.Debug($"Launcher workspace was set to {workspace.FullName}");
            }
        }

        public string Name
            => CurrentEnvironment.Name;

        public bool CanEmbed
            => CurrentEnvironment.AllowEmbeding;

        private ILauncherEnvironment CurrentEnvironment {
            get {
                if (currentEnviroment != null) {
                    return currentEnviroment;
                }

                currentEnviroment = GetCurrentEnvironment();
                logger.Debug($"Launcher environment was set to {currentEnviroment} by auto-detect.");
                return currentEnviroment;
            }
        }

        #endregion

        #region Fields

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly ILauncherEnvironment[] enviroments = RegisterAttribute.GetOccurrencesFor<ILauncherEnvironment>();
        private static readonly Uri launcherMetaUrl = new Uri("https://launchermeta.mojang.com/mc/game/version_manifest.json");

        private ILauncherEnvironment currentEnviroment;
        private DirectoryInfo workspace = Packsly.Configuration.Workspace;

        #endregion

        internal MinecraftLauncher() {
        }

        #region Logic

        public void ForceEnviromentUsage(string name) {
            ILauncherEnvironment environment = enviroments.FirstOrDefault((env) => env.Name == name);
            currentEnviroment = environment ?? throw new Exception($"Environment with name '{name}' does not exist.");
            logger.Debug($"Environment was forcefully changed to {currentEnviroment}");
        }

        public async Task<string> GetLwjglVersion(string minecraftVersion) {
            using (HttpClient client = new HttpClient()) {
                try {
                    logger.Info("Getting launcher metadata from Mojang...");
                    LauncherMetadata metadata = await RequestAndParse<LauncherMetadata>(client, launcherMetaUrl);
                    logger.Info($" - {metadata.VersionsInfo.Length} Minecraft version infos available...");

                    McVersionInfo versionInfo = metadata.VersionsInfo.FirstOrDefault(ver => ver.Id == minecraftVersion);
                    if (versionInfo == null) {
                        throw new Exception($"Failed to obtain LWJGL version for Minecraft '{minecraftVersion}', this version was not found in launcher version info entries.");
                    }

                    string displayManifestAddress = versionInfo
                        .Url
                        .AbsoluteUri
                        .Replace("https://launchermeta.mojang.com/v1/packages/", string.Empty)
                        .Replace(".json", string.Empty);

                    logger.Info($" - Minecraft {minecraftVersion} version manifest found at '{displayManifestAddress}'...");
                    McVersionPackage versionPackage = await RequestAndParse<McVersionPackage>(client, versionInfo.Url);
                    if (versionPackage == null) {
                        throw new Exception($"Failed to obtain LWJGL version for Minecraft '{minecraftVersion}', unable to parse version manifest.");
                    }

                    logger.Info($" - Version manifest contains {versionPackage.Libraries.Length} libraries...");
                    PackageLibrary lwjglLib = versionPackage.Libraries.FirstOrDefault(lib => lib.Name.StartsWith("org.lwjgl") && lib.Name.Contains(":lwjgl:"));
                    if (lwjglLib == null) {
                        throw new Exception($"Failed to obtain LWJGL version for Minecraft '{minecraftVersion}', LWJGL library not found in version manifest.");
                    }

                    string[] lwjglVersionParts = lwjglLib.Name.Split(':');
                    if (lwjglVersionParts.Length != 3) {
                        throw new Exception($"Failed to obtain LWJGL version for Minecraft '{minecraftVersion}', unexpected version format.");
                    }

                    logger.Info($" - This Minecraft version is using '{lwjglLib.Name}'.");
                    return lwjglVersionParts[2];

                } catch (HttpRequestException e) {
                    logger.Error("Failed to obtain Minecraft versions meta-data from Mojang!", e);
                }
            }

            return null;
        }

        private static async Task<T> RequestAndParse<T>(HttpClient client, Uri url) {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string raw = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(raw);
        }

        #endregion

        #region Wrappers

        private ILauncherEnvironment GetCurrentEnvironment() {
            if (enviroments.Length == 0) {
                throw new Exception("There are no environments registered.");
            }

            ILauncherEnvironment env = enviroments.FirstOrDefault(e => e.IsCompatible(Workspace));
            if (env == null) {
                throw new Exception($"No compatible environment found in workspace '{Workspace.FullName}'!");
            }

            return env;
        }

        public IMinecraftInstance[] GetInstances()
            => CurrentEnvironment.GetInstances(Workspace).ToArray();

        public IMinecraftInstance GetInstance(string id)
            => CurrentEnvironment.GetInstance(Workspace, id);

        public IMinecraftInstance CreateInstance(string id) {
            logger.Debug($"Launcher using {currentEnviroment} environment is creating new minecraft instance with id {id}");
            return CurrentEnvironment.CreateInstance(Workspace, id);
        }

        public IMinecraftInstance CreateInstanceFromModpack(FileInfo modpackFile)
            => MinecraftInstanceFactory.CreateFromModpack(modpackFile);

        public IMinecraftInstance CreateInstanceFromModpack(Uri modpackFileUrl)
            => MinecraftInstanceFactory.CreateFromModpack(modpackFileUrl);

        public IMinecraftInstance CreateInstanceFromModpack(string instanceId, string modpackJson)
            => MinecraftInstanceFactory.CreateFromModpack(instanceId, modpackJson);

        #endregion
    }

}
