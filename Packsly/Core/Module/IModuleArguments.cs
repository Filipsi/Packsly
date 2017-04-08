using Packsly.Core.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Module {

    public interface IModuleArguments {

        bool IsCompatible(IModule module);

    }

}
