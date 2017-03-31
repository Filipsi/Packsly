using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Instance {

    public abstract class MinecraftInstance {

        public abstract string Name { get; }

        public abstract string Location { get; }

        public abstract string MinecraftVersion { get; }

        public abstract void Save();

        public abstract void Delete();

    }

}
