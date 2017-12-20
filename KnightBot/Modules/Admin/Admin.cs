using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using Discord.Audio;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using KnightBot.Config;
using KnightBot;

namespace KnightBot.Modules.Admin
{
    public class Admin : ModuleBase<SocketCommandContext>
    {

        [Command("ban")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task Ban(SocketGuildUser user = null, [Remainder] string reason = null)
        {
            if (user == null) throw new ArgumentException("You Must Mention A User!");
            if (string.IsNullOrWhiteSpace(reason)) throw new ArgumentException("You Must Provide A Reason!");

            var gld = Context.Guild as SocketGuild;
            var embed = new EmbedBuilder()
            {
                Color = new Color(0, 175, 240)
            };

            embed.Title = $"**{user.Username}** has been banned!";
            embed.Description = $"**Username: **{user.Username}\n**Guild Name: **{user.Guild.Name}\n**Banned By: **{Context.User.Mention}!\n**Reason: **{reason}";

            await gld.AddBanAsync(user);
            await Context.Channel.SendMessageAsync("", false, embed);

            await Context.Message.DeleteAsync();

        }

        [Command("kick")]
        [RequireBotPermission(GuildPermission.KickMembers)]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task Kick(SocketGuildUser user = null, [Remainder] string reason = null)
        {
            if (user == null) throw new ArgumentException("You Must Mention A User!");
            if (string.IsNullOrWhiteSpace(reason)) throw new ArgumentException("You Must Provide A Reason!");

            var gld = Context.Guild as SocketGuild;
            var embed = new EmbedBuilder()
            {
                Color = new Color(0, 175, 240)
            };

            embed.Title = $"**{user.Username}** has been kicked from **{user.Guild.Name}**!";
            embed.Description = $"**Username: **{user.Username}\n**Guild name: **{user.Guild.Name}\n**Kicked By: **{Context.User.Mention}\n**Reason: **{reason}";

            await user.KickAsync();
            await Context.Channel.SendMessageAsync("", false, embed);

            await Context.Message.DeleteAsync();


        }

        [Command("setrole")]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        public async Task AddRole(IGuildUser user, [Remainder] string roleToBe)
        {
            if (user.Equals(null)) throw new ArgumentException("You Must Mention A User!");
            if (string.IsNullOrWhiteSpace(roleToBe)) throw new ArgumentException("You Must Provide A Role!");

            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == roleToBe);
            await (user as IGuildUser).AddRoleAsync(role);

            var embed = new EmbedBuilder()
            {
                Color = new Color(0, 175, 240)
            };
            embed.Description = (Context.User.Mention + ", had " + roleToBe + " added!");
            await ReplyAsync("", false, embed.Build());


            await Context.Message.DeleteAsync();


        }

        [Command("remrole")]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        public async Task RemoveRole(IGuildUser user, [Remainder] string roleToRemove)
        {
            if (user.Equals(null)) throw new ArgumentException("You Must Mention A User!");
            if (string.IsNullOrWhiteSpace(roleToRemove)) throw new ArgumentException("You Must Provide A Role!");

            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == roleToRemove);
            await (user as IGuildUser).RemoveRoleAsync(role);

            var embed = new EmbedBuilder()
            {
                Color = new Color(0, 175, 240)
            };
            embed.Description = (Context.User.Mention + ", had " + roleToRemove + " removed!");
            await ReplyAsync("", false, embed.Build());

            await Context.Message.DeleteAsync();



        }



        // Start Money

        [Command("addmoney")]
        public async Task Addmoney(IGuildUser user, [Remainder] int money)
        {
            var config = new BotConfig();
            var userName = Context.User as SocketGuildUser;
            var moneyrole = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == config.MoneyRole);
            var moneyrole1 = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == config.MoneyRole1);
            var moneyrole2 = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == config.MoneyRole2);

            if (!userName.Roles.Contains(moneyrole) || !userName.Roles.Contains(moneyrole1) || !userName.Roles.Contains(moneyrole2))
            {
                /**Database.updMoney(user, money);
                var embed = new EmbedBuilder()
                {
                    Color = new Color(0, 175, 240)
                };
                embed.Description = (Context.User.Mention + ", Has Gotton :moneybag: " + money + " Coins!");
                await ReplyAsync("", false, embed.Build());**/

                var embed = new EmbedBuilder()
                {
                    Color = new Color(0, 175, 240)
                };
                embed.Description = (Context.User.Mention + ", It fkin worked bitches!");
                await ReplyAsync("", false, embed.Build());

            }
        }

        // End Money

        [Command("rulenew")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ruleNew()
        {
            var config = new BotConfig();

            var embed = new EmbedBuilder()
            {
                Color = new Color(0, 175, 240)
            };
            embed.Description = ("**Rules: **\n\n**1. **No Racial Slurs!\n**2. **Do Not Be Rude To Others!\n**3. **Do Not Spam Random Links / Messages!\n**4. **Don't Be An Asshat!\n\n**" + BotConfig.Load().Prefix + "accept** To Accept The Rules!");
            await ReplyAsync("", false, embed.Build());

            await Context.Message.DeleteAsync();



        }



    }
}
