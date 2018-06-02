using System.Collections.Generic;
using System.IO;
using Packsly3.Core.Launcher.Modloader;

namespace Packsly3.Core.Launcher.Instance {

    public interface IMinecraftInstance {

        DirectoryInfo Location { get; }

        string Id { get; }

        string Name { set; get; }

        string MinecraftVersion { set; get; }

        Icon Icon { get; }

        ModLoaderManager ModLoaderManager { get; }

        void GetEnvironmentVariables(Dictionary<string, string> map);

        void Configure(string json);

        void Save();

        void Delete();

    }

}
