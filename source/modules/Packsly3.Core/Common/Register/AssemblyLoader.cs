using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NLog;

namespace Packsly3.Core.Common.Register {

    internal static class AssemblyLoader {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static readonly Assembly[] LoadedAssemblies;

        static AssemblyLoader() {
            // Search for Packsly libraries in the workspace
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            FileInfo[] toLoad = Packsly.AplicationDirectory
                .GetFiles("Packsly3.*.dll")
                .Where(f => assemblies.All(a => a.Location != f.FullName))
                .ToArray();

            // Load Packsly libraries
            List<Assembly> loaded = toLoad
                .Select(dll => Assembly.LoadFrom(dll.FullName))
                .ToList();

            // Add this assembly to the list
            loaded.Add(Assembly.GetExecutingAssembly());
            LoadedAssemblies = loaded.ToArray();
            Logger.Debug($"Loaded assemblies: {string.Join(", ", LoadedAssemblies.Select(assembly => assembly.GetName().Name))}");
        }

    }

}
