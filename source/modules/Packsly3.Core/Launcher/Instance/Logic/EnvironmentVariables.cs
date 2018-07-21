using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Packsly3.Core.Launcher.Instance.Logic {

    public class EnvironmentVariables {

        public static readonly string ModsFolder = "modsFolder";

        public static readonly string ConfigFolder = "configFolder";

        public static readonly string InstnaceFolder = "instanceFolder";

        public static readonly string MinecraftFolder = "minecraftFolder";

        private static readonly Regex PattenNamedParameter = new Regex("{([^}]+)}", RegexOptions.Compiled);

        public readonly ReadOnlyDictionary<string, string> Properties;

        public EnvironmentVariables(IMinecraftInstance instance, IDictionary<string, string> properties) {
            Properties = new ReadOnlyDictionary<string, string>(properties);

            if (!properties.ContainsKey(ModsFolder)) {
                properties.Add(ModsFolder, Path.Combine(instance.Location.FullName, "mods"));
            }

            if (!properties.ContainsKey(ConfigFolder)) {
                properties.Add(ConfigFolder, Path.Combine(instance.Location.FullName, "config"));
            }

            if (!properties.ContainsKey(InstnaceFolder)) {
                properties.Add(InstnaceFolder, instance.Location.FullName);
            }

            if (!properties.ContainsKey(MinecraftFolder)) {
                properties.Add(MinecraftFolder, Path.Combine(instance.Location.FullName, "minecraft"));
            }
        }

        public string GetProperty(string name) {
            return Properties.ContainsKey(name) ? Properties[name] : string.Empty;
        }

        public string Format(string input) {
            List<string> namedParameters = new List<string>();
            MatchCollection matches = PattenNamedParameter.Matches(input);
            for (int i = 0; i < matches.Count; i++) {
                Match match = matches[i];
                namedParameters.Add(match.Groups[1].Value);
            }

            Dictionary<string, string> localMap = namedParameters
                .Where(namedParameter => Properties.ContainsKey(namedParameter))
                .ToDictionary(namedParameter => namedParameter, namedParameter => Properties[namedParameter]);

            return localMap.Aggregate(input, (current, parameter) => current.Replace("{" + parameter.Key + "}", parameter.Value.ToString()));
        }

    }

}
