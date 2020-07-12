using System.Collections.Generic;
using System.IO;
using CommandLine;

namespace Packsly3.Cli.Verbs {

    [Verb("lifecycle", HelpText = "Dispatches one or more lifecycle events for specified minecraft instance")]
    internal class LifecycleOptions {

        [Value(0, Required = true, HelpText = "Identifier of minecraft instance to which are all dispatched events related to")]
        public string InstanceId { get; set; }

        [Value(1, Min = 1, HelpText = "Event names that will be dispatched")]
        public IEnumerable<string> Events { get; set; }

        [Option('e', "environment", HelpText = "Override launcher auto detection and force environment usage by specifying handler's name")]
        public string Environment { get; set; }

        [Option('w', "workspace", HelpText = "Override workspace set in local configuration by specifying path to new workspace folder")]
        public string Workspace { get; set; }

        #region Helpers

        public bool IsEnvironmentSpecified => !string.IsNullOrEmpty(Environment);

        public bool IsWorkspaceSpecified => !string.IsNullOrEmpty(Workspace);

        public bool IsWorkspaceValid => Directory.Exists(Workspace);

        #endregion

    }
}
