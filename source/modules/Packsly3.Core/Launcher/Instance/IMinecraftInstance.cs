using System.Collections.Generic;
using System.IO;
using Packsly3.Core.FileSystem;
using Packsly3.Core.Launcher.Modloader;
using Packsly3.Core.Modpack;

namespace Packsly3.Core.Launcher.Instance {

    public interface IMinecraftInstance {

        DirectoryInfo Location { get; }

        string Id { get; }

        string Name { set; get; }

        string MinecraftVersion { set; get; }

        Icon Icon { get; }

        EnvironmentVariables EnvironmentVariables { get; }

        PackslyInstanceFile PackslyConfig { get; }

        ModLoaderManager ModLoaderManager { get; }

        FileManager Files { get; }

        void Configure(string json);

        void Save();

        void Delete();

    }

}
