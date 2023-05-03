using System;
using System.Text.RegularExpressions;
using TelegramBot;
using TL;
using WTelegram;


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

//  static Messages_Dialogs? dials = null;

//  using var client = new Client(ConfigLB);
using var client = new Client(Config);
await client.LoginUserIfNeeded();

var dials = await client.Messages_GetAllDialogs();
// var chts = await clientLevent.Messages_GetAllChats();

//  MessageManager.RedChannel = dials.messages[0];

ChannelManager.RedChannel = dials.chats.Where(a => a.Value.Title == "RED CHANNEL").FirstOrDefault().Value as Channel;
ChannelManager.GreenChannel = dials.chats.Where(a => a.Value.Title == "GREEN CHANNEL").FirstOrDefault().Value as Chat;
ChannelManager.YellowChannel = dials.chats.Where(a => a.Value.Title == "YELLOW CHANNEL").FirstOrDefault().Value as Chat;
ChannelManager.BlueChannel = dials.chats.Where(a => a.Value.Title == "BLUE CHANNEL").FirstOrDefault().Value as Chat;
ChannelManager.TrendChannel = dials.chats.Where(a => a.Value.Title == "TRENDINGS").FirstOrDefault().Value as Channel;

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

        var mm = (items.UpdateList.FirstOrDefault() as UpdateNewChannelMessage).message;

        var rgx = new Regex("Token: (.*)");
        var tknMatch = rgx.Matches(mm.ToString());
        if (tknMatch.Count == 0)
        {
            return;
        }

        var tkn = tknMatch.FirstOrDefault().Value.Replace("Token: ", "");

        if (mm.ToString().IndexOf("Chain: ETH") > -1)
        {
            return;
        }

        if (mm.ToString().IndexOf("BSC TRENDING") > -1)
        {
            await client.SendMessageAsync(ChannelManager.TrendChannel, tkn);
            return;
        }

        var rs = DexAnalyzer.Check(tkn);

        if (rs != null && rs.warnings != null)
        {

            if (rs.warnings.red > 0)
            {
                await client.SendMessageAsync(ChannelManager.RedChannel, tkn);
            }
            else if (rs.warnings.yellow > 0)
            {
                await client.SendMessageAsync(ChannelManager.YellowChannel, tkn);
            }
            else if (rs.warnings.orange > 0)
            {
                await client.SendMessageAsync(ChannelManager.YellowChannel, tkn);
            }
            else
            {
                await client.SendMessageAsync(dials.chats[934711061], tkn);
            }

        }

    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        throw;
    }


}





//using var client = new WTelegram.Client(Config);
//await client.LoginUserIfNeeded();

//using var clientKO = new WTelegram.Client(ConfigKO);
//await clientKO.LoginUserIfNeeded();

//using var clientLevent = new WTelegram.Client(ConfigLevent);
//await clientLevent.LoginUserIfNeeded();

////  var chts = await client.Messages_GetAllChats();
////  var chts = await clientKO.Messages_GetAllChats();
////  var chts = await clientLevent.Messages_GetAllChats();

//client.OnUpdate += Client_OnUpdate;

//async Task Client_OnUpdate(IObject arg)
//{
//    var _message = "";
//    try
//    {
//        if (!arg.GetType().IsAssignableFrom(typeof(TL.Updates)))
//        {
//            return;
//        }

//        var items = arg as TL.Updates;
//        //  var item = items.chats.Where(a => a.Key == 1626065448).FirstOrDefault();
//        //  var item = items.chats.Where(a => a.Key == 1585811560).FirstOrDefault();
//        var item = items.chats.Where(a => a.Key == 1744775839).FirstOrDefault();

//        if (item.Key == 0)
//        {
//            return;
//        }

//        var mes = (items.UpdateList.FirstOrDefault() as UpdateNewChannelMessage as TL.UpdateNewChannelMessage).message;
//        var message = ((TL.Message)(items.UpdateList.FirstOrDefault() as UpdateNewChannelMessage as TL.UpdateNewChannelMessage).message);
//        //  if (message.message.Contains("BINANCE:"))
//        //  {
//        var result = await client.Contacts_ResolveUsername("MaestroSniperBot");
//        var resultKO = await clientKO.Contacts_ResolveUsername("MaestroSniperBot");

//        var r = new System.Text.RegularExpressions.Regex("0x[0-9abcdefABCDEF]{40}");
//        _message = client.EntitiesToHtml(message.message.ToString(), message.entities);
//        var rs = r.Matches(_message);

//        var msg = "BINANCE: " + rs[rs.Count - 1].ToString();

//        await clientKO.SendMessageAsync(resultKO.User, msg);
//        await client.SendMessageAsync(result.User, msg);
//        //  await client.SendMessageAsync()
//        //  }

//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine(ex.Message);
//        Console.WriteLine(ex.StackTrace);
//        Console.WriteLine();
//        Console.WriteLine();
//        Console.WriteLine();
//        Console.WriteLine(_message);
//        Console.WriteLine();
//        Console.WriteLine();
//        Console.WriteLine();
//    }

//}

do
{
    while (!Console.KeyAvailable)
    {

    }
} while (Console.ReadKey(true).Key != ConsoleKey.Escape);
