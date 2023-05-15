using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TL;
using WTelegram;

namespace TelegramBot
{
    public static class ChannelManager
    {

        public static TL.ChatBase RedChannel = null;
        public static TL.ChatBase GreenChannel = null;
        public static TL.ChatBase YellowChannel = null;
        public static TL.ChatBase BlueChannel = null;
        public static TL.ChatBase TrendChannel = null;
        public static TL.ChatBase PinkChannel = null;
    }

    public abstract class BaseChannel
    {

        public ChatBase TGChannel { get; set; }
        public LogManager LogManager { get; set; }
        public string logFile { get; }
        public Client client { get; set; }

        public BaseChannel(Client client, string logFile, ChatBase tgChannel)
        {
            this.client = client;
            this.TGChannel = tgChannel;
            this.LogManager = new LogManager(logFile);
        }

        public abstract void Check(string address, DexAnalyzerResult result);

    }

    public class GreenChannel : BaseChannel
    {
        public GreenChannel(Client client, string logFile, ChatBase tgChannel) : base(client, logFile, tgChannel) { }

        public override async void Check(string address, DexAnalyzerResult result)
        {

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
                result.warnings.yellow == 0 &&
                result.warnings.orange == 0 &&
                result._checkResult.liquid >= 300)
            {

                await client.SendMessageAsync(this.TGChannel, address);

            }

        }
    }

    public class RedChannel : BaseChannel
    {
        public RedChannel(Client client, string logFile, ChatBase tgChannel) : base(client, logFile, tgChannel) { }

        public override async void Check(string address, DexAnalyzerResult result)
        {

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
                result.warnings.red > 0 &&
                result._checkResult.liquid >= 600 &&
                result._checkResult.Unverified == false)
            {
                await client.SendMessageAsync(this.TGChannel, address);
            }

        }
    }

    public class YellowChannel : BaseChannel
    {
        public YellowChannel(Client client, string logFile, ChatBase tgChannel) : base(client, logFile, tgChannel) { }

        public override async void Check(string address, DexAnalyzerResult result)
        {

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
                result._checkResult.liquid >= 300)
            {

                await client.SendMessageAsync(this.TGChannel, address);

            }

        }
    }

    public class PinkChannel : BaseChannel
    {
        public PinkChannel(Client client, string logFile, ChatBase tgChannel) : base(client, logFile, tgChannel) { }

        public override async void Check(string address, DexAnalyzerResult result)
        {

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
                result._checkResult.Unverified == true &&
                result.warnings.red == 0 &&
                result._checkResult.liquid <= 1 &&
                result._checkResult.mcap <= 1)
            {

                await client.SendMessageAsync(this.TGChannel, address);

            }

        }
    }

    public class BlueChannel : BaseChannel
    {
        public BlueChannel(Client client, string logFile, ChatBase tgChannel) : base(client, logFile, tgChannel) { }

        public override async void Check(string address, DexAnalyzerResult result)
        {

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
                result._checkResult.Unverified == true &&
                result.warnings.red == 0 &&
                result.warnings.yellow == 0 &&
                result.warnings.orange == 0 &&
                result._checkResult.liquid >= 1 &&
                result._checkResult.liquid <= 290)
            {

                await client.SendMessageAsync(this.TGChannel, address);

            }

        }
    }

}
