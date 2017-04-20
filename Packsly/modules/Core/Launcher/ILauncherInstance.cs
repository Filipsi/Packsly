namespace Packsly.Core.Launcher {

    public interface ILauncherInstance {

        string Id { get; }

        string Name { get; }

        string Location { get; }

        string MinecraftVersion { get; }

        void Save();

        void Delete();

    }

}
