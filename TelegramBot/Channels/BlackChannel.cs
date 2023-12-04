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
        public BlackChannel(Client client, string logFile, ChatBase tgChannel, Settings.Config config) : base(client, logFile, tgChannel, config)
        {
            
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

                (config.ageSeconds.min == null || result._checkResult.age >= config.ageSeconds.min) &&
                (config.ageSeconds.max == null || result._checkResult.age <= config.ageSeconds.max)
                )       //  NaN
            {

                //PinkChannel.PinkBlackLog.AppendLine(address, result._checkResult.age + " sn", this.TGChannel.Title);
                PinkChannel.PinkBlackLog.AppendLine(address);

                await client.SendMessageAsync(this.TGChannel, address);

            }

        }
    }
}
