using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Common.Registry {

    public interface IRegistry<T> {

        void Register(params T[] elements);

    }

}
