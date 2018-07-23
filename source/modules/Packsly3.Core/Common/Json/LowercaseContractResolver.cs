using Newtonsoft.Json.Serialization;

namespace Packsly3.Core.Common.Json {

    public class LowercaseContractResolver : DefaultContractResolver {

        protected override string ResolvePropertyName(string propertyName) {
            return propertyName.ToLower();
        }

    }

}
