using Packsly.Core.Content;

namespace Packsly.Launcher {

    public interface IMinecraftInstance {

        string Id { get; }

        string Name { get; }

        string Location { get; }

        string LauncherLocation { get; }

        string MinecraftVersion { get; }

        void Save();

        void Delete();

    }

}
