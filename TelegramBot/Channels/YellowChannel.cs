using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TL;
using WTelegram;

namespace TelegramBot.Channels
{
    public class YellowChannel : ChannelBase
    {
        public YellowChannel(Client client, string logFile, ChatBase tgChannel, Settings.Config config) : base(client, logFile, tgChannel, config) { }

        public override async void Check(string address, DexAnalyzerResult result)
        {
            base.Check(address, result);
            if (result == null) { return; }

            if (this.LogManager.content.Any(a => a.token == address))
            {
                Console.WriteLine("Aynı Token Geldi : " + address);
                return;
            }
            else
            {
                this.LogManager.AppendLine(address);
            }

            if (
                result.warnings.red == 0 &&
                (result.warnings.yellow > 0 || result.warnings.orange > 0) &&
                (config.liquidDollar.min != null || result._checkResult.liquid >= config.liquidDollar.min) &&
                (config.liquidDollar.max != null || result._checkResult.liquid <= config.liquidDollar.max))
            {

                await client.SendMessageAsync(this.TGChannel, address);

            }

        }
    }
}
