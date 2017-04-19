using Packsly.Core.Configuration;
using Packsly.Core.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Launcher {

    public static class MinecraftInstanceFactory {

        private static List<ILauncherSchema> _schemas = new List<ILauncherSchema>();

        public static ILauncherSchema CurrentLauncher {
            get {
                return _schemas.Find(s => s.Check(Settings.Instance.Launcher));
            }
        }

        public static void RegisterSchema(ILauncherSchema schema) {
            if(!_schemas.Contains(schema))
                _schemas.Add(schema);
        }

        public static IMinecraftInstance CreateFrom(string source) {
            ILauncherSchema launcher = CurrentLauncher;

            if(launcher == null)
                throw new Exception("Was not able to create Minecraft instance from source, no compatible launcher found");

            return launcher.Create(ModpackFactory.Acquire(source));
        }

    }

}
