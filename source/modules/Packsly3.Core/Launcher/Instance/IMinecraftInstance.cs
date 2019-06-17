using System.IO;
using Packsly3.Core.FileSystem.Impl;
using Packsly3.Core.Launcher.Modloader;

namespace Packsly3.Core.Launcher.Instance {

    public interface IMinecraftInstance {

        DirectoryInfo Location { get; }

        EnvironmentVariables EnvironmentVariables { get; }

        PackslyInstanceFile PackslyConfig { get; }

        string Id { get; }

        string Name { set; get; }

        string MinecraftVersion { set; get; }

        Icon Icon { get; }

        ModLoaderManager ModLoaderManager { get; }

        FileManager Files { get; }

        void Configure(string json);

        void Load();

        void Save();

        void Delete();

    }

}
