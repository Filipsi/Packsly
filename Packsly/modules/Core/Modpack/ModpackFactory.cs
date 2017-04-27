﻿using Packsly.Core.Common.Factory;
using Packsly.Core.Common.Registry;
using Packsly.Core.Modpack.Provider;
using System;

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
