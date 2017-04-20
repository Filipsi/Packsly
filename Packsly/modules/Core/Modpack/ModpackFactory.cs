using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Modpack {

    public static class ModpackFactory {

        private static List<IModpackProvider> _providers = new List<IModpackProvider>();

        public static void RegisterProvider(IModpackProvider provider) {
            if(!_providers.Contains(provider))
                _providers.Add(provider);
        }

        public static Modpack Acquire(string source) {
            IModpackProvider provider = _providers.Find(m => m.CanUseSource(source));

            if(provider == null)
                throw new Exception($"Failed while processing source from '{source}'. No usable provider found.");

            return provider.Create(source);
        }

    }

}
