using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TL;
using WTelegram;

namespace TelegramBot.Channels
{
    public class OttoChannel : ChannelBase
    {

        public static ChannelBase Base_Channel { get; set; }

        public OttoChannel(Client client, string logFile, ChatBase tgChannel, Settings.Config config) : base(client, logFile, tgChannel, config) { }

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

                (config.liquidDollar.min == null || result._checkResult.liquid >= config.liquidDollar.min) &&
                (config.liquidDollar.max == null || result._checkResult.liquid <= config.liquidDollar.max) &&

                (config.mcapDollar.min == null || result._checkResult.mcap >= config.mcapDollar.min) &&
                (config.mcapDollar.max == null || result._checkResult.mcap <= config.mcapDollar.max) &&

                (
                (result._checkResult.Unverified == true && result.warnings.red > 0) ||
                (!result._checkResult.Unverified == true && result.warnings.red == 0)
                )

                )

            {

                var log = PinkChannel.PinkBlackLog.content.FirstOrDefault(a => a.token.ToLower() == address.ToLower());
                if (log != null)
                {
                    var message = await client.SendMessageAsync(this.TGChannel, address + System.Environment.NewLine + log.age + Environment.NewLine + log.from);
                    //  var dexxanaly = new DexAnalyzer().Check(address);
                    //  if (!dexxanaly.options.Contains("HP RISK"))
                    //  {
                    //      client.Messages_UpdatePinnedMessage(this.TGChannel, message.id);
                    //  }
                }

                //  BlueLog.AppendLine(address);

            }

        }

        public override void init()
        {
            Base_Channel = this;
            base.init();
        }

    }
}
