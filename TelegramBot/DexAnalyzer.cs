using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TL.Methods;

namespace TelegramBot
{

    public class DexAnalyzer
    {

        static double? BNBUSD { get; set; }

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

                var _liq = Convert.ToDouble(json.liquidparite.Replace("WBNB", "").Trim());
                var _mcap = Convert.ToInt32(json.marketcap);
                var _unv = json.options.Contains("UNVERIFIED CONTACT");

                json._checkResult = new CheckResult
                {
                    liquid = _liq * DexAnalyzer.BNBUSD,
                    mcap = _mcap,
                    Unverified = _unv
                };

                return json;

            }
            catch (Exception ex)
            {
                return null;
            }

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

    }

    public class DexAnalyzerResult
    {

        public CheckResult _checkResult { get; set; }

        public int network { get; set; }

        [JsonProperty("token-name")]
        public string tokenname { get; set; }

        [JsonProperty("contract-address")]
        public string contractaddress { get; set; }

        [JsonProperty("pair-address")]
        public string pairaddress { get; set; }
        public string owner { get; set; }
        public string decimals { get; set; }

        [JsonProperty("chain-website")]
        public string chainwebsite { get; set; }
        public int liquid { get; set; }
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
        public string options { get; set; }
        public Warnings warnings { get; set; }
        public string hp { get; set; }
        public string dexlink { get; set; }
        public string dexname { get; set; }
        public long date { get; set; }
    }

    public class Warnings
    {
        public int red { get; set; }
        public int orange { get; set; }
        public int yellow { get; set; }
    }

}
