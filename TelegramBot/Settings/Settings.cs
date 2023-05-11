using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TelegramBot.Settings
{


    public class Configuration
    {
        public ChannelFilter Green { get; set; }
        public ChannelFilter Yellow { get; set; }
        public ChannelFilter Red { get; set; }
        public ChannelFilter Trends { get; set; }
        public ChannelFilter Pink { get; set; }
        public ChannelFilter Blue { get; set; }


    }

    public class ChannelFilter
    {
        public string logName { get; set; }
        public string text { get; set; }
        public bool? unverified { get; set; }
        public int? likit_min { get; set; }
        public int? likit_max { get; set; }
        public int? mcap_max { get; set; }
        public bool? block_duplicate { get; set; }
        public WarningsMax warnings_max { get; set; }

        public string Address { get; private set; }
        public LogManager LogManager { get; set; } = null;

        public bool Check(string rawData)
        {
            this.LogManager = this.LogManager ?? new LogManager(this.logName);
            var res = true;
            DexAnalyzerResult dexAnalyz = null;

            var rgx = new Regex("Token: (.*)");
            var tknMatch = rgx.Matches(rawData);
            if (tknMatch.Count == 0)
            {
                return false;
            }

            var address = tknMatch.FirstOrDefault().Value.Replace("Token: ", "");
            this.Address = address;

            if (this.LogManager.content.Any(a => a.token == this.Address))
            {
                Console.WriteLine("Aynı Token Geldi : " + this.Address);
                return false;
            }
            else
            {
                this.LogManager.AppendLine(this.Address);
            }

            if (!String.IsNullOrEmpty(this.text) && rawData.IndexOf(this.text) == -1)
            {
                res = false;
            }

            if (this.unverified != null)
            {
                dexAnalyz = dexAnalyz ?? DexAnalyzer.Check(address);

                if
                    (dexAnalyz != null && this.unverified == true && dexAnalyz.options.Contains("UNVERIFIED CONTACT") ||
                    (dexAnalyz != null && this.unverified == false && !dexAnalyz.options.Contains("UNVERIFIED CONTACT"))
                    )
                {
                    //  res = true;
                }
                else if
                    (dexAnalyz != null && this.unverified == true && !dexAnalyz.options.Contains("UNVERIFIED CONTACT") ||
                    (dexAnalyz != null && this.unverified == false && dexAnalyz.options.Contains("UNVERIFIED CONTACT"))
                    )
                {
                    res = false;
                }

            }

            if (this.likit_min != null)
            {

                dexAnalyz = dexAnalyz ?? DexAnalyzer.Check(address);

                if (dexAnalyz != null && this.likit_min < dexAnalyz.liquid)
                {
                    res = false;
                }
                else if (dexAnalyz != null && this.likit_min >= dexAnalyz.liquid)
                {
                    //  res = true;
                }

            }

            if (this.likit_max != null)
            {

                dexAnalyz = dexAnalyz ?? DexAnalyzer.Check(address);

                if (dexAnalyz != null && this.likit_max > dexAnalyz.liquid)
                {
                    res = false;
                }
                else if (dexAnalyz != null && this.likit_max <= dexAnalyz.liquid)
                {
                    //res = true;
                }

            }

            if (this.mcap_max != null)
            {

                dexAnalyz = dexAnalyz ?? DexAnalyzer.Check(address);

                if (dexAnalyz != null && dexAnalyz.marketcap != "NaN" && this.mcap_max >= Convert.ToInt32(dexAnalyz.marketcap))
                {
                    res = false;
                }
                else if (dexAnalyz != null && dexAnalyz.marketcap != "NaN" && this.mcap_max < Convert.ToInt32(dexAnalyz.marketcap))
                {
                    //res = true;
                }

            }

            if (this.warnings_max != null)
            {

                dexAnalyz = dexAnalyz ?? DexAnalyzer.Check(address);

                if (this.warnings_max.red != null)
                {
                    if (dexAnalyz != null && this.warnings_max.red >= dexAnalyz.warnings.red)
                    {
                        res = false;
                    }
                    else if (dexAnalyz != null && this.warnings_max.red < dexAnalyz.warnings.red)
                    {
                        //res = true;
                    }
                }

                if (this.warnings_max.orange != null)
                {
                    if (dexAnalyz != null && this.warnings_max.orange >= dexAnalyz.warnings.orange)
                    {
                        res = false;
                    }
                    else if (dexAnalyz != null && this.warnings_max.orange < dexAnalyz.warnings.orange)
                    {
                        //res = true;
                    }
                }

                if (this.warnings_max.yellow != null)
                {
                    if (dexAnalyz != null && this.warnings_max.yellow >= dexAnalyz.warnings.yellow)
                    {
                        res = false;
                    }
                    else if (dexAnalyz != null && this.warnings_max.yellow < dexAnalyz.warnings.yellow)
                    {
                        //res = true;
                    }
                }

            }

            return res;

        }


    }

    public class WarningsMax
    {
        public int? red { get; set; }
        public int? orange { get; set; }
        public int? yellow { get; set; }
    }



}
