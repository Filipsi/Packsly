using Core.Instance.MultiMC;
using Packsly.Core.Configuration;
using Packsly.Core.Instance.MultiMC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Cli {

    class Cli {

        static void Main(string[] args) {
            Config.Load();

            MultimcInstance mmcInstnace = new MultimcInstance("muchkek", "1.11");
            mmcInstnace.ConfigFile.Save();
        
            Console.ReadKey();

        }

    }

}
