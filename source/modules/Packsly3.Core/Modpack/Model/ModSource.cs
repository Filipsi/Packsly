using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using Packsly3.Core.Launcher.Instance.Logic;

namespace Packsly3.Core.Modpack.Model {

    public class ModSource : RemoteResource {

        [JsonProperty("resources")]
        public RemoteResource[] Resources { private set; get; } = new RemoteResource[0];

        protected override void HandleOnDeserialized(StreamingContext context) {
            if (string.IsNullOrEmpty(FilePath)) {
                FilePath = "{" + EnvironmentVariables.ModsFolder + "}";
            }

            if (Url != null && string.IsNullOrEmpty(FileName)) {
                FileName = $"{CreateMd5(Url.AbsoluteUri)}.jar".ToLower();
            }
        }

        public static string CreateMd5(string input) {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create()) {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes) {
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }

    }

}
