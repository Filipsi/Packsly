using Newtonsoft.Json.Linq;
using Packsly3.Core.FileSystem.Impl;
using Packsly3.Core.Launcher.Instance;

namespace Packsly3.Core.Launcher.Adapter {

    internal interface IAdapter {

        string Id { get; }

        bool IsCompatible(IMinecraftInstance instance);

        bool IsCompatible(string lifecycleEvent);

        void Execute(JObject config, string lifecycleEvent, IMinecraftInstance instance);

        void Save(IMinecraftInstance instance, object config);

    }

    public abstract class Adapter<TConfig> : IAdapter {

        #region IAdapter

        public abstract string Id { get; }

        public abstract bool IsCompatible(IMinecraftInstance instance);

        public abstract bool IsCompatible(string lifecycleEvent);

        public void Execute(JObject config, string lifecycleEvent, IMinecraftInstance instance) {
            Execute((TConfig)config?.ToObject(typeof(TConfig)), lifecycleEvent, instance);
        }

        #endregion

        public abstract void Execute(TConfig config, string lifecycleEvent, IMinecraftInstance instance);

        public void Save(IMinecraftInstance instance, object config) {
            PackslyInstanceFile cfg = instance.PackslyConfig;
            cfg.Adapters.SetConfigFor(this, config);
            cfg.Save();
        }

    }

    public abstract class InstanceAdapter<TConfig, TInstance> : Adapter<TConfig> {

        public override bool IsCompatible(IMinecraftInstance instance) {
            return instance.GetType() == typeof(TInstance);
        }

    }

}

