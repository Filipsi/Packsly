using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.CommandEngine {

    internal partial class Command {

        internal class Argument {

            public string   Name            { private set; get; }
            public bool     IsRequired      { private set; get; }
            public bool     IsDataholder    { private set; get; }
            public bool     IsLiteral       { private set; get; }

            public Argument(string descriptor) {
                if(descriptor == string.Empty || descriptor == "") {
                    Name = string.Empty;
                    IsRequired = false;
                    IsDataholder = false;
                } else {
                    Name = descriptor.TrimStart('[', '(').TrimEnd(']', ')');
                    IsDataholder = IsSurroundedWith("[]", descriptor) || IsSurroundedWith("()", descriptor);
                    IsLiteral = !IsDataholder;
                    IsRequired = IsLiteral || IsSurroundedWith("[]", descriptor);
                }
            }

            private bool IsSurroundedWith(string chars, string source) {
                return source.First() == chars.ElementAt(0) && source.Last() == chars.ElementAt(1);
            }

        }

    }

}
