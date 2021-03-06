﻿using System;

namespace Packsly3.Cli.Common {

    internal static class Logo {

        private static readonly string[] PackslyLogo = {
            @" ____                 __               ___               __     ",
            @"/\  _`\              /\ \             /\_ \            /'__`\   ",
            @"\ \ \L\ \ __      ___\ \ \/'\     ____\//\ \    __  __/\_\L\ \ ",
            @" \ \ ,__/'__`\   /'___\ \ , <    /',__\ \ \ \  /\ \/\ \/_/_\_<_ ",
            @"  \ \ \/\ \L\.\_/\ \__/\ \ \\`\ /\__, `\ \_\ \_\ \ \_\ \/\ \L\ \",
            @"   \ \_\ \__/.\_\ \____\\ \_\ \_\/\____/ /\____\\/`____ \ \____/",
            @"    \/_/\/__/\/_/\/____/ \/_/\/_/\/___/  \/____/ `/___/> \/___/ ",
            @"                                                    /\___/      ",
            @"                                                    \/__/       ",
        };

        public static void Print() {
            foreach (string line in PackslyLogo) {
                Console.WriteLine(line);
            }
        }

    }
}
