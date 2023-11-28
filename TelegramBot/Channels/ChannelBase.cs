using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Settings;
using TL;
using WTelegram;

namespace TelegramBot.Channels
{
    public abstract class ChannelBase
    {

        public ChatBase TGChannel { get; set; }
        public LogManager LogManager { get; set; }
        public CheckList CheckList { get; set; }
        public string logFile { get; }
        public Client client { get; set; }
        public TelegramBot.Settings.Config config { get; set; }

        public ChannelBase(Client client, string logFile, ChatBase tgChannel, TelegramBot.Settings.Config config)
        {
            this.client = client;
            this.TGChannel = tgChannel;
            this.LogManager = new LogManager(logFile);
            this.CheckList = new CheckList("checklist_" + logFile);
            this.config = config;
            
            this.init();
        }

        public virtual void Check(string address, DexAnalyzerResult result)
        {
            this.CheckList.AppendLine(address + " - " + Newtonsoft.Json.JsonConvert.SerializeObject(result));
        }

        public virtual void init()
        {



        }

    }
}
