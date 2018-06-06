namespace Packsly3.Core.Launcher.Modloader {

    public class ModLoaderInfo {

        public string Name { get; }

        public string Version { get; }

        public ModLoaderManager Manager { get; }

        public ModLoaderInfo(ModLoaderManager manager, string name, string version) {
            Manager = manager;
            Name = name;
            Version = version;
        }

        public void Uninstall()
            => Manager.Uninstall(Name);

    }

}
