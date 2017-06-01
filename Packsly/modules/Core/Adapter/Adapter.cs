using Packsly.Core.Launcher;
using System;

namespace Packsly.Core.Adapter {

    public abstract class Adapter {

        internal abstract Type MinecraftInstaceType { get; }

        internal abstract Type ExecutionContextType { get; }

        public abstract void Execute(IMinecraftInstance instance, IAdapterContext context);

    }

    public abstract class Adapter<I, C> : Adapter where I : IMinecraftInstance where C : IAdapterContext {

        #region Logic

        internal override Type MinecraftInstaceType {
            get {
                return typeof(I);
            }
        }

        internal override Type ExecutionContextType {
            get {
                return typeof(C);
            }
        }

        public override void Execute(IMinecraftInstance instance, IAdapterContext context) {
            Execute((I)instance, (C)context);
        }

        #endregion

        #region Abstracts

        protected abstract void Execute(I instance, C context);

        #endregion

    }

}
