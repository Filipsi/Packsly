using Packsly.Core.Common;
using Packsly.Core.Common.Configuration;
using Packsly.Core.Common.Factory;
using Packsly.Core.Common.Registry;
using Packsly.Core.Modpack;
using System;
using System.Linq;

namespace Packsly.Core.Launcher {

    public class LauncherInstanceFactory : SingleTypeRegistry<ILauncherSchema>, IFactory<IMinecraftInstance, string> {

        public ILauncherSchema CurrentLauncher {
            get {
                return modules.Find(s => s.IsPresent(Settings.Instance.Launcher));
            }
        }

        public IMinecraftInstance BuildFrom(string source) {
            ILauncherSchema launcher = CurrentLauncher;

            if(launcher == null)
                throw new Exception("Was not able to create Minecraft instance from source, no compatible launcher found");

            ModpackInfo modpack = PackslyFactory.Modpack.BuildFrom(source);

            if(launcher.GetInstances(Settings.Instance.Launcher).Any(i => i.Contains(modpack.Id)))
                throw new Exception("Minecraft instance with the same id allready exists");

            return launcher.Create(modpack);
        }

    }

}
