using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Instance {

    public abstract class MinecraftInstance {

        public string Name { private set; get; }

        public string Location { private set; get; }

        public string MinecraftVersion { private set; get; }

        public MinecraftInstance(string name, string location, string mcversion) {
            Name = name;
            Location = location;
            MinecraftVersion = mcversion;
        }

    }

}
