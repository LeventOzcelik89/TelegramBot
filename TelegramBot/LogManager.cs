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

    public abstract class LogManagerBase
    {
        private string dir { get; set; }
        public string rawLog { get; private set; }
        public string fileName { get; private set; }
        public List<LogRow> content { get; set; }

        public LogManagerBase(string filename)
        {
            this.fileName = filename;
            if (this.fileName == null) { return; }
            this.dir = Path.Combine(Environment.CurrentDirectory, this.fileName);
            if (!File.Exists(this.dir))
            {
                File.WriteAllText(this.dir, Newtonsoft.Json.JsonConvert.SerializeObject(new LogRow("0x00000")) + Environment.NewLine);
            }
            this.rawLog = File.ReadAllText(filename);
            content = rawLog.Split(System.Environment.NewLine).Where(a => !String.IsNullOrEmpty(a)).ToArray().Select(a => Newtonsoft.Json.JsonConvert.DeserializeObject<LogRow>(a)).ToList();
        }

        public void AppendLine(string token)
        {
            try
            {
                this.content.Add(new LogRow(DateTime.Now, token));
                var newLine = Newtonsoft.Json.JsonConvert.SerializeObject(new LogRow(token));
                File.AppendAllText(this.dir, newLine + Environment.NewLine);
            }
            catch (Exception ex)
            {

            }
        }

    }

    public class LogManager : LogManagerBase
    {

        public LogManager(string fileName) : base(fileName)
        {
        }

    }

    public class CheckList : LogManagerBase
    {

        public CheckList(string fileName) : base(fileName)
        {
        }



    }

    public class PinkBlackChannel : LogManagerBase
    {

        public PinkBlackChannel(string fileName) : base(fileName)
        {
        }

    }

}
