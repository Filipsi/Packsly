using Packsly.CommandEngine;
using Packsly.Operation.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Packsly.Operation {

    internal class OperationManager : CommandManager {

        protected override void Initialize() {
            RegisterCommand("help")
                .WithArguments(string.Empty, "Print help", (args) => {
                    const short lName = 10, lArgs = 20;

                    foreach(Command c in this) {
                        Console.Write("-" + c.Name);
                        Console.Write(new string(' ', lName - c.Name.Length - 1));

                        if(c.ArgumentMaps != null)
                            for(short i = 0; i < c.ArgumentMaps.Count; i++) {
                                Command.ArgumentMap argument = c.ArgumentMaps[i];

                                if(i > 0)
                                    Console.Write(new string(' ', lName));

                                Console.Write(argument.Mapping);
                                Console.Write(new string(' ', lArgs - argument.Mapping.Length));
                                Console.Write(argument.Description);
                                Console.WriteLine();
                            }

                        Console.WriteLine();
                    }
                });

            RegisterCommand("set")
                .WithArguments("[property] [value]", "Set configuration valiable to specified value", OperationCollection.Set);

            RegisterCommand("seek")
                .WithArguments("[url]", "Search for modpacks collection at specified url", OperationCollection.Seek);

            RegisterCommand("install")
                .WithArguments("[url] [packname]", "Install specified modpack from specified url to local MultiMC instance", OperationCollection.InstallFromUrl)
                .WithArguments("seek [packname]", "Install specified modpack from last seeked url to local MultiMC instance", OperationCollection.InstallFromSeek);

            RegisterCommand("update")
                .WithArguments("[instance]", "Update instance using it's 'instance.packsly' file at specified absolute path", OperationCollection.UpdateFromDesciptor)
                .WithArguments("instance [name]", "Will search for instance with specified name and update it", OperationCollection.UpdateInstace);
        }

    }
}
