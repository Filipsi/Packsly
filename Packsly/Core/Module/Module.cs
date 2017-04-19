using Packsly.Core.Module;
using Packsly.Launcher;
using System;

namespace Packsly.Core.Module {

    public abstract class Module<T, A> : IModule where T : IMinecraftInstance where A : IModuleArguments {

        #region IModule

        Type IModule.MinecraftInstanceType {
            get {
                return typeof(T);
            }
        }

        void IModule.Execute(IMinecraftInstance instance, IModuleArguments args) {
            Execute((T)instance, (A)args);
        }

        #endregion

        #region Abstracts

        public abstract string Type { get; }

        public abstract void Execute(T instance, A args);

        #endregion

    }

}
