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
using KnightBot.util;
using KnightBot.Modules.NewServer;
using System.IO;
using KnightBot.Modules.Economy;

namespace KnightBot
{
    public class CommandHandler : ModuleBase
    {
        private CommandService commands;
        private DiscordSocketClient bot;
        private IServiceProvider map;

        private int total;

        private BankConfig save = new BankConfig();

        public static readonly string appdir = AppContext.BaseDirectory;


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

        /*
        public async Task addMoney(SocketMessage msg)
        {
            Errors errors = new Errors();

            var user = msg.Author;
            var result = Database.CheckExistingUser(user);
            if (result.Count <= 0 && user.IsBot != true)
            {
                Database.EnterUser(user);
            }

            Random rand = new Random();
            int maxChance = 10, maxAmt = 5;

            int randNumber = rand.Next(1, maxChance);
            int randChance = rand.Next(1, maxChance);
            int randAmt = rand.Next(1, maxAmt);

            if (randChance == randNumber)
            {
                Database.updMoney(user, randAmt);
            }

        }
        */


        public async Task addMoney(SocketMessage msg)
        {
            var result = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").currentMoney;

            int bal = 10;

            total = result + bal;

            save.userID = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").userID;
            save.currentMoney = total;
            save.currentPoints = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").currentPoints;
            save.Save("bank/" + Context.User.Id.ToString() + ".json");

        }


        public async Task AnnounceLeftUser(SocketGuildUser user) { }

        public async Task AnnounceUserJoined(SocketGuildUser user)
        {
            /*
            var newmemrole = ServerConfig.Load("servers/" + Context.Guild.Id.ToString() + ".json").newMemRole;

            var role = user.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == );
            await (user as IGuildUser).AddRoleAsync(role);
            */

            var user2 = Context.User;
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == ServerConfig.Load("servers/" + Context.Guild.Id.ToString() + ".json").newMemRole);
            await (user2 as IGuildUser).AddRoleAsync(role);

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
            else if (message.HasStringPrefix(ServerConfig.Load("servers/" + context.Guild.Id.ToString() + ".json").serverPrefix, ref argPos))
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