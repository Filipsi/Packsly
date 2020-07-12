using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NLog;

namespace Packsly3.Core.Launcher.Instance {

    public class EnvironmentVariables {

        #region Properties

        public readonly ReadOnlyDictionary<string, string> Properties;

        #endregion

        #region Fields

        public static readonly string ModsFolder = "modsFolder";
        public static readonly string ConfigFolder = "configFolder";
        public static readonly string InstnaceFolder = "instanceFolder";
        public static readonly string RootFolder = "rootFolder";

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly Regex pattenNamedParameter = new Regex("{([^}]+)}", RegexOptions.Compiled);

        #endregion

        public EnvironmentVariables(IMinecraftInstance instance, IDictionary<string, string> properties) {
            // Register defaults
            if (!properties.ContainsKey(ModsFolder)) {
                properties.Add(ModsFolder, Path.Combine(instance.Location.FullName, "mods"));
            }

            if (!properties.ContainsKey(ConfigFolder)) {
                properties.Add(ConfigFolder, Path.Combine(instance.Location.FullName, "config"));
            }

            if (!properties.ContainsKey(InstnaceFolder)) {
                properties.Add(InstnaceFolder, instance.Location.FullName);
            }

            if (!properties.ContainsKey(RootFolder)) {
                properties.Add(RootFolder, Path.Combine(instance.Location.FullName, "minecraft"));
            }

            // Order by longest value string (this is important when converting from paths)
            Properties = new ReadOnlyDictionary<string, string>(
                properties.OrderByDescending(prop => prop.Value.Length).ToDictionary(k => k.Key, v => v.Value)
            );

            logger.Debug($"Environment variables of minecraft instance {instance} were set to: {Environment.NewLine + string.Join(Environment.NewLine, Properties.Select(pair => $"{pair.Key} <-> {pair.Value}"))}");
        }

        public string GetProperty(string name) {
            return Properties.ContainsKey(name) ? Properties[name] : string.Empty;
        }

        public string ToFormatedString(string input) {
            MatchCollection matches = pattenNamedParameter.Matches(input);

            List<string> namedParameters = new List<string>();
            for (int i = 0; i < matches.Count; i++) {
                namedParameters.Add(matches[i].Groups[1].Value);
            }

            Dictionary<string, string> localMap = namedParameters
                .Where(namedParameter => Properties.ContainsKey(namedParameter))
                .ToDictionary(namedParameter => namedParameter, namedParameter => Properties[namedParameter]);

            string result = localMap.Aggregate(input, (current, parameter) => current.Replace("{" + parameter.Key + "}", parameter.Value));
            logger.Debug($"{input} -> {result}");
            return result;
        }

        public string FromFormatedString(string input) {
            string result = input;

            foreach (KeyValuePair<string, string> property in Properties) {
                if (!result.Contains(property.Value)) {
                    continue;
                }

                string propertyKey = "{" + property.Key + "}";
                result = result.Replace(property.Value, propertyKey);
            }

            logger.Debug($"{result} <- {input}");
            return result;
        }

    }

}
