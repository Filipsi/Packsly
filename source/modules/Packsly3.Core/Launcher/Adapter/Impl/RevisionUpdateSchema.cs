using System;
using Newtonsoft.Json;
using Packsly3.Core.Common;
using Packsly3.Core.Launcher.Instance;

namespace Packsly3.Core.Launcher.Adapter.Impl {

    [Register]
    public class RevisionUpdateSchema : Adapter<RevisionUpdateSchemaConfig> {

        public override string Id { get; }
            = "revisionBasedUpdater";

        public override bool IsCompatible(IMinecraftInstance instance)
            => true;

        public override bool IsCompatible(string lifecycleEvent)
            => lifecycleEvent == Lifecycle.PreLaunch;

        public override void Execute(RevisionUpdateSchemaConfig config, string lifecycleEvent, IMinecraftInstance instance) {
            throw new NotImplementedException();
        }

    }

    [JsonObject(MemberSerialization.OptIn)]
    public class RevisionUpdateSchemaConfig {

        [JsonProperty("url")]
        public Uri UpdateUrl { private set; get; }

        [JsonProperty("revision")]
        public int Revision { private set; get; }

    }

}
