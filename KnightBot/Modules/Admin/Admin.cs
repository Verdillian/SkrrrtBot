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
using KnightBot.util;
using KnightBot.Modules.NewServer;
using KnightBot.Modules.Economy;

namespace KnightBot.Modules.Admin
{
    public class Admin : ModuleBase<SocketCommandContext>
    {
        Errors errors = new Errors();
        BotConfig config = new BotConfig();

        [Command("clear")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task Cl([Remainder] int x = 0)
        {

            if (x <= 0)
            {
                var embed = new EmbedBuilder() { Color = Colors.adminCol };
                embed.Title = ("**Clear Messages**");
                embed.Description = ($"**{Context.User.Mention}**, You Cannot Delete **0** Messages!");
                await ReplyAsync("", false, embed.Build());
            } else if (x <= 100)
            {
                var messagesToDelete = await Context.Channel.GetMessagesAsync(x + 1).Flatten();
                await Context.Channel.DeleteMessagesAsync(messagesToDelete);

                var embed = new EmbedBuilder() { Color = Colors.adminCol };
                embed.Title = ("**Clear Messages**");
                embed.Description = ($"**{Context.User.Mention}** Deleted **{x}** Messages.");
                await ReplyAsync("", false, embed.Build());
            } else if (x > 100)
            {
                var embed = new EmbedBuilder() { Color = Colors.adminCol };
                embed.Title = ("**Clear Messages**");
                embed.Description = ("**{Context.User.Mention}**, You Cannot Delete More Than 100 Messages!");
                await ReplyAsync("", false, embed.Build());
            }
        }
    

        [Command("ban")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task Ban(SocketGuildUser user = null, [Remainder] string reason = null)
        {
            if (user == null) await errors.sendError(Context.Channel, "You must enter a user!", Colors.adminCol);
            if (string.IsNullOrWhiteSpace(reason)) await errors.sendError(Context.Channel, "You must enter a reason!", Colors.adminCol);

            var gld = Context.Guild as SocketGuild;
            var embed = new EmbedBuilder()
            {
                Color = Colors.adminCol
            };

            embed.Title = $"**{user.Username}** has been banned!";
            embed.Description = $"**Username: **{user.Username}\n**Guild Name: **{user.Guild.Name}\n**Banned By: **{Context.User.Mention}!\n**Reason: **{reason}";

            await gld.AddBanAsync(user);
            await Context.Guild.GetTextChannel(434728317208494094).SendMessageAsync("", false, embed);

            await Context.Message.DeleteAsync();
        }

        [Command("kick")]
        [RequireBotPermission(GuildPermission.KickMembers)]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task Kick(SocketGuildUser user = null, [Remainder] string reason = null)
        {
            if (user == null) await errors.sendError(Context.Channel, "You must enter the user!", Colors.adminCol);
            if (string.IsNullOrWhiteSpace(reason)) await errors.sendError(Context.Channel, "You must enter a reason!", Colors.adminCol);

            var gld = Context.Guild as SocketGuild;
            var embed = new EmbedBuilder() { Color = Colors.adminCol };

            embed.Title = $"**{user.Username}** has been kicked from **{user.Guild.Name}**!";
            embed.Description = $"**Username: **{user.Username}\n**Guild name: **{user.Guild.Name}\n**Kicked By: **{Context.User.Mention}\n**Reason: **{reason}";

            await user.KickAsync();
            await Context.Guild.GetTextChannel(434728317208494094).SendMessageAsync("", false, embed);

            await Context.Message.DeleteAsync();
        }

        [Command("setrole")]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        public async Task AddRole(IGuildUser user, [Remainder] string roleToBe)
        {
            if (user.Equals(null)) await errors.sendError(Context.Channel, "You must enter the user!", Colors.adminCol);
            if (string.IsNullOrWhiteSpace(roleToBe)) await errors.sendError(Context.Channel, "You must enter a role!", Colors.adminCol);

            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == roleToBe);
            await (user as IGuildUser).AddRoleAsync(role);

            var embed = new EmbedBuilder()
            {
                Color = Colors.adminCol
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
            if (user.Equals(null)) await errors.sendError(Context.Channel, "You must enter the user!", Colors.adminCol);
            if (string.IsNullOrWhiteSpace(roleToRemove)) await errors.sendError(Context.Channel, "You must enter a role!", Colors.adminCol);

            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == roleToRemove);
            await (user as IGuildUser).RemoveRoleAsync(role);

            var embed = new EmbedBuilder()
            {
                Color = Colors.adminCol
            };
            embed.Description = (Context.User.Mention + ", had " + roleToRemove + " removed!");
            await ReplyAsync("", false, embed.Build());

            await Context.Message.DeleteAsync();
        }
    
        /**[Command("rulenew")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ruleNew()
        {
            var config = new BotConfig();

            var embed = new EmbedBuilder()
            {
                Color = Colors.adminCol
            };
            embed.Description = ("**Rules: **\n\n**1. **No Racial Slurs!\n**2. **Do Not Be Rude To Others!\n**3. **Do Not Spam Random Links / Messages!\n**4. **Don't Be An Asshat!\n\n**" + BotConfig.Load().Prefix + "accept** To Accept The Rules!");
            await ReplyAsync("", false, embed.Build());

            await Context.Message.DeleteAsync();
        }**/
    }
}
