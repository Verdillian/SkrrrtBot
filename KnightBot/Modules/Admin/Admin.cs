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

namespace KnightBot.Modules.Admin
{
    public class Admin : ModuleBase<SocketCommandContext>
    {
        Errors errors = new Errors();
        BotConfig config = new BotConfig();

        [Command("setprefix")]
        [RequireBotPermission(GuildPermission.Administrator)]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AddRole(string prefix)
        {
            if (prefix.Equals(null)) await errors.sendError(Context.Channel, "You need to enter the prefix you want to use!", Colours.adminCol);
            else
            {
                config.Prefix = prefix;
                config.Token = BotConfig.Load().Token;
                config.NewMemberRank = BotConfig.Load().NewMemberRank;
                config.AcceptedMemberRole = BotConfig.Load().AcceptedMemberRole;
                config.MoneyRole = BotConfig.Load().MoneyRole;
                config.MoneyRole1 = BotConfig.Load().MoneyRole1;
                config.MoneyRole2 = BotConfig.Load().MoneyRole2;
                config.NSFWRole = BotConfig.Load().NSFWRole;
                config.Save();

                var embed = new EmbedBuilder() { Color = Colours.adminCol };
                embed.Title = ("Set Prefix");
                embed.Description = ("Prefix has been set to " + prefix + " successfully!");
                await ReplyAsync("", false, embed.Build());
            }
        }
    

        [Command("ban")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task Ban(SocketGuildUser user = null, [Remainder] string reason = null)
        {
            if (user == null) await errors.sendError(Context.Channel, "You must enter a user!", Colours.adminCol);
            if (string.IsNullOrWhiteSpace(reason)) await errors.sendError(Context.Channel, "You must enter a reason!", Colours.adminCol);

            var gld = Context.Guild as SocketGuild;
            var embed = new EmbedBuilder()
            {
                Color = Colours.adminCol
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
            if (user == null) await errors.sendError(Context.Channel, "You must enter the user!", Colours.adminCol);
            if (string.IsNullOrWhiteSpace(reason)) await errors.sendError(Context.Channel, "You must enter a reason!", Colours.adminCol);

            var gld = Context.Guild as SocketGuild;
            var embed = new EmbedBuilder() { Color = Colours.adminCol };

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
            if (user.Equals(null)) await errors.sendError(Context.Channel, "You must enter the user!", Colours.adminCol);
            if (string.IsNullOrWhiteSpace(roleToBe)) await errors.sendError(Context.Channel, "You must enter a role!", Colours.adminCol);

            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == roleToBe);
            await (user as IGuildUser).AddRoleAsync(role);

            var embed = new EmbedBuilder()
            {
                Color = Colours.adminCol
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
            if (user.Equals(null)) await errors.sendError(Context.Channel, "You must enter the user!", Colours.adminCol);
            if (string.IsNullOrWhiteSpace(roleToRemove)) await errors.sendError(Context.Channel, "You must enter a role!", Colours.adminCol);

            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == roleToRemove);
            await (user as IGuildUser).RemoveRoleAsync(role);

            var embed = new EmbedBuilder()
            {
                Color = Colours.adminCol
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
            var moneyrole = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == BotConfig.Load().MoneyRole);
            var moneyrole1 = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == BotConfig.Load().MoneyRole1);
            var moneyrole2 = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == BotConfig.Load().MoneyRole2);

            if (moneyrole != null && moneyrole1 != null && moneyrole2 != null)
            {
                if (userName.Roles.Contains(moneyrole) || userName.Roles.Contains(moneyrole1) || userName.Roles.Contains(moneyrole2))
                {
                    Database.updMoney(user, money);
                    var embed = new EmbedBuilder()
                    {
                        Color = Colours.adminCol
                    };
                    embed.Description = (Context.User.Mention + ", Has Gotton :moneybag: " + money + " Coins!");
                    await ReplyAsync("", false, embed.Build());
                }
                else await errors.sendError(Context.Channel, "You do not have permission to give people dosh!", Colours.adminCol);
            }
            else await errors.sendError(Context.Channel, "Appears money roles are not set up correctly, they are returning null!", Colours.adminCol);
        }

        // End Money

        [Command("rulenew")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ruleNew()
        {
            var config = new BotConfig();

            var embed = new EmbedBuilder()
            {
                Color = Colours.adminCol
            };
            embed.Description = ("**Rules: **\n\n**1. **No Racial Slurs!\n**2. **Do Not Be Rude To Others!\n**3. **Do Not Spam Random Links / Messages!\n**4. **Don't Be An Asshat!\n\n**" + BotConfig.Load().Prefix + "accept** To Accept The Rules!");
            await ReplyAsync("", false, embed.Build());

            await Context.Message.DeleteAsync();
        }
    }
}
