using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TL;
using WTelegram;

namespace TelegramBot.Channels
{
    public class BlackChannel : ChannelBase
    {

        public PinkBlackChannel PinkBlackLog { get; set; }

        public BlackChannel(Client client, string logFile, ChatBase tgChannel, Settings.Config config) : base(client, logFile, tgChannel, config)
        {
            this.PinkBlackLog = new PinkBlackChannel("DB_PinkBlack.txt");
        }

        public override async void Check(string address, DexAnalyzerResult result)
        {
            base.Check(address, result);
            if (result == null) { return; }

            if (this.LogManager.content.Any(a => a.token == address))
            {
                Console.WriteLine("Aynı Token Geldi : " + address);
                return;
            }

            this.LogManager.AppendLine(address);

            if (
                result._checkResult.Unverified == true &&
                result._checkResult.liquid <= 1 &&
                result._checkResult.mcap == -999 &&

                (config.ageSeconds == null || result._checkResult.age <= config.ageSeconds)
                )       //  NaN
            {

                PinkBlackLog.AppendLine(address);

                await client.SendMessageAsync(this.TGChannel, address);

            }

        }
    }
}
