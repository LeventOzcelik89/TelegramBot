using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot
{

    public class LogRow
    {
        public LogRow()
        {
            
        }
        public LogRow(string token)
        {
            this.date = DateTime.Now;
            this.token = token;
        }

        public LogRow(DateTime date, string token)
        {
            this.date = DateTime.Now;
            this.token = token;
        }

        public DateTime date { get; set; }
        public string token { get; set; }
    }

    public class LogManager
    {
        private string dir { get; set; }
        public string rawLog { get; private set; }
        public string fileName { get; private set; }
        public LogRow[] content { get; set; }

        public LogManager(string filename)
        {
            this.fileName = filename;
            if (this.fileName == null) { return; }
            this.dir = Path.Combine(Environment.CurrentDirectory, this.fileName);
            if (!Directory.Exists(this.dir))
            {
                File.WriteAllText(this.dir, Newtonsoft.Json.JsonConvert.SerializeObject(new LogRow("0x00000")) + Environment.NewLine);
            }
            this.rawLog = File.ReadAllText(filename);
            content = rawLog.Split(System.Environment.NewLine).Where(a => !String.IsNullOrEmpty(a)).ToArray().Select(a => Newtonsoft.Json.JsonConvert.DeserializeObject<LogRow>(a)).ToArray();
        }

        public void AppendLine(string token)
        {
            try
            {
                var newLine = Newtonsoft.Json.JsonConvert.SerializeObject(new LogRow(token));
                File.AppendAllText(this.dir, newLine + Environment.NewLine);
            }
            catch (Exception ex)
            {

            }
        }


    }
}
