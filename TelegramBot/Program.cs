using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using TelegramBot;
using TelegramBot.Channels;
using TL;
using WTelegram;

Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

ChatBase GetChannel(Messages_Dialogs dials, string channelName)
{
    return dials.chats.Where(a => a.Value.Title == channelName && a.Value.IsActive).FirstOrDefault().Value as ChatBase;
}

var confg = System.IO.File.ReadAllText("Configuration.json");
var settings = Newtonsoft.Json.JsonConvert.DeserializeObject<TelegramBot.Settings.Configuration>(confg);
var log = new LogManager("Errors.txt");

new DexAnalyzer().InitBNBUSD();

static string Config(string what)
{
    switch (what)
    {
        case "session_pathname": return "session01";
        case "api_id": return "22888657";
        case "api_hash": return "514000553ea36393605df0c3f4f704b8";
        case "phone_number": return "+905304119456";
        case "verification_code": Console.Write("Code: "); return Console.ReadLine();
        case "first_name": return "Volkan";      // if sign-up is required
        case "last_name": return "Okan";        // if sign-up is required
        case "password": return "------";     // if user has enabled 2FA
        case "MaxAutoReconnects": return "9999";
        default: return null;                  // let WTelegramClient decide the default config
    }
}

static string ConfigLB(string what)
{

    switch (what)
    {
        case "session_pathname": return "sessionlbb";
        case "api_id": return "22934089";
        case "api_hash": return "62f551ebbe934b6f2f7cf83ee97ae7eb";
        case "phone_number": return "+905368997528";
        case "verification_code": Console.Write("Code (Lebibe Okan): "); return Console.ReadLine();
        case "first_name": return "La";      // if sign-up is required
        case "last_name": return "marka";        // if sign-up is required
        case "password": return "------";     // if user has enabled 2FA
        default: return null;                  // let WTelegramClient decide the default config
    }

}

using var client = new Client(Config);
await client.LoginUserIfNeeded();

var dials = await client.Messages_GetAllDialogs();

var ch_Red = new RedChannel(client, "Ch_Red.txt", GetChannel(dials, "RED-CHANNEL"), settings.Red);
var ch_Green = new GreenChannel(client, "Ch_Green.txt", GetChannel(dials, "GREEN-CHANNEL"), settings.Green);
var ch_Yellow = new YellowChannel(client, "Ch_Yellow.txt", GetChannel(dials, "YELLOW-CHANNEL"), settings.Yellow);
//  VIP BSC LIQUDITY
//  var ch_Blue = new BlueChannel(client, "Ch_Blue.txt", GetChannel(dials, "BLUE-CHANNEL"), settings.Blue);
var ch_Pink = new PinkChannel(client, "Ch_Pink.txt", GetChannel(dials, "PINK-CHANNEL"), settings.Pink);
var ch_Black = new BlackChannel(client, "Ch_Black.txt", GetChannel(dials, "BLACK-CHANNEL"), settings.Black);
//  var ch_Otto = new OttoChannel(client, "Ch_Otto.txt", GetChannel(dials, "Otto BSC Deployments"), settings.Otto);
var ch_blueNew = new BlueNewChannel(client, "VIP BSC LIQUDITY", GetChannel(dials, "BLUE-CHANNEL"), settings.Blue);



var adr = "0x7c597cB25EEd33898bbaa57FF520d3E582661869";
ch_blueNew.Check(adr, new DexAnalyzerResult { _checkResult = new CheckResult { liquid = 223.201, mcap = 291 }, warnings = new Warnings { red = 0 } });


//for (int offset = 0; ;)
//{
//    var messagesBase = await client.Messages_GetHistory(OttoChannel.Base_Channel.TGChannel, 0, default, offset, 1000, 0, 0, 0);
//    if (messagesBase is not Messages_ChannelMessages channelMessages) break;
//    //  foreach (var msgBase in channelMessages.messages)
//    //      if (msgBase is Message msg)
//    //      {
//    //          // process the message
//    //      }
//    offset += channelMessages.messages.Length;
//    if (offset >= 10000) break;
//}



client.OnUpdate += Client_UpDate;

async Task Client_UpDate(IObject arg)
{

    var parsed = "NOT PARSED";

    try
    {
        if (!arg.GetType().IsAssignableFrom(typeof(TL.Updates)))
        {
            return;
        }

        var items = arg as TL.Updates;

        if (!items.UpdateList.FirstOrDefault().GetType().IsAssignableFrom(typeof(UpdateNewChannelMessage)))
        {
            return;
        }

        var item = items.chats.Where(a => a.Key == 1675723936).FirstOrDefault();
        if (item.Key == 0)
        {
            //  		Key	1662862406	long
            //          VIP BSC LIQUDITY
            item = items.chats.Where(a => a.Key == 1662862406).FirstOrDefault();
            if (item.Key == 0)
            {
                return;
            }
        }

        var mm = (items.UpdateList.FirstOrDefault() as UpdateNewChannelMessage).message.ToString();

        Regex rgx;
        string address;
        MatchCollection tknMatch;

        if (item.Key == 1662862406)
        {
            rgx = new Regex("BINANCE: (.*)");
            tknMatch = rgx.Matches(mm);
            if (tknMatch.Count == 0)
            {
                return;
            }

            address = tknMatch.FirstOrDefault().Value.Replace("BINANCE: ", "").Trim();

            var liquidRgx = new Regex("Liquidity: (.*)");
            var liquidMatch = liquidRgx.Matches(mm);
            if (liquidMatch.Count == 0)
            {
                return;
            }

            var liquid = Convert.ToDouble(liquidMatch.FirstOrDefault().Value.Replace("Liquidity:", "").Replace("WBNB", "").Replace(",", "").Trim());

            var mCapRgx = new Regex("MCap: (.*) ");
            var mCapMatch = mCapRgx.Matches(mm);
            if (mCapMatch.Count == 0)
            {
                return;
            }

            var msg = ((TL.Message)((TL.UpdateNewMessage)items.UpdateList.FirstOrDefault()).message).message;
            var ent = ((TL.Message)((TL.UpdateNewMessage)items.UpdateList.FirstOrDefault()).message).entities;
            parsed = client.EntitiesToHtml(msg.ToString(), ent);

            //var reveralRegex = new Regex("\"https:\\/\\/t\\.me\\/wagiebot\\?start=safebot(.*)\"");
            //var reveralRegexResult = reveralRegex.Matches(parsed);
            //if (reveralRegexResult.Count() == 0)
            //{
            //    reveralRegexResult = new Regex("start=iBEhhEXl_snipe_(.*)\"").Matches(parsed);
            //}
            //var tokenAddress = reveralRegexResult[0].Value
            //    .Replace("\"https://t.me/wagiebot?start=safebot", "")
            //    .Replace("href=\"https://bscscan.com/address/", "")
            //    .Replace("start=", "")
            //    .Replace("iBEhhEXl_snipe_", "")
            //    .Replace("\">", "")
            //    .Replace("\"", "");

            var mCap = Convert.ToInt32(Convert.ToDouble(mCapMatch.FirstOrDefault().Value.Replace("MCap:", "").Replace("$", "").Replace(",", "").Trim()));
            ch_blueNew.Check(address, new DexAnalyzerResult
            {
                _checkResult = new CheckResult
                {
                    liquid = liquid * DexAnalyzer.BNBUSD,
                    mcap = mCap
                },
                warnings = new Warnings
                {
                    red = parsed.IndexOf("🔴") > -1 ? 1 : 0
                }
            });

            return;

        }

        rgx = new Regex("Token: (.*)");
        tknMatch = rgx.Matches(mm);
        if (tknMatch.Count == 0)
        {
            return;
        }

        if (mm.IndexOf("Chain: ETH") > -1)
        {
            return;
        }

        if (mm.IndexOf("ETH TREND") > -1)
        {
            return;
        }

        address = tknMatch.FirstOrDefault().Value.Replace("Token: ", "").Trim();
        var dexResult = new DexAnalyzer().Check(address);

        if (dexResult == null)
        {
            log.AppendLine(address + System.Environment.NewLine + $"https://api.dexanalyzer.io/full/bsc/{address}?apikey=a3c28e4485ca47a8b8d33c73059");
            return;
        }

        ch_Red.Check(address, dexResult);
        ch_Green.Check(address, dexResult);
        ch_Pink.Check(address, dexResult);
        //  ch_Blue.Check(address, dexResult);
        ch_Yellow.Check(address, dexResult);
        ch_Black.Check(address, dexResult);

    }
    catch (Exception ex)
    {
        log.AppendLine(ex.Message.ToString() + parsed + System.Environment.NewLine + ex.StackTrace);
        Console.WriteLine(ex.Message);
        throw;
    }

}

do
{
    while (!Console.KeyAvailable)
    {

    }
} while (Console.ReadKey(true).Key != ConsoleKey.Escape);
