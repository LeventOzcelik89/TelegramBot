﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Settings;
using TL;
using WTelegram;

namespace TelegramBot.Channels
{
    public class RedChannel : ChannelBase
    {
        public RedChannel(Client client, string logFile, ChatBase tgChannel, Settings.Config config) : base(client, logFile, tgChannel, config) { }

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
                result.warnings.red > 0 &&
                (config.liquidDollar.min == null || result._checkResult.liquid >= config.liquidDollar.min) &&
                (config.liquidDollar.max == null || result._checkResult.liquid <= config.liquidDollar.max) &&
                (config.ageSeconds?.min == null || result._checkResult.age >= config.ageSeconds.min) &&
                (config.ageSeconds?.max == null || result._checkResult.age <= config.ageSeconds.max) &&
                (config.unverified == null || result._checkResult.Unverified == config.unverified))
            {
                await client.SendMessageAsync(this.TGChannel, address);
            }

        }
    }
}
