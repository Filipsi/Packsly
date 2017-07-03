using Packsly.Core.Common;
using Packsly.Core.Common.FileSystem;
using Packsly.Core.Launcher;
using Packsly.Core.Modpack;
using Packsly.Core.Modpack.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Adapter.Update {

    public class UpdateAdapter : Adapter<IMinecraftInstance, UpdateAdapterContext> {

        protected override void Execute(IMinecraftInstance instance, UpdateAdapterContext context) {

            if(string.IsNullOrEmpty(context.UpdateSource))
                return;

            string location = Path.Combine(instance.Location, "instance.packsly.json");

            // If there is no modpack file, create it and stop update procedure
            // This is probably when modpack is installed
            if(!File.Exists(location)) {
                if(context.Modpack == null)
                    throw new Exception($"Packsly file 'instance.packsly.json' is missing in {instance.Name} instace.");

                context.Modpack.Save(location);
                return;
            }

            ModpackInfo oldModpack = new ModpackInfo().Load(location);
            ModpackInfo sourceModpack = PackslyFactory.Modpack.BuildFrom(context.UpdateSource);

            // Update instance icon
            if(sourceModpack.Icon != instance.Icon)
                instance.Icon = sourceModpack.Icon;

            // Update instance name
            if(sourceModpack.Name != instance.Name)
                instance.Name = sourceModpack.Name;

            // If there is version mismatch, update all mods and config files
            if(sourceModpack.Version != oldModpack.Version) {

            }

            // Save new ModpackInfo as packsly file and instance changes
            // sourceModpack.Save(instance.Location);
            instance.Save();

        }

    }

}
