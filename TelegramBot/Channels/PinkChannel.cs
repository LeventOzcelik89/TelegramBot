using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TL;
using WTelegram;

namespace TelegramBot.Channels
{
    public class PinkChannel : ChannelBase
    {
        public PinkChannel(Client client, string logFile, ChatBase tgChannel, Settings.Config config) : base(client, logFile, tgChannel, config) { }

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
                
                ((config.unverified == null || result._checkResult.Unverified == config.unverified) || ((config.unverified == false || config.unverified == null) && result.warnings.red == 0)) &&
                (config.liquidDollar.max == null || result._checkResult.liquid <= config.liquidDollar.max) &&
                (config.liquidDollar.min == null || result._checkResult.liquid >= config.liquidDollar.min) &&
                (config.mcapDollar.max == null || result._checkResult.mcap <= config.mcapDollar.max) &&
                (config.mcapDollar.min == null || result._checkResult.mcap >= config.mcapDollar.min))
            {

                await client.SendMessageAsync(this.TGChannel, address);

            }

        }
    }
}
