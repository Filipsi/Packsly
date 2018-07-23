using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NLog;

namespace Packsly3.Core.Common.Register {

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class RegisterAttribute : Attribute {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static T[] GetOccurrencesFor<T>() {
            List<T> occurrences = new List<T>();

            foreach (Assembly assembly in AssemblyLoader.LoadedAssemblies) {
                foreach (Type type in assembly.GetTypes()) {
                    object[] attributes = type.GetCustomAttributes(typeof(RegisterAttribute), false);

                    occurrences
                        .AddRange(attributes.Where(unused => typeof(T).IsAssignableFrom(type))
                        .Select(unused => (T) Activator.CreateInstance(type)));
                }
            }

            Logger.Debug($"Available implementations of {typeof(T).Name}: {string.Join(", ", occurrences.Select((occurrence) => occurrence.GetType().Name))}");
            return occurrences.ToArray();
        }

    }

}

