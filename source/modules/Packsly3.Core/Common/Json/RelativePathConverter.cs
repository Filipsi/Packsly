using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Packsly3.Core.Common.Json {

    internal class RelativePathConverter : JsonConverter {

        public string Root { get; set; } = Directory.GetCurrentDirectory();

        public override bool CanConvert(Type objectType)
            => objectType.IsSubclassOf(typeof(FileSystemInfo)) || objectType == typeof(FileSystemInfo);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            string path = ((FileSystemInfo)value).FullName;

            JValue jValue = new JValue(
                path.StartsWith(Root)
                    ? "." + path.Remove(0, Root.Length)
                    : path);

            jValue.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            if (objectType != typeof(FileInfo) && objectType != typeof(DirectoryInfo))
                throw new NotSupportedException( $"{GetType().Name} does not support deserialization of '{objectType}' type.");

            string path = JToken.Load(reader).Value<string>();

            if (string.IsNullOrEmpty(path)) {
                return null;
            }

            string resolvedPath = path.StartsWith(@".\")
                ? Path.Combine(Root, path.Remove(0, 2))
                : path;

            return Activator.CreateInstance(objectType, resolvedPath);
        }

    }

}
