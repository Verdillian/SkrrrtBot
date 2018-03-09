using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System.Linq;
using KnightBot.Config;
using KnightBot.util;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Linq;
using KnightBot.Modules.NewServer;

namespace KnightBot.Modules.Public
{
    public class PublicModule : ModuleBase
    {
        Errors errors = new Errors();


        // DM Command - Sends a message directly to knight.
        private static IUser ThisIsMe;
        [Command("dm")]
        [Remarks("Dm's the owner.")]
        public async Task Dmowner([Remainder] string dm)
        {
            var myId = Context.User.Mention;
            if (ThisIsMe == null)
            {
                foreach (var user in Context.Guild.GetUsersAsync().Result)
                {
                    if (user.Id == 146377960360902656)
                    {
                        ThisIsMe = user;
                        myId = user.Mention;
                        break;
                    }
                }
            }

            var application = await Context.Client.GetApplicationInfoAsync();
            var message = await application.Owner.GetOrCreateDMChannelAsync();
 
            var embed = new EmbedBuilder()
            {
                Color = Colors.adminCol
            };

            embed.Description = $"{dm}";
            embed.WithFooter(new EmbedFooterBuilder().WithText($"Message From: {Context.User.Username} | Guild: {Context.Guild.Name}"));
            await message.SendMessageAsync("", false, embed);
            embed.Description = $"You have sent a message to {myId} he will read the message soon.";
            embed.WithFooter(new EmbedFooterBuilder().WithText($"Message Has Been Sent Succesfully!"));

            await Context.Channel.SendMessageAsync("", false, embed);

            await Context.Message.DeleteAsync();
        }

        // Accept Command - Used to accept the rules and get the members role.
        [Command("accept")]
        public async Task Accept()
        {
            var chan = Context.Channel;
            var userName = Context.User as SocketGuildUser;
            var newMemberRole = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == BotConfig.Load().NewMemberRank);

            if (newMemberRole != null)
            {
                if (userName.Roles.Contains(newMemberRole))
                {
                    var user = Context.User;

                    var config = new BotConfig();
                    var role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == BotConfig.Load().AcceptedMemberRole);

                    var remrole = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == BotConfig.Load().NewMemberRank);

                    await (user as IGuildUser).AddRoleAsync(role);
                    await (user as IGuildUser).RemoveRoleAsync(remrole);

                    var message = await ReplyAsync("You Have Accepted The Rules! Enjoy Being A Full Member!");
                    await Delete.DelayDeleteMessage(message, 10);

                    await Context.Message.DeleteAsync();
                }
            }
            else await errors.sendError(chan, "The new members role is not set up correctly in the config!", Colors.generalCol);
        }

    }
}