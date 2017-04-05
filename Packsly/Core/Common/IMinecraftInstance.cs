using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Common {

    public interface IMinecraftInstance {

        string Id { get; }

        string Name { get; }

        string Location { get; }

        string LauncherLocation { get; }

        string MinecraftVersion { get; }

        void Save();

        void Delete();

    }

}
