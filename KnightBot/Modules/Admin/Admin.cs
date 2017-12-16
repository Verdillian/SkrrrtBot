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
    public class Admin : ModuleBase   <SocketCommandContext> 
    {
        [Command("ban")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task Ban(SocketGuildUser user = null, [Remainder] string reason = null)
        {
            if (user.Equals(null)) throw new ArgumentException("You Must Mention A User!");
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

        }

        [Command("kick")]
        [RequireBotPermission(GuildPermission.KickMembers)]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task Kick(SocketGuildUser user = null, [Remainder] string reason = null)
        {
            if (user.Equals(null)) throw new ArgumentException("You Must Mention A User!");
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

        }

    }
}
