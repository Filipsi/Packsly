using Packsly.Core.Common.Configuration;
using Packsly.Core.Launcher;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Adapter.Override {

    public class OverrideAdapter : Adapter<IMinecraftInstance, OverrideAdapterContext> {

        protected override void Execute(IMinecraftInstance instance, OverrideAdapterContext context)  {

            if(!Directory.Exists(context.OverrideFilesLocation))
                return;

            // Replace files in IMinecraftInstance location with override files from context
            foreach(string file in context.Overrides) {
                string destination = Path.Combine(instance.Location, "minecraft", file.Replace(Settings.Instance.Temp.FullName + @"\", string.Empty));
                Directory.CreateDirectory(Path.GetDirectoryName(destination));
                File.Copy(Path.Combine(context.OverrideFilesLocation, file), destination);
            }

        }

    }

}
