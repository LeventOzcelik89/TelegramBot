using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TL;
using WTelegram;

namespace TelegramBot.Channels
{
    public class BlueChannel : ChannelBase
    {
        public PinkBlackChannel BlueLog { get; set; }

        public BlueChannel(Client client, string logFile, ChatBase tgChannel, Settings.Config config) : base(client, logFile, tgChannel, config)
        {
            this.BlueLog = new PinkBlackChannel("DB_PinkBlack.txt");
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
            else
            {
                this.LogManager.AppendLine(address);
            }

            if (

                (config.unverified == null || result._checkResult.Unverified == config.unverified) &&
                (config.liquidDollar.min == null || result._checkResult.liquid >= config.liquidDollar.min) &&
                (config.liquidDollar.max == null || result._checkResult.liquid <= config.liquidDollar.max) &&

                (config.mcapDollar.min == null || result._checkResult.mcap >= config.mcapDollar.min) &&
                (config.mcapDollar.max == null || result._checkResult.mcap <= config.mcapDollar.max) &&

                result.warnings.red == 0 &&
                result.warnings.yellow == 0 &&
                result.warnings.orange == 0)

            {

                BlueLog.AppendLine(address);

                await client.SendMessageAsync(this.TGChannel, address);

            }

        }
    }
}
