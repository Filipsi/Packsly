using System;
using Newtonsoft.Json.Linq;
using Packsly3.Core.Launcher.Instance;
using Packsly3.Core.Modpack;

namespace Packsly3.Core.Launcher.Adapter {

    public interface IAdapter {

        Lifecycle[] Triggers { get; }

        void Execute(Lifecycle trigger, IMinecraftInstance instance);

    }

    public abstract class ConfigurableAdapter<T> : IAdapter {

        #region IAdapter

        public abstract Lifecycle[] Triggers { get; }

        public void Execute(Lifecycle trigger, IMinecraftInstance instance)
            => Execute(trigger, default(T), instance);

        #endregion

        public abstract void Execute(Lifecycle trigger, T adapterConfig, IMinecraftInstance instance);

    }

}
