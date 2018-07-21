using System.Collections.Generic;
using CommandLine;

namespace Packsly3.Cli.Verbs {

    [Verb("lifecycle", HelpText = "Dispatches one or more lifecycle events for specified minecraft instance")]
    internal class LifecycleOptions {

        [Value(0, Required = true, HelpText = "Name of minecraft instance to which are all dispatched events related to")]
        public string InstanceName { get; set; }

        [Value(1, Min = 1, HelpText = "Name of the events that should be dispatched")]
        public IEnumerable<string> Events { get; set; }

    }
}
