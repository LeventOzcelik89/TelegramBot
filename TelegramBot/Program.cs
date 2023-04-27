using System;
using TL;

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

static string ConfigKO(string what)
{
    switch (what)
    {
        case "session_pathname": return "session02";
        case "api_id": return "14975510";
        case "api_hash": return "e9d3fe245774f2d4553c971d8353af26";
        case "phone_number": return "+905346340132";
        case "verification_code": Console.Write("Code KO: "); return Console.ReadLine();
        case "first_name": return "Kadir";      // if sign-up is required
        case "last_name": return "Okan";        // if sign-up is required
        case "password": return "------";     // if user has enabled 2FA
        default: return null;                  // let WTelegramClient decide the default config
    }
}

static string ConfigLevent(string what)
{
    switch (what)
    {
        case "session_pathname": return "session03";
        case "api_id": return "16232260";
        case "api_hash": return "9fa36c2f6751e326ca05b596cadd93a2";
        case "phone_number": return "+905374608988";
        case "verification_code": Console.Write("Code Levent: "); return Console.ReadLine();
        case "first_name": return "Levent";      // if sign-up is required
        case "last_name": return "Özçelik";        // if sign-up is required
        case "password": return "------";     // if user has enabled 2FA
        default: return null;                  // let WTelegramClient decide the default config
    }
}

using var client = new WTelegram.Client(Config);
await client.LoginUserIfNeeded();

using var clientKO = new WTelegram.Client(ConfigKO);
await clientKO.LoginUserIfNeeded();

//  using var clientLevent = new WTelegram.Client(ConfigLevent);
//  await clientLevent.LoginUserIfNeeded();

//  var chts = await client.Messages_GetAllChats();
//  var chts = await clientKO.Messages_GetAllChats();
//  var chts = await clientLevent.Messages_GetAllChats();

client.OnUpdate += Client_OnUpdate;

async Task Client_OnUpdate(IObject arg)
{
    var _message = "";
    try
    {
        if (!arg.GetType().IsAssignableFrom(typeof(TL.Updates)))
        {
            return;
        }

        var items = arg as TL.Updates;
        //  var item = items.chats.Where(a => a.Key == 1626065448).FirstOrDefault();
        //  var item = items.chats.Where(a => a.Key == 1585811560).FirstOrDefault();
        var item = items.chats.Where(a => a.Key == 1744775839).FirstOrDefault();

        if (item.Key == 0)
        {
            return;
        }

        var mes = (items.UpdateList.FirstOrDefault() as UpdateNewChannelMessage as TL.UpdateNewChannelMessage).message;
        var message = ((TL.Message)(items.UpdateList.FirstOrDefault() as UpdateNewChannelMessage as TL.UpdateNewChannelMessage).message);
        //  if (message.message.Contains("BINANCE:"))
        //  {
        var result = await client.Contacts_ResolveUsername("MaestroSniperBot");
        var resultKO = await clientKO.Contacts_ResolveUsername("MaestroSniperBot");

        var r = new System.Text.RegularExpressions.Regex("0x[0-9abcdefABCDEF]{40}");
        _message = client.EntitiesToHtml(message.message.ToString(), message.entities);
        var rs = r.Matches(_message);

        var msg = "BINANCE: " + rs[rs.Count - 1].ToString();

        await clientKO.SendMessageAsync(resultKO.User, msg);
        await client.SendMessageAsync(result.User, msg);
        //  await client.SendMessageAsync()
        //  }

    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        Console.WriteLine(ex.StackTrace);
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine(_message);
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
    }

}

do
{
    while (!Console.KeyAvailable)
    {

    }
} while (Console.ReadKey(true).Key != ConsoleKey.Escape);
