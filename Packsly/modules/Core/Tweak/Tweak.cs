using Packsly.Core.Launcher;
using Packsly.Core.Tweak;
using System;

namespace Packsly.Core.Tweak {

    public abstract class Tweak<T, A> : ITweak where T : ILauncherInstance where A : ITweakArguments {

        #region ITweak

        Type ITweak.MinecraftInstanceType {
            get {
                return typeof(T);
            }
        }

        void ITweak.Execute(ILauncherInstance instance, ITweakArguments args) {
            Execute((T)instance, (A)args);
        }

        #endregion

        #region Abstracts

        public abstract string Type { get; }

        public abstract void Execute(T instance, A args);

        #endregion

    }

}
