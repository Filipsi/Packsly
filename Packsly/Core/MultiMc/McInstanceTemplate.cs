using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.MultiMc {

    public abstract class McInstanceTemplate {

        #region Public

        public abstract string Name { get; }

        public abstract string Location { get; }

        public abstract string MinecraftVersion { get; }

        #endregion

        #region IO

        public abstract void Save();

        public abstract void Delete();

        #endregion

    }

}
