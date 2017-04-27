using Packsly.Core.Modpack;
using Packsly.Core.Launcher;

namespace Packsly.Core.Common.Factory {

    public static class PackslyFactory {

        public static readonly ModpackFactory Modpack = new ModpackFactory();

        public static readonly MinecraftInstanceFactory MinecraftInstance = new MinecraftInstanceFactory();


    }

}
