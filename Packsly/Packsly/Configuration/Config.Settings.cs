﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Configuration {

    public static partial class Config {

        public class Settings {

            public string MultiMC       { get; set; } = string.Empty;

            public string LastSeekUrl   { get; set; } = string.Empty;

        }

    }

}