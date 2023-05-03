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
        public Warnings warnings { get; set; }
        public string options { get; set; }
        public string telegram { get; set; }
        public string website { get; set; }
        public string twitter { get; set; }
        public string audit { get; set; }
    }

    public class Warnings
    {
        public int red { get; set; }
        public int orange { get; set; }
        public int yellow { get; set; }
    }

}
