using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.CommandEngine {

    internal partial class Command {

        internal class ArgumentMap {

            public string   Mapping         { private set; get; }
            public string   Description     { private set; get; }
            public int      Priority        { private set; get; }

            private List<Argument>                      _arguments;
            private Action<Dictionary<string, string>>  _executor;
            private Dictionary<string, string>          _context;

            public ArgumentMap(string mapping, string description, Action<Dictionary<string, string>> executor) {
                Mapping = mapping;
                Description = description;
                _executor = executor;
                _context = new Dictionary<string, string>();

                _arguments = new List<Argument>();
                foreach(string descriptor in mapping.Split(' '))
                    _arguments.Add(new Argument(descriptor));

                Priority = 0;
                for(short i = 0; i < _arguments.Count; i++)
                    if(_arguments[i].IsLiteral)
                        Priority += _arguments.Count - i;
            }

            public bool Execute(string[] arguments) {
                if(arguments.Length < _arguments.Count(a => a.IsRequired))
                    return false;

                for(short i = 0; i < _arguments.Count; i++) {
                    Argument mapArg = _arguments[i];

                    if(mapArg.IsRequired && !mapArg.IsDataholder)
                        if(arguments[i] == mapArg.Name) {
                            continue;
                        } else {
                            _context.Clear();
                            return false;
                        }

                    if(mapArg.IsDataholder) 
                        _context.Add(mapArg.Name, arguments[i]);
                }

                _executor.Invoke(_context);
                _context.Clear();
                return true;
            }

        }

    }

}
