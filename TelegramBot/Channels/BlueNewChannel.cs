using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TL;
using WTelegram;

namespace TelegramBot.Channels
{
    public class BlueNewChannel : ChannelBase
    {
        public BlueNewChannel(Client client, string logFile, ChatBase tgChannel, Settings.Config config) : base(client, logFile, tgChannel, config) { }

        public override async void Check(string address, DexAnalyzerResult result)
        {
            base.Check(address, result);
            if (result == null) { return; }

            //var msgs = await client.Messages_GetHistory(OttoChannel.Base_Channel.TGChannel) ;
            ////  await client.SendMessageAsync(OttoChannel.Base_Channel.TGChannel, )

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
                    var message = await client.SendMessageAsync(this.TGChannel, address);
                    //  var dexxanaly = new DexAnalyzer().Check(address);
                    //  if (!dexxanaly.options.Contains("HP RISK"))
                    //  {
                    //      client.Messages_UpdatePinnedMessage(this.TGChannel, message.id);
                    //  }
                }

                //  BlueLog.AppendLine(address);

            }

        }
    }
}
