using System.Data;
using System.IO;
using System.Linq;
using Packsly3.Core.Common.Register;
using Packsly3.Core.Launcher;
using Packsly3.Core.Launcher.Instance;

namespace Packsly3.MultiMC.Launcher {

    [Register]
    public class MmcLauncherEnvironment : ILauncherEnvironment {

        public string Name { get; } = "multimc";

        public bool IsCompatible(DirectoryInfo workspace)
            => File.Exists(Path.Combine(workspace.FullName, "MultiMC.exe")) ||
               File.Exists(Path.Combine(workspace.FullName, "bin", "MultiMC"));

        public IMinecraftInstance[] GetInstances(DirectoryInfo workspace) {
            DirectoryInfo instancesFolder = workspace.GetDirectories("instances").FirstOrDefault();

            if (instancesFolder == null) {
                return new IMinecraftInstance[0];
            }

            DirectoryInfo[] instances = instancesFolder.GetDirectories()
                .Where(dir => !dir.Name.StartsWith("_"))
                .ToArray();

            // ReSharper disable once CoVariantArrayConversion
            return instances
                .Select(instnaceFolder => new MmcMinecraftInstance(instnaceFolder))
                .ToArray();
        }

        public IMinecraftInstance GetInstance(DirectoryInfo workspace, string id) {
            DirectoryInfo instancesFolder = workspace.GetDirectories("instances").FirstOrDefault();

            if (instancesFolder == null) {
                throw new DirectoryNotFoundException($"There is no instance with id '{id}'!");
            }

            DirectoryInfo instanceForlder = instancesFolder
                .GetDirectories()
                .FirstOrDefault(dir => dir.Name == id);

            if (instanceForlder == null) {
                throw new DirectoryNotFoundException($"There is no instance with id '{id}'!");
            }

            return new MmcMinecraftInstance(instanceForlder);
        }

        public IMinecraftInstance CreateInstance(DirectoryInfo workspace, string id) {
            DirectoryInfo instancesFolder = workspace.GetDirectories("instances").FirstOrDefault();

            if (instancesFolder == null) {
                instancesFolder = workspace.CreateSubdirectory("instances");
            } else {
                bool hasInstanceWithId = instancesFolder
                    .GetDirectories()
                    .FirstOrDefault(dir => dir.Name == id) != null;

                if (hasInstanceWithId) {
                    throw new DuplicateNameException($"Instance with id '{id}' already exists!");
                }
            }

            DirectoryInfo instanceFolder = new DirectoryInfo(Path.Combine(instancesFolder.FullName, id));
            return new MmcMinecraftInstance(instanceFolder);
        }

    }

}
