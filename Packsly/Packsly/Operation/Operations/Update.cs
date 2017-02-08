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
            Console.WriteLine(args["instance"]);
            MultiMCInstance current = MultiMCInstance.FromFile(Path.Combine(args["instance"], "instance.packsly"));
            current.Update();
        }

    }

}
