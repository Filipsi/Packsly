using Newtonsoft.Json.Linq;
using Packsly3.Core.Launcher.Instance;

namespace Packsly3.Core.Launcher.Adapter {

    public interface IAdapter {

        string Id { get; }

        bool IsCompatible(IMinecraftInstance instance);

        bool IsCompatible(string lifecycleEvent);

        void Execute(JObject config, string lifecycleEvent, IMinecraftInstance instance);

    }

    public abstract class Adapter<T> : IAdapter {

        #region IAdapter

        public abstract string Id { get; }

        public abstract bool IsCompatible(IMinecraftInstance instance);

        public abstract bool IsCompatible(string lifecycleEvent);

        public void Execute(JObject config, string lifecycleEvent, IMinecraftInstance instance)
            => Execute(config.ToObject<T>(), lifecycleEvent, instance);

        #endregion

        public abstract void Execute(T config, string lifecycleEvent, IMinecraftInstance instance);

    }

    public abstract class InstanceAdapter<TC, TI> : Adapter<TC> {

        public override bool IsCompatible(IMinecraftInstance instance)
            => instance.GetType() == typeof(TI);

    }

}

