using Packsly.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Forge {

    public interface IForgeInstallationSchema {

        Type Type { get; }

        bool Install(ForgeInstaller installer, IMinecraftInstance mcinstance, string forgeVersion);

    }

}
