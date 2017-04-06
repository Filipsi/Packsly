using Packsly.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Module {

    public abstract class Module<T, A> : IModule where T : IMinecraftInstance {

        Type IModule.InstanceType {
            get {
                return typeof(T);
            }
        }

        Type IModule.ArgumentType {
            get {
                return typeof(A);
            }
        }

        void IModule.Execute(IMinecraftInstance instance, object args) {
            Execute((T)instance, (A)args);
        }

        public abstract string Id { get; }

        public abstract void Execute(T instance, A args);

    }

}
