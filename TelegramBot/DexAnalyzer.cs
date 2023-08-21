using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TL.Methods;

namespace TelegramBot
{

    public class DexAnalyzer
    {

        public static double? BNBUSD { get; set; }

        static HttpClient cli = new HttpClient();

        public DexAnalyzerResult Check(string token)
        {

            try
            {

                var rs = cli.GetAsync($"https://api.dexanalyzer.io/full/bsc/{token}?apikey=a3c28e4485ca47a8b8d33c73059");

                var rsr = rs.Result.Content.ReadAsStringAsync().Result.ToString();

                rsr = rsr.Replace("\\\"", "\"");
                rsr = rsr.Substring(1, rsr.Length - 2);

                var json = Newtonsoft.Json.JsonConvert.DeserializeObject<DexAnalyzerResult>(rsr);

                var _liq = Convert.ToDouble(json.liquidparite.Replace("WBNB", "").Replace("BNB", "").Trim());
                var _mcap = Int32.TryParse(json.marketcap, out _) ? Convert.ToInt32(json.marketcap) : -999;
                var _unv = json.options.Contains("UNVERIFIED CONTACT");

                json._checkResult = new CheckResult
                {
                    liquid = _liq * DexAnalyzer.BNBUSD,
                    mcap = _mcap,
                    Unverified = _unv,
                    age = CalculateTimeSpan(json.age)
                };

                return json;

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        private int CalculateTimeSpan(string age)
        {

            var ts = 0;
            //  29d 7h 20m 21s
            var rDay = new Regex("[0-9]+d");
            if (rDay.Match(age).Success)
            {
                ts += Convert.ToInt32(rDay.Match(age).Value.ToString().Replace("d", "")) * 60 * 60 * 24;
            }

            var rHour = new Regex("[0-9]+h");
            if (rHour.Match(age).Success)
            {
                ts += Convert.ToInt32(rHour.Match(age).Value.ToString().Replace("h", "")) * 60 * 60;
            }

            var rMinute = new Regex("[0-9]+m");
            if (rMinute.Match(age).Success)
            {
                ts += Convert.ToInt32(rMinute.Match(age).Value.ToString().Replace("m", "")) * 60;
            }

            var rSec = new Regex("[0-9]+s");
            if (rSec.Match(age).Success)
            {
                ts += Convert.ToInt32(rSec.Match(age).Value.ToString().Replace("s", ""));
            }

            return ts;

        }

        public void InitBNBUSD()
        {

            var rs = cli.GetAsync("https://api.binance.com/api/v3/ticker/price?symbol=BNBUSDT");
            var rsr = rs.Result.Content.ReadAsStringAsync().Result.ToString();
            var json = Newtonsoft.Json.JsonConvert.DeserializeObject<BinancePrice>(rsr);

            BNBUSD = json.USD;

            Console.WriteLine("BNB TO USD : " + json.USD);

        }

    }

    public class BinancePrice
    {
        public string symbol { get; set; }
        public string price { get; set; }
        public double USD { get { return Convert.ToDouble(this.price); } }
    }


    public class CheckResult
    {

        public double? liquid { get; set; }
        public int? mcap { get; set; }
        public bool? Unverified { get; set; }
        public int age { get; set; }

    }

    public class DexAnalyzerResult
    {

        public CheckResult _checkResult { get; set; }


        public int? network { get; set; }

        [JsonProperty("token-name")]
        public string tokenname { get; set; }

        [JsonProperty("contract-address")]
        public string contractaddress { get; set; }

        [JsonProperty("pair-address")]
        public string pairaddress { get; set; }
        public string owner { get; set; }
        public string decimals { get; set; }
        public double? dead { get; set; }

        [JsonProperty("chain-website")]
        public string chainwebsite { get; set; }
        public int? liquid { get; set; }
        public string marketcap { get; set; }

        [JsonProperty("liquid-parite")]
        public string liquidparite { get; set; }
        public string @lock { get; set; }
        public string taxes { get; set; }

        [JsonProperty("max-amount")]
        public string maxamount { get; set; }
        public string telegram { get; set; }
        public string audit { get; set; }
        public string website { get; set; }
        public string twitter { get; set; }
        public string discord { get; set; }
        public string age { get; set; }
        public string options { get; set; }
        public Warnings warnings { get; set; }
        public string hp { get; set; }
        public string dexlink { get; set; }
        public string dexname { get; set; }
        public long? date { get; set; }

        public string initialmcap { get; set; }
        public string initiallp { get; set; }



    }

    public class Warnings
    {
        public int red { get; set; }
        public int orange { get; set; }
        public int yellow { get; set; }
    }

}
