using Newtonsoft.Json;
using Packsly.Common;
using System;

namespace Packsly.Core.Module {

    public interface IModule {

        string Id { get; }

        Type InstanceType { get; }

        Type ArgumentType { get; }

        void Execute(IMinecraftInstance instance,  object args);

    }

}
