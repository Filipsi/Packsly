namespace Packsly.Core.Launcher {

    public interface IMinecraftInstance {

        #region Properties

        string Id { get; }

        string Name { get; }

        string Location { get; }

        string MinecraftVersion { get; }

        #endregion

        #region Logic

        void Save();

        void Delete();

        #endregion

    }

}
