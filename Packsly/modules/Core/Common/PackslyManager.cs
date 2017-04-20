using Packsly.Core.Common.Factory;
using Packsly.Core.Common.Registry;
using Packsly.Core.Launcher;
using Packsly.Core.Modpack;
using Packsly.Core.Tweak;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Common {

    public static class PackslyManager {

        #region Registries

        private static readonly ModpackFactory _modpackFactory = new ModpackFactory();

        private static readonly LauncherInstanceFactory _launcherInstanceFactory = new LauncherInstanceFactory();

        private static readonly IRegistry<ITweak> _tweakRegistry = new TweakRegistry();

        #endregion

        #region Logic

        public static void Register(params ITweak[] elements) {
            _tweakRegistry.Register(elements);
        }

        public static void Register(params IModpackProvider[] elements) {
            _modpackFactory.Register(elements);
        }

        public static void Register(params ILauncherSchema[] elements) {
            _launcherInstanceFactory.Register(elements);
        }

        public static ModpackInfo BuildModpackInfo(string source) {
            return _modpackFactory.BuildFrom(source);
        }

        public static ILauncherInstance CreateLauncherInstance(string source) {
            return _launcherInstanceFactory.BuildFrom(source);
        }

        #endregion

    }

}
