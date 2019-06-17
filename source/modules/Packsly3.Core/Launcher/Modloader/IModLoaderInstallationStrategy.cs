using Packsly3.Core.Launcher.Instance;

namespace Packsly3.Core.Launcher.Modloader {

    public interface IModLoaderInstallationStrategy<in T> where T : IMinecraftInstance {

        string Name { get; }

        string Identificator { get; }

        void Install(T instance, string version);

        void Uninstall(T instance);

    }

}
