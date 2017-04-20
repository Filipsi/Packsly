namespace Packsly.Core.Launcher {

    public interface IMinecraftInstance {

        string Id { get; }

        string Name { get; }

        string Location { get; }

        string MinecraftVersion { get; }

        void Save();

        void Delete();

    }

}
