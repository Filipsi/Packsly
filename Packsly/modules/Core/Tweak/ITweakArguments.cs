using Packsly.Core.Tweak;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Tweak {

    public interface ITweakArguments {

        bool IsCompatible(ITweak tweak);

    }

}
