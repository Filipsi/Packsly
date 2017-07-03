﻿using Newtonsoft.Json;
using Packsly.Core.Modpack;
using Packsly.Core.Modpack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Adapter.Update {

    public class UpdateAdapterContext : IAdapterContext {

        [JsonProperty(PropertyName = "source")]
        public readonly string UpdateSource;

        internal readonly ModpackInfo Modpack;

        public UpdateAdapterContext(string source, ModpackBuilder builder) {
            UpdateSource = source;
            Modpack = builder.Instace;
        }

    }

}
