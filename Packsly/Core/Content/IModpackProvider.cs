using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Content {

    public interface IModpackProvider {

        bool CanUseSource(string source);

        Modpack Create(string source);

    }

}
