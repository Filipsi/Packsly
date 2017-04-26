using Packsly.Core.Launcher;
using System;

namespace Packsly.Core.Tweaker {

    public abstract class Adapter {

        internal abstract Type MinecraftInstaceType { get; }

        internal abstract Type ExecutionContextType { get; }

        public abstract void Execute(IMinecraftInstance instance, IExecutionContext context);

    }

    public abstract class Adapter<I, C> : Adapter where I : IMinecraftInstance where C : IExecutionContext {

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

        public override void Execute(IMinecraftInstance instance, IExecutionContext context) {
            Execute((I)instance, (C)context);
        }

        #endregion

        #region Abstracts

        protected abstract void Execute(I instance, C context);

        #endregion

    }

}
