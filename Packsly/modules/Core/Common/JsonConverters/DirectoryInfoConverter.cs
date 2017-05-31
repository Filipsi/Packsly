using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace Packsly.Core.Common.JsonConverters {

    // TODO: Allow usage of relative paths
    public class DirectoryInfoConverter : JsonConverter {

        public override bool CanConvert(Type objectType) {
            return objectType.Equals(typeof(DirectoryInfo));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            string path = JToken.Load(reader).Value<string>();
            return new DirectoryInfo(path.StartsWith(@".\") ? Path.Combine(Directory.GetCurrentDirectory(), path.Remove(0, 2)) : path);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            string workspace = Directory.GetCurrentDirectory();
            string path = (value as DirectoryInfo).FullName;
            new JValue(path.StartsWith(workspace) ? "." + path.Remove(0, workspace.Length) : path).WriteTo(writer);
        }

    }

}
