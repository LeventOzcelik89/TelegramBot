using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot
{

    public static class DexAnalyzer
    {

        static HttpClient cli = new HttpClient();

        public static DexAnalyzerResult Check(string token)
        {

            try
            {

                var rs = cli.GetAsync($"https://api.dexanalyzer.io/rug/bsc/{token}?apikey=a3c28e4485ca47a8b8d33c73059");

                var rsr = rs.Result.Content.ReadAsStringAsync().Result.ToString();

                rsr = rsr.Replace("\\\"", "\"");
                rsr = rsr.Substring(1, rsr.Length - 2);

                var json = Newtonsoft.Json.JsonConvert.DeserializeObject<DexAnalyzerResult>(rsr);

                return json;

            }
            catch (Exception ex)
            {
                return null;
            }

        }



    }

    public class DexAnalyzerResult
    {
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
