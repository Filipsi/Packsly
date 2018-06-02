using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Packsly3.Core.Launcher.Instance {

    public class EnvironmentVariables {

        public static readonly string ModsFolder = "modsFolder";
        public static readonly string ConfigFolder = "configFolder";
        public static readonly string InstnaceFolder = "instanceFolder";
        public static readonly string MinecraftFolder = "minecraftFolder";

        private static readonly Regex PattenNamedParameter = new Regex("{([^}]+)}", RegexOptions.Compiled);

        public readonly ReadOnlyDictionary<string, string> Map;

        public EnvironmentVariables(IMinecraftInstance instance) {
            Dictionary<string, string> map = new Dictionary<string, string> {
                { ModsFolder,      Path.Combine(instance.Location.FullName, "mods")      },
                { ConfigFolder,    Path.Combine(instance.Location.FullName, "config")    },
                { InstnaceFolder,  instance.Location.FullName                            },
                { MinecraftFolder, Path.Combine(instance.Location.FullName, "minecraft") }
            };

            instance.GetEnvironmentVariables(map);
            Map = new ReadOnlyDictionary<string, string>(map);
        }

        public string Format(string input) {
            List<string> namedParameters = new List<string>();
            MatchCollection matches = PattenNamedParameter.Matches(input);
            for (int i = 0; i < matches.Count; i++) {
                Match match = matches[i];
                namedParameters.Add(match.Groups[1].Value);
            }

            Dictionary<string, string> localMap = namedParameters
                .Where(namedParameter => Map.ContainsKey(namedParameter))
                .ToDictionary(namedParameter => namedParameter, namedParameter => Map[namedParameter]);

            return localMap.Aggregate(input, (current, parameter) => current.Replace("{" + parameter.Key + "}", parameter.Value.ToString()));
        }

    }

}
