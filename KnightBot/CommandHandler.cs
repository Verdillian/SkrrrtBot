using System.Threading.Tasks;
using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;
using Discord;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using KnightBot.Config;
using KnightBot.Modules.Public;

namespace KnightBot
{
    public class CommandHandler : ModuleBase
    {
        private CommandService commands;
        private DiscordSocketClient bot;
        private IServiceProvider map;

        public CommandHandler(IServiceProvider provider)
        {
            map = provider;
            bot = map.GetService<DiscordSocketClient>();
            bot.UserJoined += AnnounceUserJoined;
            bot.UserLeft += AnnounceLeftUser;
            bot.Ready += SetGame;
            //Send user message to get handled
            bot.MessageReceived += HandleCommand;
            commands = map.GetService<CommandService>();
            bot.MessageReceived += addMoney;
        }

        public async Task addMoney(SocketMessage msg)
        {
            var user = msg.Author;
            var result = Database.CheckExistingUser(user);
            if(result.Count <=0 && user.IsBot != true)
            {
                Database.EnterUser(user);
            }

            Database.updMoney(user, 2);

        }



        public async Task AnnounceLeftUser(SocketGuildUser user) {}

        public async Task AnnounceUserJoined(SocketGuildUser user)
        {
            var newmemrole = new BotConfig();

            var role = user.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == newmemrole.NewMemberRank);
            await (user as IGuildUser).AddRoleAsync(role);
            
            //var result = Database.CheckExistingUser(user);
            //if(result.Count() <= 0)
            //{
            //    Database.EnterUser(user);
            //}
        }

        public async Task SetGame()
        {
            await bot.SetGameAsync("KnightBot.xyz");
        }




        public async Task ConfigureAsync()
        {
            await commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        public async Task HandleCommand(SocketMessage pMsg)
        {
            //Don't handle the command if it is a system message
            var message = pMsg as SocketUserMessage;
            if (message == null)
                return;
            var context = new SocketCommandContext(bot, message);

            //Mark where the prefix ends and the command begins
            int argPos = 0;
            //Determine if the message has a valid prefix, adjust argPos
            if (message.HasStringPrefix(BotConfig.Load().Prefix, ref argPos))
            {
                if (message.Author.IsBot)
                    return;
                //Execute the command, store the result
                var result = await commands.ExecuteAsync(context, argPos, map);

                //If the command failed, notify the user
                if (!result.IsSuccess && result.ErrorReason != "Unknown command.")

                    await message.Channel.SendMessageAsync($"**Error:** {result.ErrorReason}");
            }
        }
    }
}