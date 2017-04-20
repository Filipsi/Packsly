using Packsly.Core.Launcher;
using System;

namespace Packsly.Core.Tweak {

    public interface ITweak {

        string Type { get; }

        Type MinecraftInstanceType { get; }

        void Execute(IMinecraftInstance instance, ITweakArguments args);

    }

}
