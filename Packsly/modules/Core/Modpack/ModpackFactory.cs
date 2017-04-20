using Packsly.Core.Common.Factory;
using Packsly.Core.Common.Registry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Modpack {

    public class ModpackFactory : SingleTypeRegistry<IModpackProvider>, IFactory<ModpackInfo, string> {

        public ModpackInfo BuildFrom(string source) {
            IModpackProvider provider = modules.Find(m => m.CanUseSource(source));

            if(provider == null)
                throw new Exception($"Failed while processing source from '{source}'. No usable provider found.");

            return provider.Create(source);
        }

    }

}
