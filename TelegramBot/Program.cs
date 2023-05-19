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
var ch_Blue = new BlueChannel(client, "Ch_Blue.txt", GetChannel(dials, "BLUE-CHANNEL"), settings.Blue);
var ch_Pink = new PinkChannel(client, "Ch_Pink.txt", GetChannel(dials, "PINK-CHANNEL"), settings.Pink);

//  var adr = "0x93b2bc0d9d054395260dd825f2daf7e77e6ddb78";
//  var dexResult = new DexAnalyzer().Check(adr);
//  ch_Red.Check(adr, dexResult);
//  ch_Pink.Check(adr, dexResult);


client.OnUpdate += Client_UpDate;

async Task Client_UpDate(IObject arg)
{
    try
    {
        if (!arg.GetType().IsAssignableFrom(typeof(TL.Updates)))
        {
            return;
        }

        var items = arg as TL.Updates;
        var item = items.chats.Where(a => a.Key == 1675723936).FirstOrDefault();
        if (item.Key == 0)
        {
            return;
        }

        if (!items.UpdateList.FirstOrDefault().GetType().IsAssignableFrom(typeof(UpdateNewChannelMessage)))
        {
            return;
        }

        var mm = (items.UpdateList.FirstOrDefault() as UpdateNewChannelMessage).message.ToString();

        var rgx = new Regex("Token: (.*)");
        var tknMatch = rgx.Matches(mm);
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

        var address = tknMatch.FirstOrDefault().Value.Replace("Token: ", "");
        var dexResult = new DexAnalyzer().Check(address);

        if (dexResult == null)
        {
            log.AppendLine(address + System.Environment.NewLine + $"https://api.dexanalyzer.io/full/bsc/{address}?apikey=a3c28e4485ca47a8b8d33c73059");
            return;
        }

        ch_Red.Check(address, dexResult);
        ch_Green.Check(address, dexResult);
        ch_Pink.Check(address, dexResult);
        ch_Blue.Check(address, dexResult);
        ch_Yellow.Check(address, dexResult);

    }
    catch (Exception ex)
    {
        log.AppendLine(ex.Message.ToString() + System.Environment.NewLine + ex.StackTrace);
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
