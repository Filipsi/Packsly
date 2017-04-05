﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace Core.JsonConverters {

    public class DirectoryInfoJsonConverter : JsonConverter {

        public override bool CanConvert(Type objectType) {
            return objectType.Equals(typeof(DirectoryInfo));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            return new DirectoryInfo(JToken.Load(reader).Value<string>());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            new JValue((value as DirectoryInfo).FullName).WriteTo(writer);
        }

    }

}