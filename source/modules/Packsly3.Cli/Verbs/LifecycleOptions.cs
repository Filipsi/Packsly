using System.Collections.Generic;
using CommandLine;

namespace Packsly3.Cli.Verbs {

    [Verb("lifecycle", HelpText = "Dispatches one or more lifecycle events for specified minecraft instance")]
    internal class LifecycleOptions {

        [Value(0, Required = true, HelpText = "Identifier of minecraft instance to which are all dispatched events related to")]
        public string InstanceId { get; set; }

        [Value(1, Min = 1, HelpText = "Event names that will be dispatched")]
        public IEnumerable<string> Events { get; set; }

    }
}
