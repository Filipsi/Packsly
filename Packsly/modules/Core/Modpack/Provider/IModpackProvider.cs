using Packsly.Core.Modpack.Model;

namespace Packsly.Core.Modpack.Provider {

    public interface IModpackProvider {

        bool CanUseSource(string source);

        ModpackInfo Create(string source);

    }

}
