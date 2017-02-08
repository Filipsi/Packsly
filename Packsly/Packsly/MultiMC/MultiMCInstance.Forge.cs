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

            string version = UpdateInfo.Value<string>("forge");
            string patchDir = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "patches");
            string patchFile = Path.Combine(patchDir, version + ".json");

            if(!Directory.Exists(patchDir))
                Directory.CreateDirectory(patchDir);

            if(!File.Exists(patchFile)) {
                Console.WriteLine("   > Patch file for " + version + " not found, building one");
                JObject patchRaw = BuildForgePatch(
                    ResolveForgeLibraries(version), version
                );

                Console.WriteLine("   > Saving patch file for " + version + " for later usage");
                using(FileStream writer = File.Open(patchFile, FileMode.Create)) {
                    byte[] buffer = Encoding.UTF8.GetBytes(patchRaw.ToString(Formatting.Indented));
                    writer.Write(buffer, 0, buffer.Length);
                }
            }

            string paches = Path.Combine(InstancePath, "patches");
            if(!Directory.Exists(paches)) {
                Console.WriteLine("   > Creating instace patches directory");
                Directory.CreateDirectory(paches);
            }

            string dest = Path.Combine(paches, "net.minecraftforge.json");
            if(File.Exists(dest))
                File.Delete(dest);

            Console.WriteLine("   > Saving forge patch file as patches/net.minecraftforge.json");
            File.Copy(patchFile, dest);
        }

        private JArray ResolveForgeLibraries(string version) {
            string file = "forge-" + version + "-universal.jar";
            string tempDir = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "temp");
            string tempJar = Path.Combine(tempDir, file);
            Uri url = new Uri("http://files.minecraftforge.net/maven/net/minecraftforge/forge/" + version + "/" + file);

            Directory.CreateDirectory(tempDir);

            Console.WriteLine("   > Downloading " + file);
            using(WebClient client = new WebClient())
                client.DownloadFile(url, tempJar);

            Console.WriteLine("   > Extracting universal jar");
            using(ZipFile jar = new ZipFile(tempJar))
                jar.ExtractAll(tempDir);

            Console.WriteLine("   > Reading 'version.json'");
            string versionInfo;
            using(StreamReader reader = File.OpenText(Path.Combine(tempDir, "version.json")))
                versionInfo = reader.ReadToEnd();

            JArray libs = JObject.Parse(versionInfo).Value<JArray>("libraries");

            Directory.Delete(tempDir, true);

            return libs;
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
                    string name = entry.Value<string>("name");
                    JToken url = entry.GetValue("url");

                    JObject libroot = new JObject();
                    libroot.Add(new JProperty("name", name));
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
