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
            Config.Load();

            //MultimcInstance mmcInstnace = new MultimcInstance("muchkek", "1.11");
            //mmcInstnace.Save();

            //MultimcInstance mmcInstnace = MultimcInstance.FromExisting("muchkek");
            //mmcInstnace.Delete();

            Console.ReadKey();

        }

    }

}
