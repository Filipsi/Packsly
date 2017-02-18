using Packsly.Configuration;
using Packsly.MultiMC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Operation.Operations {

    internal static partial class OperationCollection {

        public static void UpdateFromDesciptor(Dictionary<string, string> args) {
            MultiMCInstance
                .FromFile(Path.Combine(args["instance"], "instance.packsly"))
                .Update();
        }

        public static void UpdateInstace(Dictionary<string, string> args) {
            if(Config.Current.MultiMC == string.Empty || !Directory.Exists(Config.Current.MultiMC))
                throw new Exception("MultiMC directory is not specified or does not exists, use --set multimc [path] to specify working directory");

            string instances = Path.Combine(Config.Current.MultiMC, "instances");

            foreach(string instance in Directory.EnumerateDirectories(instances))
                if(Path.GetFileName(instance) == args["name"]) {
                    MultiMCInstance
                        .FromFile(Path.Combine(instance, "instance.packsly"))
                        .Update();

                    return;
                }

            throw new Exception(string.Format("Instance with name '{0}' does not exist in 'MultiMC/instnaces' directory", args["name"], instances));
        }

    }

}
