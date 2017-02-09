using Ionic.Zip;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.MultiMC {

    internal partial class MultiMCInstance {

        private void InstallForge() {
            if(!Directory.Exists(InstancePath))
                throw new Exception("Cound not install Forge because MultiMC instance directory does not exist");

            string forgeVersion = UpdateInfo.Value<string>("forge");
            string myDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string myPatchesDirectory = EnsureDirectory(Path.Combine(myDirectory, "patches"));
            string myPatch = Path.Combine(myPatchesDirectory, forgeVersion + ".json");
            bool missingPatch = !File.Exists(myPatch);

            if(missingPatch) {
                string tempDir = EnsureDirectory(Path.Combine(myDirectory, "temp"));
                string forgeJar = DownloadForgeUniversal(forgeVersion, tempDir);

                Console.WriteLine("   > Patch file for " + forgeVersion + " not found, building one");
                JObject patchRaw = BuildForgePatch(ResolveForgeLibraries(forgeJar), forgeVersion);

                Console.WriteLine("   > Saving patch file for " + forgeVersion + " for later usage");
                using(FileStream writer = File.Open(myPatch, FileMode.Create)) {
                    byte[] buffer = Encoding.UTF8.GetBytes(patchRaw.ToString(Formatting.Indented));
                    writer.Write(buffer, 0, buffer.Length);
                }
                

                Directory.Delete(tempDir, true);
            }

            string paches = Path.Combine(InstancePath, "patches");
            if(!Directory.Exists(paches)) {
                Console.WriteLine("   > Creating instace patches directory");
                Directory.CreateDirectory(paches);
            }

            string patchDestination = Path.Combine(paches, "net.minecraftforge.json");
            if(File.Exists(patchDestination))
                File.Delete(patchDestination);

            Console.WriteLine("   > Saving forge patch file as patches/net.minecraftforge.json");
            File.Copy(myPatch, patchDestination);
        }

        private string DownloadForgeUniversal(string version, string destination) {
            string fileName = "forge-" + version + "-universal.jar";
            string jarPath = Path.Combine(destination, fileName);

            Console.WriteLine("   > Downloading " + fileName);
            using(WebClient client = new WebClient())
                client.DownloadFile(
                    new Uri("http://files.minecraftforge.net/maven/net/minecraftforge/forge/" + version + "/" + fileName),
                    jarPath
                );

            return jarPath;
        }

        private JArray ResolveForgeLibraries(string forgeJar) {
            string root = Path.GetDirectoryName(forgeJar);

            Console.WriteLine("   > Extracting universal jar");
            using(ZipFile jar = new ZipFile(forgeJar))
                jar.ExtractAll(root);

            Console.WriteLine("   > Reading 'version.json'");
            string versionInfo;
            using(StreamReader reader = File.OpenText(Path.Combine(root, "version.json")))
                versionInfo = reader.ReadToEnd();

            return JObject.Parse(versionInfo).Value<JArray>("libraries");
        }

        private JObject BuildForgePatch(JArray libraries, string version) {
            Console.WriteLine("   > Building patch");
            JObject root = new JObject();

            Console.WriteLine("   > Resolving libraries");
            JArray libs = new JArray();
            foreach(JObject entry in libraries) {
                JToken client = entry.GetValue("clientreq");
                JToken server = entry.GetValue("serverreq");

                if((client == null || client.Value<bool>()) || (server == null || !server.Value<bool>())) {
                    JObject libroot = new JObject();

                    if(entry.GetValue("checksums") != null)
                        libroot.Add(new JProperty("MMC-hint", "forge-pack-xz"));

                    string name = entry.Value<string>("name");
                    if(name.Contains("net.minecraftforge:forge"))
                        name += ":universal";
                    libroot.Add(new JProperty("name", name));

                    JToken url = entry.GetValue("url");
                    if(url != null)
                        libroot.Add(new JProperty("url", url.Value<string>()));

                    libs.Add(libroot);
                }
            }
            root.Add(new JProperty("+libraries", libs));

            Console.WriteLine("   > Writing tweakers");
            root.Add(new JProperty("+tweakers", new JArray("net.minecraftforge.fml.common.launcher.FMLTweaker")));

            Console.WriteLine("   > Writing metadata");
            root.Add(new JProperty("fileId", "net.minecraftforge"));
            root.Add(new JProperty("mainClass", "net.minecraft.launchwrapper.Launch"));
            root.Add(new JProperty("mcVersion", MinecraftVersion));
            root.Add(new JProperty("name", "Forge"));
            root.Add(new JProperty("order", 5));
            root.Add(new JProperty("version", version));

            return root;
        }

    }

}
