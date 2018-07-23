using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Packsly3.Core.Common.Json {

    public class RelativePathConverter : JsonConverter {

        public string Root { get; set; } = Packsly.AplicationDirectory.FullName;

        public override bool CanConvert(Type objectType)
            => objectType.IsSubclassOf(typeof(FileSystemInfo)) || objectType == typeof(FileSystemInfo) || objectType == typeof(string);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            string path = value is string valueString
                ? valueString
                : ((FileSystemInfo)value).FullName;

            JValue token = new JValue(path);
            if (path.StartsWith(Root)) {
                token.Value = $".{path.Remove(0, Root.Length)}";

            } else if (Root.StartsWith(path)) {
                string rootFragment = Root.Substring(path.Length, Root.Length - path.Length);
                int deep = rootFragment.Count(ch => ch == '\\');
                token.Value = $".{string.Concat(Enumerable.Repeat("\\..", deep))}";
            }

            token.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            if (objectType != typeof(FileInfo) && objectType != typeof(DirectoryInfo) && objectType != typeof(string))
                throw new NotSupportedException( $"{GetType().Name} does not support serialization of '{objectType}' type.");

            string path = JToken.Load(reader).Value<string>();

            if (string.IsNullOrEmpty(path)) {
                return null;
            }

            string resolvedPath = path.StartsWith(@".\")
                ? Path.Combine(Root, path.Remove(0, 2))
                : path;

            return objectType == typeof(string)
                ? resolvedPath
                : Activator.CreateInstance(objectType, resolvedPath);
        }

    }

}
