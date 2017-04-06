using Core.Forge;
using Packsly.Core.Module;
using Packsly.MultiMc;
using PackslyMultiMc;
using System;

namespace Packsly.Cli {

    class Cli {

        static void Main(string[] args) {

            MmcInstance mmcTest = new MmcInstance("tut", "1.10.2");
            mmcTest.Icon = "http://i.imgur.com/Be877im.png";
            mmcTest.Save();

            ModuleRegistry tr = new ModuleRegistry(
                new MmcForgeModule()
            );

            tr.Execute(mmcTest, new ForgeModuleArgs("1.10.2-12.18.3.2221"));

            Console.ReadKey();
        }

    }

}
