using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.CommandEngine {

    internal abstract class CommandManager : IEnumerable {

        private Dictionary<string, Command> _commands;

        public CommandManager() {
            _commands = new Dictionary<string, Command>();

            Initialize();

            foreach(Command command in this)
                command.PrioritizeMappings();
        }

        protected abstract void Initialize();

        protected Command RegisterCommand(string name) {
            if(_commands.ContainsKey(name))
                throw new Exception("Command manager already has command with the same name");

            Command command = new Command(name);
            _commands.Add(name, command);
            return command;
        }

        public void Execute(string[] args) {
            if(args == null || args.Length == 0)
                return;

            string currentCommand = null;
            List<string> currentArguments = new List<string>();

            for(int i = 0; i < args.Length; i++) {
                // Search for command
                if(args[i].First().Equals('-')) {

                    // If there is currently no command been built, start building this one
                    if(currentCommand == null) {
                        currentCommand = args[i].TrimStart('-');
                        continue;

                    // but if there is, execute it and clear buffer
                    } else {
                        if(_commands.ContainsKey(currentCommand))
                            _commands[currentCommand].Execute(currentArguments.ToArray());

                        currentCommand = null;
                        currentArguments.Clear();
                    }

                // if not command, consider it to be argument for current command
                } else {
                    if(currentCommand != null)
                        currentArguments.Add(args[i]);
                }
            }

            if(_commands.ContainsKey(currentCommand))
                _commands[currentCommand].Execute(currentArguments.ToArray());
        }

        // IEnumerable

        public IEnumerator GetEnumerator() {
            return _commands.Values.GetEnumerator();
        }

    }

}
