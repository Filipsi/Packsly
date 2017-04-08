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

        public static Modpack FromSource(string source) {
            IModpackProvider provider = _providers.Find(m => m.CanUseSource(source));
            return provider == null ? null : provider.Create(source);
        }

    }

}
