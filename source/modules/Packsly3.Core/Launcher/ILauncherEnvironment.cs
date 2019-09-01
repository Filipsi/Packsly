using System.Collections.Generic;
using System.IO;
using Packsly3.Core.Launcher.Instance;
using Packsly3.Core.Modpack;

namespace Packsly3.Core.Launcher {

    public interface ILauncherEnvironment {

        /// <summary>
        /// Unique name of this environment.
        /// This name is used in commands.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// A flag informing the installer if it should embed Packsly instance into the workspace.
        /// </summary>
        bool AllowEmbeding { get; }

        /// <summary>
        /// Provides information about environment compatibility with given workspace.
        /// Used to automatically detect environment Packsly is running in.
        /// </summary>
        /// <param name="workspace">Directory of the workspace Packsly is running in.</param>
        /// <returns>True if this environment is compatible with specified workspace.</returns>
        bool IsCompatible(DirectoryInfo workspace);

        /// <summary>
        /// Creates new minecraft instance with given id.
        /// Used to create minecraft instance during installation from modpack definition.
        /// This should is a place to prepare everything around minecraft instance itself, setting up properties like instance name is done by the installer.
        /// </summary>
        /// <param name="workspace">Directory of the workspace Packsly is running in.</param>
        /// <param name="id">Unique identifier of currently installed minecraft instance.</param>
        /// <param name="modpack">Modpack definition specs of currently installed modpack.</param>
        /// <returns>New minecraft instance.</returns>
        IMinecraftInstance CreateInstance(DirectoryInfo workspace, string id, ModpackDefinition modpack);

        /// <summary>
        /// Creates new minecraft instance from existing identifier, representing existing instance in the launcher.
        /// </summary>
        /// <param name="workspace">Directory of the workspace Packsly is running in.</param>
        /// <param name="id">Unique identifier of existing minecraft instance.</param>
        /// <returns>New minecraft instance, representing existing instance in the launcher.</returns>
        IMinecraftInstance GetInstance(DirectoryInfo workspace, string id);

        /// <summary>
        /// Creates new minecraft instances for every existing instance in the launcher.
        /// </summary>
        /// <param name="workspace">Directory of the workspace Packsly is running in.</param>
        /// <returns>List of minecraft instances existing in the launcher.</returns>
        IReadOnlyCollection<IMinecraftInstance> GetInstances(DirectoryInfo workspace);

    }

}
