using Discord;
using Discord.Commands;
using Discord.WebSocket;
using KnightBot.Config;
using KnightBot.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightBot.Modules.Profanity
{
    class ProfanityFilter
    {
        public static async Task ProfanityCheckAsync(SocketMessage pMsg)
        {
            var message = pMsg as SocketUserMessage;
            var user = pMsg.Author;
            if (message == null)
                return;

            /**
            if (user.Username.ToString().Equals("MUTED USER")) 
            {
                await message.DeleteAsync();
                await user.SendMessageAsync("You have been muted in the GTA5Police server until you speak to Crunch in teamspeak. Teamspeak IP: gta5police.com");
            }
            **/

            KnightBot.Modules.Statistics.Statistics.AddIncomingMessages();

            for (int i = 0; i <= BotConfig.Load().Filters - 1; i++)
            {
                if (message.ToString().ToLower().Contains(BotConfig.Load().FilteredWords[i].ToLower()))
                {
                    await ProfanityMessage.WarningMessageAsync(pMsg, BotConfig.Load().FilteredWords[i]);
                    await ProfanityMessage.LogMessageAsync(CommandHandler.GetBot(), pMsg, BotConfig.Load().FilteredWords[i]);
                    await ProfanityMessage.DMMessageAsync(CommandHandler.GetBot(), pMsg, BotConfig.Load().FilteredWords[i]);
                    await ProfanityBanAsync(CommandHandler.GetBot(), pMsg);
                    KnightBot.Modules.Statistics.Statistics.AddProfanityDetected();
                }
            }
        }

        public static async Task ProfanityBanAsync(DiscordSocketClient bot, SocketMessage pMsg)
        {
            var server = bot.Guilds.FirstOrDefault(x => x.Id == BotConfig.Load().serverId);
            await server.AddBanAsync(pMsg.Author, 7, "Profanity detected in discord chat. Check server logs for more information.");
            await Program.Logger(new LogMessage(LogSeverity.Info, "Knight Gaming Profanity", "Profanity was detected by the user " + pMsg.Author.Username + "."));
        }
    }
}
