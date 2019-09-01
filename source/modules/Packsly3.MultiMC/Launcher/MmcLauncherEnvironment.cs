using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Packsly3.Core;
using Packsly3.Core.Common.Register;
using Packsly3.Core.Launcher;
using Packsly3.Core.Launcher.Instance;
using Packsly3.Core.Modpack;

namespace Packsly3.MultiMC.Launcher {

    [Register]
    public class MmcLauncherEnvironment : ILauncherEnvironment {

        #region Properties

        public string Name { get; } = "multimc";

        public bool AllowEmbeding { get; } = true;

        #endregion

        private static readonly IReadOnlyCollection<IMinecraftInstance> noInstances =
            Array.AsReadOnly(new IMinecraftInstance[0]);

        #region Logic

        public bool IsCompatible(DirectoryInfo workspace) {
            string executablePath = Packsly.IsLinux
                ? Path.Combine(workspace.FullName, "bin", "MultiMC")
                : Path.Combine(workspace.FullName, "MultiMC.exe");

            return File.Exists(executablePath);
        }

        public IMinecraftInstance CreateInstance(DirectoryInfo workspace, string id, ModpackDefinition modpack) {
            DirectoryInfo instancesFolder = workspace.GetDirectories("instances").FirstOrDefault();

            if (instancesFolder == null) {
                instancesFolder = workspace.CreateSubdirectory("instances");
            } else {
                if (instancesFolder.GetDirectories().Any(dir => dir.Name == id)) {
                    throw new DuplicateNameException($"Instance with id '{id}' already exists!");
                }
            }

            DirectoryInfo instanceFolder = new DirectoryInfo(Path.Combine(instancesFolder.FullName, id));
            return new MmcMinecraftInstance(instanceFolder);
        }

        public IMinecraftInstance GetInstance(DirectoryInfo workspace, string id) {
            DirectoryInfo instancesFolder = workspace.GetDirectories("instances").FirstOrDefault();
            if (instancesFolder == null) {
                throw new DirectoryNotFoundException("Unable to locate 'instances' folder!");
            }

            DirectoryInfo instanceForlder = instancesFolder
                .GetDirectories()
                .FirstOrDefault(dir => dir.Name == id);

            if (instanceForlder == null) {
                throw new DirectoryNotFoundException($"There is no instance with id '{id}'!");
            }

            return new MmcMinecraftInstance(instanceForlder);
        }

        public IReadOnlyCollection<IMinecraftInstance> GetInstances(DirectoryInfo workspace) {
            DirectoryInfo instancesFolder = workspace.GetDirectories("instances").FirstOrDefault();
            if (instancesFolder == null) {
                return noInstances;
            }

            MmcMinecraftInstance[] instances = instancesFolder
                .GetDirectories()
                .Where(dir => !dir.Name.StartsWith("_"))
                .Select(instnaceFolder => new MmcMinecraftInstance(instnaceFolder))
                .ToArray();

            return Array.AsReadOnly(instances);
        }

        #endregion

    }

}
