using Packsly.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Operation.Operations {

    internal static partial class OperationCollection {

        public static void Set(Dictionary<string, string> args) {
            string prop = args["property"];
            string val = args["value"];

            switch(prop) {
                case "multimc":
                    if(!Directory.Exists(val))
                        throw new Exception("Directory does not exits or path is not valid");

                    Config.Current.MultiMC = val;
                    Config.Save();

                    Console.WriteLine("MultiMC path was set to {0}", val);
                    break;

                default:
                    throw new Exception(string.Format("Invalid property name '{0}'", prop));
            }

        }

    }

}
