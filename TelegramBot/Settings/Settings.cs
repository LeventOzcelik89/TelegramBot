using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TL;

namespace TelegramBot.Settings
{

    public class MinMax
    {
        public int? min { get; set; }
        public int? max { get; set; }
    }

    public class Configuration
    {
        public Config Blue { get; set; }
        public Config Green { get; set; }
        public Config Pink { get; set; }
        public Config Red { get; set; }
        public Config Yellow { get; set; }
        public Config Black { get; set; }
        public Config Liquid { get; set; }
    }

    public class Config
    {
        public bool? unverified { get; set; }
        public MinMax liquidDollar { get; set; }
        public MinMax mcapDollar { get; set; }
        public MinMax ageSeconds { get; set; }
//          public int? ageSeconds { get; set; }
    }



}
