using Newtonsoft.Json.Serialization;

namespace Packsly3.Core.Common.Json {

    internal class LowercaseContractResolver : DefaultContractResolver {

        protected override string ResolvePropertyName(string propertyName) {
            return propertyName.ToLower();
        }

    }

}
