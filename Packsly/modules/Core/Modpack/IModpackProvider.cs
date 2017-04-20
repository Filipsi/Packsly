using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Modpack {

    public interface IModpackProvider {

        bool CanUseSource(string source);

        ModpackInfo Create(string source);

    }

}
