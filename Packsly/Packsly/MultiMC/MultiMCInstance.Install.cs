using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Packsly.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.MultiMC {

    internal partial class MultiMCInstance {

        public void Install() {
            Console.WriteLine(" - Setting up instance workspace");

            // Create intance direcory
            Console.WriteLine("   > Creating instance directory");
            Directory.CreateDirectory(InstancePath);

            // Create mc direcory
            Console.WriteLine("   > Creating instance Minecraft directory");
            Directory.CreateDirectory(MinecraftPath);

            // Icon
            Console.WriteLine("   > Downloading instance icon");
            DownloadIcon(Icon);

            // Create instance.cfg file
            Console.WriteLine("   > Creating instance.cfg");
            using(FileStream writer = CfgFile.Open(FileMode.Create)) {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine("InstanceType=OneSix");
                builder.AppendLine("IntendedVersion=" + MinecraftVersion);
                builder.AppendLine("LogPrePostOutput=true");
                builder.AppendLine("MaxMemAlloc=2048");
                builder.AppendLine("MinMemAlloc=512");
                builder.AppendLine("OverrideCommands=true");
                builder.AppendLine("OverrideConsole=false");
                builder.AppendLine("OverrideJavaArgs=false");
                builder.AppendLine("OverrideJavaLocation=false");
                builder.AppendLine("OverrideMemory=true");
                builder.AppendLine("OverrideWindow=false");
                builder.AppendLine("PostExitCommand=");
                builder.AppendLine(@"PreLaunchCommand=" + Path.Combine(PackslyDirectory.FullName, "Packsly.exe").Replace(@"\", @"\\") + " -update $INST_DIR");
                builder.AppendLine("PermGen=256");
                builder.AppendLine("iconKey=" + Name);
                builder.AppendLine("lastLaunchTime= 0");
                builder.AppendLine("name=" + Name.First().ToString().ToUpper() + Name.Substring(1));
                builder.AppendLine("notes=Managed by Packsly");
                builder.AppendLine("totalTimePlayed = 0");

                byte[] buffer = Encoding.UTF8.GetBytes(builder.ToString());
                writer.Write(buffer, 0, buffer.Length);
            }

            Console.WriteLine(" - Setting up Forge");
            InstallForge();

            Console.WriteLine(" - Setting up Packsly");
            InstallPacksly();

            Console.WriteLine(" - Installing modpack");
            DownloadMods(false);
        }

        private void InstallPacksly() {
            string self = Assembly.GetEntryAssembly().Location;

            if(!PackslyDirectory.Exists) {
                PackslyDirectory.Create();

                Console.WriteLine("   > Copying dependencies"); // TODO: Do this automaticly with all deps
                File.Copy(Path.Combine(Directory.GetCurrentDirectory(), "Newtonsoft.Json.dll"), Path.Combine(PackslyDirectory.FullName, "Newtonsoft.Json.dll"));
                File.Copy(Path.Combine(Directory.GetCurrentDirectory(), "DotNetZip.dll"), Path.Combine(PackslyDirectory.FullName, "DotNetZip.dll"));
            }

            Console.WriteLine("   > Installing Packsly");
            string dest = Path.Combine(PackslyDirectory.FullName, Path.GetFileName(self));
            File.Delete(dest);
            File.Copy(self, dest);

            CreateInstanceFile();
        }

        private void CreateInstanceFile() {
            Console.WriteLine("   > Creating instance.packsly file");
            using(FileStream writer = File.Open(Path.Combine(InstancePath, "instance.packsly"), FileMode.Create)) {
                byte[] buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this, Formatting.Indented));
                writer.Write(buffer, 0, buffer.Length);
            }
        }

        private void DownloadMods(bool soft) {
            DirectoryInfo modsDirectory = new DirectoryInfo(Path.Combine(MinecraftPath, "mods"));

            if(!modsDirectory.Exists)
                modsDirectory.Create();

            DirectoryInfo configDirectory = new DirectoryInfo(Path.Combine(MinecraftPath, "config"));

            if(!configDirectory.Exists)
                configDirectory.Create();

            List<JToken> shouldHaveMods = UpdateInfo.Value<JArray>("mods").ToList();
            string[] shouldHaveFiles = shouldHaveMods.Select(m => m.Value<string>("file")).ToArray();

            // Download config
            foreach(JObject info in shouldHaveMods) {
                JProperty configProp = info.Property("config");

                if(configProp != null) {
                    using(WebClient client = new WebClient()) {
                        foreach(JToken entry in configProp.Value) {
                            string url = entry.Value<string>();
                            Console.WriteLine("   > Downloading configuration {0}", Path.GetFileName(url));
                            client.DownloadFile(url, Path.Combine(configDirectory.FullName, Path.GetFileName(url)));
                        }
                    }
                }
            }

            // Remove files
            foreach(FileInfo file in modsDirectory.EnumerateFiles("*.jar", SearchOption.TopDirectoryOnly)) {
                if(!shouldHaveFiles.Contains(file.Name)) {
                    Console.WriteLine("   > Deleting file {0}", file.Name);
                    file.Delete();
                } else if(soft) {
                    shouldHaveMods.RemoveAll(t => t.Value<string>("file") == file.Name);
                }
            }

            // Download files
            using(WebClient client = new WebClient()) {
                foreach(JObject info in shouldHaveMods) {
                    string file = info.Value<string>("file");
                    Console.WriteLine("   > Downloading file {0}", file);
                    string destination = Path.Combine(modsDirectory.FullName, file);
                    client.DownloadFile(info.Value<string>("url"), destination);
                }
            }
        }

    }

}
