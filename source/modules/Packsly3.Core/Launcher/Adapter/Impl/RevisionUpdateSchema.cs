using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Packsly3.Core.Common;
using Packsly3.Core.FileSystem;
using Packsly3.Core.Launcher.Instance;
using Packsly3.Core.Modpack;

namespace Packsly3.Core.Launcher.Adapter.Impl {

    public class RevisionUpdateSchema : ConfigurableAdapter<RevisionUpdateSchemaConfig> {

        public override Lifecycle[] Triggers { get; } = {
            Lifecycle.PostInstallation,
            Lifecycle.PreLaunch
        };

        public override void Execute(Lifecycle trigger, RevisionUpdateSchemaConfig adapterConfig, IMinecraftInstance instance) {
            // TODO
        }

    }

    [JsonObject(MemberSerialization.OptIn)]
    public class RevisionUpdateSchemaConfig {

        [JsonProperty("url")]
        public Uri UpdateUrl { private set; get; }

        [JsonProperty("revision")]
        public int Revision { private set; get; } = 42;

    }

}
