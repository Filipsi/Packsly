﻿using System;
using System.IO;
using CommandLine;

namespace Packsly3.Cli.Verbs {

    [Verb("install", HelpText = "Installs modpack into current workspace from config file or url address")]
    internal class InstallOptions {

        [Value(0, Required = true, HelpText = "Modpack source (file path or url address)")]
        public string Source { get; set; }

        [Option('e', "environment", HelpText = "Manually override launcher auto detection and force environment usage by specifying handler's name")]
        public string Environment { get; set; }

        [Option('w', "workspace", HelpText = "Set workspace by specifying path to new workspace folder")]
        public string Workspace { get; set; }

        #region Helpers

        public bool IsEnvironmentSpecified => !string.IsNullOrEmpty(Environment);

        public bool IsWorkspaceSpecified => !string.IsNullOrEmpty(Workspace);

        public bool IsWorkspaceValid => Directory.Exists(Workspace);

        public bool IsSourceUrl => Uri.IsWellFormedUriString(Source, UriKind.Absolute);

        public bool IsSourceLocalFile => File.Exists(Source);

        #endregion

    }

}