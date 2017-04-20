using Packsly.Core.Common;
using Packsly.Core.Common.Configuration;
using Packsly.Core.Common.Factory;
using Packsly.Core.Common.Registry;
using Packsly.Core.Modpack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Launcher {

    public class LauncherInstanceFactory : SingleTypeRegistry<ILauncherSchema>, IFactory<ILauncherInstance, string> {

        public ILauncherSchema CurrentLauncher {
            get {
                return modules.Find(s => s.IsPresent(Settings.Instance.Launcher));
            }
        }

        public ILauncherInstance BuildFrom(string source) {
            ILauncherSchema launcher = CurrentLauncher;

            if(launcher == null)
                throw new Exception("Was not able to create Minecraft instance from source, no compatible launcher found");

            ModpackInfo modpack = PackslyManager.BuildModpackInfo(source);

            if(launcher.GetInstances(Settings.Instance.Launcher).Any(i => i.Contains(modpack.Id)))
                throw new Exception("Minecraft instance with the same id allready exists");

            return launcher.Create(modpack);
        }

    }

}
