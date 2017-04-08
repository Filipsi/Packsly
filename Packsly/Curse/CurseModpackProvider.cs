using Packsly.Core.Modpack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Packsly.Curse {

    public class CurseModpackProvider : IModpackProvider {

        private static Regex _patten = new Regex(@"(\w+:\/\/minecraft.curseforge.com)\/projects\/(\w+)\/files\/\d+");

        public bool CanUseSource(string source) {
            return Uri.IsWellFormedUriString(source, UriKind.Absolute) && _patten.IsMatch(source);
        }

        public Modpack Create(string source) {
            throw new NotImplementedException();
        }

    }
}
