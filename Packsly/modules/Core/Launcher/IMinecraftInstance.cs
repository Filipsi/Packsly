namespace Packsly.Core.Launcher {

    public interface IMinecraftInstance {

        string Id               { get; }

        string Name             { set; get; }

        string Location         { get; }

        string MinecraftVersion { get; }

        string Icon             { set; get; }

        void Save();

        void Delete();

    }

}
