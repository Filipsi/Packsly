using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.CommandEngine {

    internal partial class Command {

        public string               Name { private set; get; }
        public List<ArgumentMap>    ArgumentMaps { private set; get; }

        public Command(string name) {
            ArgumentMaps = new List<ArgumentMap>();
            Name = name;
        }

        public Command WithArguments(string mapping, string description, Action<Dictionary<string, string>> executor) {
            ArgumentMaps.Add(new ArgumentMap(mapping, description, executor));
            return this;
        } 

        public void Execute(string[] arguments) {
            foreach(ArgumentMap map in ArgumentMaps)
                if(map.Execute(arguments))
                    break;
        }

        public void PrioritizeMappings() {
            ArgumentMaps = ArgumentMaps.OrderByDescending(m => m.Priority).ToList();
        }

    }

}
