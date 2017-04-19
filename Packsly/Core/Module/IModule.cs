using Packsly.Core.Launcher;
using System;

namespace Packsly.Core.Module {

    public interface IModule {

        string Type { get; }

        Type MinecraftInstanceType { get; }

        void Execute(IMinecraftInstance instance, IModuleArguments args);

    }

}
