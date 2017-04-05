using Packsly.Core.Configuration;
using Packsly.Core.Forge;
using Packsly.Core.MultiMc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Cli {

    class Cli {

        static void Main(string[] args) {

            MmcInstance mmcTest = new MmcInstance("tut", "1.10.2");
            mmcTest.Icon = "http://i.imgur.com/Be877im.png";
            mmcTest.Save();

            ForgeInstaller installer = new ForgeInstaller(
                new MmcForgeInstallationSchema()
            );

            installer.Install(mmcTest, "1.10.2-12.18.3.2221");

            Console.ReadKey();
        }

    }

}
