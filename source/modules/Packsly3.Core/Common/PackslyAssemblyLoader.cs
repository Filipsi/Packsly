using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Packsly3.Core.Common {

    internal static class PackslyAssemblyLoader {

        public static Assembly[] LoadedAssemblies;

        private static readonly DirectoryInfo Root = new DirectoryInfo(Directory.GetCurrentDirectory());

        static PackslyAssemblyLoader() {
            // Search for Packsly dlls in workspace
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            FileInfo[] toLoad = Root
                .GetFiles("Packsly3.*.dll")
                .Where(f => assemblies.All(a => a.Location != f.FullName))
                .ToArray();

            // Load Packsly dlls
            List<Assembly> loaded = toLoad
                .Select(dll => Assembly.LoadFrom(dll.FullName))
                .ToList();

            // Add this assembly to the list
            loaded.Add(Assembly.GetExecutingAssembly());

            LoadedAssemblies = loaded.ToArray();
        }

    }

}
