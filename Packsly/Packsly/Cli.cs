using Packsly.Core.Configuration;
using Packsly.Core.Instance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Cli {

    class Cli {

        static void Main(string[] args) {
            MultimcInstance mmc = new MultimcInstance("tut", "1.11");
            mmc.Icon = "http://i.imgur.com/Be877im.png";
            mmc.Save();

            Console.ReadKey();
        }

    }

}
