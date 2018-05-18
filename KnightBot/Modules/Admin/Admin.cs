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
using SkrrrtBot.Config;
using SkrrrtBot;
using SkrrrtBot.util;

namespace SkrrrtBot.Modules.Admin
{
    public class Admin : ModuleBase<SocketCommandContext>
    {
        Errors errors = new Errors();
        BotConfig config = new BotConfig();

        [Command("setgame")]
        [Remarks("Sets the game the bot is currently playing")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Setgame()
        {

            await (Context.Client as DiscordSocketClient).SetGameAsync(BotConfig.Load().Prefix + "help" + " | " + "SkrrrtSkrrrtStudios.xyz");


            await Context.Message.DeleteAsync();
        }

        [Command("clear")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task Cl([Remainder] int x = 0)
        {

            if (x <= 0)
            {
                var embed = new EmbedBuilder() { Color = Colors.adminCol };
                embed.Title = ("**Clear Messages**");
                embed.Description = ($"**{Context.User.Mention}**, You Cannot Delete **0** Messages!");
                var messageToDel = await Context.Channel.SendMessageAsync("", false, embed);
                await Delete.DelayDeleteMessage(Context.Message, 10);
                await Delete.DelayDeleteMessage(messageToDel, 10);

            } else if (x <= 100)
            {
                var messagesToDelete = await Context.Channel.GetMessagesAsync(x + 1).Flatten();
                await Context.Channel.DeleteMessagesAsync(messagesToDelete);

                var embed = new EmbedBuilder() { Color = Colors.adminCol };
                embed.Title = ("**Clear Messages**");
                embed.Description = ($"**{Context.User.Mention}** Deleted **{x}** Messages.");
                var messageToDel = await Context.Channel.SendMessageAsync("", false, embed);
                await Delete.DelayDeleteMessage(Context.Message, 10);
                await Delete.DelayDeleteMessage(messageToDel, 10);
            } else if (x > 100)
            {
                var embed = new EmbedBuilder() { Color = Colors.adminCol };
                embed.Title = ("**Clear Messages**");
                embed.Description = ("**{Context.User.Mention}**, You Cannot Delete More Than 100 Messages!");
                var messageToDel = await Context.Channel.SendMessageAsync("", false, embed);
                await Delete.DelayDeleteMessage(Context.Message, 10);
                await Delete.DelayDeleteMessage(messageToDel, 10);

            }

            SkrrrtBot.Modules.Statistics.Statistics.AddCommandRequests();
            SkrrrtBot.Modules.Statistics.Statistics.AddOutgoingMessages();

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
            var footer = new EmbedFooterBuilder() { Text = "SkrrrtBot By SkrrrtSkrrrtStudios" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute };

            embed.WithFooter(footer);
            embed.Title = $"**{user.Username}** has been banned!";
            embed.Description = $"**Username: **{user.Username}\n**Guild Name: **{user.Guild.Name}\n**Banned By: **{Context.User.Mention}!\n**Reason: **{reason}";

            await gld.AddBanAsync(user);

            await Context.Message.DeleteAsync();

            var logchannel = Context.Guild.GetChannel(437977945680773130) as SocketTextChannel;
            await logchannel.SendMessageAsync("", false, embed);

            SkrrrtBot.Modules.Statistics.Statistics.AddCommandRequests();
            SkrrrtBot.Modules.Statistics.Statistics.AddOutgoingMessages();
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
            var footer = new EmbedFooterBuilder() { Text = "SkrrrtBot By SkrrrtSkrrrtStudios" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute };

            embed.WithFooter(footer);
            embed.Title = $"**{user.Username}** has been kicked from **{user.Guild.Name}**!";
            embed.Description = $"**Username: **{user.Username}\n**Guild name: **{user.Guild.Name}\n**Kicked By: **{Context.User.Mention}\n**Reason: **{reason}";

            await user.KickAsync();

            await Context.Message.DeleteAsync();

            var logchannel = Context.Guild.GetChannel(437977945680773130) as SocketTextChannel;
            await logchannel.SendMessageAsync("", false, embed);

            SkrrrtBot.Modules.Statistics.Statistics.AddCommandRequests();
            SkrrrtBot.Modules.Statistics.Statistics.AddOutgoingMessages();
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

            SkrrrtBot.Modules.Statistics.Statistics.AddCommandRequests();
            SkrrrtBot.Modules.Statistics.Statistics.AddOutgoingMessages();
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

            SkrrrtBot.Modules.Statistics.Statistics.AddCommandRequests();
            SkrrrtBot.Modules.Statistics.Statistics.AddOutgoingMessages();
        }
    }
}
