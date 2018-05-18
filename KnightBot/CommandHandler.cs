using System.Threading.Tasks;
using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;
using Discord;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using SkrrrtBot.Config;
using SkrrrtBot.Modules.Public;
using SkrrrtBot.util;
using System.IO;
using SkrrrtBot.Modules.Economy;
using Discord.Rest;
using System.Threading;
using System.Timers;
using SkrrrtBot.Modules.Admin;
using SkrrrtBot.Modules.Profanity;

namespace SkrrrtBot
{
    public class CommandHandler : ModuleBase
    {
        private CommandService commands;
        private static DiscordSocketClient bot;
        private IServiceProvider map;

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
            //bot.MessageReceived += addMoney;
            // Start Logs Lmao
            bot.ChannelCreated += ChannelCreatedAsync;
            bot.ChannelDestroyed += ChannelDeletedAsync;
            bot.RoleCreated += RoleCreatedAsync;
            bot.RoleDeleted += RoleDeletedAsync;
            //bot.RoleUpdated += RoleUpdatedAsync;
            bot.UserBanned += BannedUserAsync;
            bot.UserUnbanned += UnBannedUserAsync;
            bot.GuildUpdated += GuildUpdatedAsync;
            bot.MessageUpdated += MessageUpdatedAsync;
            // End Logs Lmao
            //start msg received stuff
            bot.MessageReceived += ProfanityFilter.ProfanityCheckAsync;
            //end msg recieved stff
        }

        public async Task UserUpdatedAsync(SocketUser user, SocketUser usr)
        {
            var embed = new EmbedBuilder() { Color = Colors.adminCol };
            var footer = new EmbedFooterBuilder() { Text = "SkrrrtBot By SkrrrtSkrrrtStudios" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "SkrrrtSkrrrtStudios.xyz" };
            embed.Title = ("**A User Has Been Updated**");
            embed.Description = ("Username: " + user.Username + "\nUser Id: " + user.Id + "\nTime: " + DateTime.Now.Hour + ":" + DateTime.Now.Minute);
            embed.WithFooter(footer);

            var logchannel = bot.GetChannel(BotConfig.Load().LogChannel) as SocketTextChannel;
            await logchannel.SendMessageAsync("", false, embed);
        }

        public async Task MessageUpdatedAsync(Cacheable<IMessage, ulong> msgid, SocketMessage msg, ISocketMessageChannel chnl)
        {

            var aftermsg = await msgid.GetOrDownloadAsync();

            var embed = new EmbedBuilder() { Color = Colors.adminCol };
            var footer = new EmbedFooterBuilder() { Text = "SkrrrtBot By SkrrrtSkrrrtStudios" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "SkrrrtSkrrrtStudios.xyz" };
            embed.Title = ("**User Edited A Message**");
            embed.Description = ("Username: " + msg.Author + "\nNew Message: " + aftermsg + "\nTime: " + DateTime.Now.Hour + ":" + DateTime.Now.Minute);
            embed.WithFooter(footer);

            var logchannel = bot.GetChannel(BotConfig.Load().LogChannel) as SocketTextChannel;
            await logchannel.SendMessageAsync("", false, embed);
        }

        public async Task BotUpdatedAsync(SocketSelfUser usr, SocketSelfUser user)
        {
            var embed = new EmbedBuilder() { Color = Colors.adminCol };
            var footer = new EmbedFooterBuilder() { Text = "SkrrrtBot By SkrrrtSkrrrtStudios" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "SkrrrtSkrrrtStudios.xyz" };
            embed.Title = ("**The Bot Has Been Updated**");
            embed.Description = ("Bot Name: " + usr.Username + "\nBot Id: " + usr.Id + "\nBot Game: " + usr.Game + "\nTime: " + DateTime.Now.Hour + ":" + DateTime.Now.Minute);
            embed.WithFooter(footer);

            var logchannel = bot.GetChannel(BotConfig.Load().LogChannel) as SocketTextChannel;
            await logchannel.SendMessageAsync("", false, embed);
        }

        public async Task GuildUpdatedAsync(SocketGuild gld, SocketGuild guld)
        {
            var embed = new EmbedBuilder() { Color = Colors.adminCol };
            var footer = new EmbedFooterBuilder() { Text = "SkrrrtBot By SkrrrtSkrrrtStudios" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "SkrrrtSkrrrtStudios.xyz" };
            embed.Title = ("**The Guild Has Been Updated**");
            embed.Description = ("Guild Name: " + gld.Name + "\nGuild Id: " + gld.Id + "\nMember Amount: " + gld.MemberCount + "\nGuild Owner: " + gld.Owner + "\nTime: " + DateTime.Now.Hour + ":" + DateTime.Now.Minute);
            embed.WithFooter(footer);

            var logchannel = bot.GetChannel(BotConfig.Load().LogChannel) as SocketTextChannel;
            await logchannel.SendMessageAsync("", false, embed);
        }

        public async Task BannedUserAsync(SocketUser usr, SocketGuild gld)
        {
            var embed = new EmbedBuilder() { Color = Colors.adminCol };
            var footer = new EmbedFooterBuilder() { Text = "SkrrrtBot By SkrrrtSkrrrtStudios" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "SkrrrtSkrrrtStudios.xyz" };
            embed.Title = ("**User Has Been Banned From The Server**");
            embed.Description = ("Username: " + usr + "\nUser Id: " + usr.Id + "\nTime: " + DateTime.Now.Hour + ":" + DateTime.Now.Minute);
            embed.WithFooter(footer);

            var logchannel = bot.GetChannel(BotConfig.Load().LogChannel) as SocketTextChannel;
            await logchannel.SendMessageAsync("", false, embed);
        }

        public async Task UnBannedUserAsync(SocketUser usr, SocketGuild gld)
        {
            var embed = new EmbedBuilder() { Color = Colors.adminCol };
            var footer = new EmbedFooterBuilder() { Text = "SkrrrtBot By SkrrrtSkrrrtStudios" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "SkrrrtSkrrrtStudios.xyz" };
            embed.Title = ("**User Has Been UnBanned From The Server**");
            embed.Description = ("Username: " + usr + "\nUser Id: " + usr.Id + "\nTime: " + DateTime.Now.Hour + ":" + DateTime.Now.Minute);
            embed.WithFooter(footer);

            var logchannel = bot.GetChannel(BotConfig.Load().LogChannel) as SocketTextChannel;
            await logchannel.SendMessageAsync("", false, embed);
        }

        public async Task RoleCreatedAsync(SocketRole role)
        {
            var embed = new EmbedBuilder() { Color = Colors.adminCol };
            var footer = new EmbedFooterBuilder() { Text = "SkrrrtBot By SkrrrtSkrrrtStudios" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "SkrrrtSkrrrtStudios.xyz" };
            embed.Title = ("**User Created A Role**");
            embed.Description = ("Role Name: " + role.Name + "\nRole Id: " + role.Id + "\nTime: " + DateTime.Now.Hour + ":" + DateTime.Now.Minute);
            embed.WithFooter(footer);

            var logchannel = bot.GetChannel(BotConfig.Load().LogChannel) as SocketTextChannel;
            await logchannel.SendMessageAsync("", false, embed);
        }

        public async Task RoleDeletedAsync(SocketRole role)
        {
            var embed = new EmbedBuilder() { Color = Colors.adminCol };
            var footer = new EmbedFooterBuilder() { Text = "SkrrrtBot By SkrrrtSkrrrtStudios" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "SkrrrtSkrrrtStudios.xyz" };
            embed.Title = ("**User Deleted A Role**");
            embed.Description = ("Role Name: " + role.Name + "\nRole Id: " + role.Id + "\nTime: " + DateTime.Now.Hour + ":" + DateTime.Now.Minute);
            embed.WithFooter(footer);

            var logchannel = bot.GetChannel(BotConfig.Load().LogChannel) as SocketTextChannel;
            await logchannel.SendMessageAsync("", false, embed);
        }

        /*public async Task RoleUpdatedAsync(SocketRole role, SocketRole role2)
        {

            var embed = new EmbedBuilder() { Color = Colors.adminCol };
            var footer = new EmbedFooterBuilder() { Text = "KnightBotV2 By KnightDev" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "SkrrrtSkrrrtStudios.xyz" };

            var blank = new EmbedFieldBuilder() { Name = "\u200b", Value = "\u200b" };
            var roleField = new EmbedFieldBuilder() { Name = "Role Name", Value = role.Name, IsInline = true };
            var roleIdField = new EmbedFieldBuilder() { Name = "Role Id", Value = role.Id, IsInline = true };
            var reacField = new EmbedFieldBuilder() { Name = "Add Reactions", Value = role.Permissions.AddReactions, IsInline = true };
            var adminField = new EmbedFieldBuilder() { Name = "Admin", Value = role.Permissions.Administrator, IsInline = true };
            var atfileField = new EmbedFieldBuilder() { Name = "Attach Files", Value = role.Permissions.AttachFiles, IsInline = true };
            var banField = new EmbedFieldBuilder() { Name = "Can Ban", Value = role.Permissions.BanMembers, IsInline = true };
            var nickField = new EmbedFieldBuilder() { Name = "Change Nickname", Value = role.Permissions.ChangeNickname, IsInline = true };
            var connField = new EmbedFieldBuilder() { Name = "Connect", Value = role.Permissions.Connect, IsInline = true };
            var deafField = new EmbedFieldBuilder() { Name = "Deafen Members", Value = role.Permissions.DeafenMembers, IsInline = true };
            var kickField = new EmbedFieldBuilder() { Name = "Can Kick", Value = role.Permissions.KickMembers, IsInline = true };
            var chnlmngField = new EmbedFieldBuilder() { Name = "Manage Channels", Value = role.Permissions.ManageChannels, IsInline = true };
            var mnggldField = new EmbedFieldBuilder() { Name = "Manage Guild", Value = role.Permissions.ManageGuild, IsInline = true };
            var msgmngField = new EmbedFieldBuilder() { Name = "Manage Messages", Value = role.Permissions.ManageMessages, IsInline = true };
            var nickmngField = new EmbedFieldBuilder() { Name = "Manage Nicknames", Value = role.Permissions.ManageNicknames, IsInline = true };
            var mngrolField = new EmbedFieldBuilder() { Name = "Manage Roles", Value = role.Permissions.ManageRoles, IsInline = true };
            var mnghookField = new EmbedFieldBuilder() { Name = "Manage Webhooks", Value = role.Permissions.ManageWebhooks, IsInline = true };
            var menevryField = new EmbedFieldBuilder() { Name = "Mention Everyone", Value = role.Permissions.MentionEveryone, IsInline = true };
            var mvememField = new EmbedFieldBuilder() { Name = "Move Members", Value = role.Permissions.MoveMembers, IsInline = true };
            var mtememField = new EmbedFieldBuilder() { Name = "Mute Members", Value = role.Permissions.MuteMembers, IsInline = true };
            var msghistField = new EmbedFieldBuilder() { Name = "Read Message History", Value = role.Permissions.ReadMessageHistory, IsInline = true };
            var redmsgField = new EmbedFieldBuilder() { Name = "Read Messages", Value = role.Permissions.ReadMessages, IsInline = true };
            var sndmsgField = new EmbedFieldBuilder() { Name = "Send Messages", Value = role.Permissions.SendMessages, IsInline = true };

            embed.Title = ("**User Updated A Role**");
            embed.Description = ("Info For The Recently Updated Role");
            embed.WithThumbnailUrl("https://www.knightdev.xyz/forums/gifimages/Logo2.png");
            embed.WithFooter(footer);
            embed.AddField(blank);
            embed.AddField(roleField);
            embed.AddField(roleIdField);
            embed.AddField(blank);
            embed.AddField(reacField);
            embed.AddField(adminField);
            embed.AddField(atfileField);
            embed.AddField(banField);
            embed.AddField(nickField);
            embed.AddField(connField);
            embed.AddField(deafField);
            embed.AddField(kickField);
            embed.AddField(chnlmngField);
            embed.AddField(mnggldField);
            embed.AddField(msgmngField);
            embed.AddField(nickmngField);
            embed.AddField(mngrolField);
            embed.AddField(mnghookField);
            embed.AddField(menevryField);
            embed.AddField(mvememField);
            embed.AddField(mtememField);
            embed.AddField(msghistField);
            embed.AddField(redmsgField);
            embed.AddField(sndmsgField);
            embed.AddField(blank);


            var logchannel = bot.GetChannel(BotConfig.Load().LogChannel) as SocketTextChannel;
            await logchannel.SendMessageAsync("", false, embed);
        }*/

        public async Task ChannelCreatedAsync(SocketChannel chnl)
        {
            var embed = new EmbedBuilder() { Color = Colors.adminCol };
            var footer = new EmbedFooterBuilder() { Text = "SkrrrtBot By SkrrrtSkrrrtStudios" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "SkrrrtSkrrrtStudios.xyz" };
            embed.Title = ("**User Created A Channel**");
            embed.Description = ("\nChannel Name: " + chnl + "\nTime: " + DateTime.Now.Hour + ":" + DateTime.Now.Minute);
            embed.WithFooter(footer);

            var logchannel = bot.GetChannel(BotConfig.Load().LogChannel) as SocketTextChannel;
            await logchannel.SendMessageAsync("", false, embed);
        }

        public async Task ChannelDeletedAsync(SocketChannel chnl)
        {
            var embed = new EmbedBuilder() { Color = Colors.adminCol };
            var footer = new EmbedFooterBuilder() { Text = "SkrrrtBot By SkrrrtSkrrrtStudios" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "SkrrrtSkrrrtStudios.xyz" };
            embed.Title = ("**User Deleted A Channel**");
            embed.Description = ("\nChannel Name: " + chnl + "\nTime: " + DateTime.Now.Hour + ":" + DateTime.Now.Minute);
            embed.WithFooter(footer);

            var logchannel = bot.GetChannel(BotConfig.Load().LogChannel) as SocketTextChannel;
            await logchannel.SendMessageAsync("", false, embed);
        }

        
        /*
        public async Task addMoney(SocketMessage msg)
        {
            var result = BankConfig.Load("bank/" + user.Id.ToString() + ".json").currentMoney;

            int bal = 10;

            total = result + bal;

            save.userID = BankConfig.Load("bank/" + user.Id.ToString() + ".json").userID;
            save.currentMoney = total;
            save.currentPoints = BankConfig.Load("bank/" + user.Id.ToString() + ".json").currentPoints;
            save.Save("bank/" + user.Id.ToString() + ".json");
        }
        */

        public async Task AnnounceLeftUser(SocketGuildUser user)
        {

            var embed = new EmbedBuilder() { Color = Colors.adminCol };
            var footer = new EmbedFooterBuilder() { Text = "SkrrrtBot By SkrrrtSkrrrtStudios" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "SkrrrtSkrrrtStudios.xyz" };
            embed.Title = ("**User Left The Discord**");
            embed.Description = ("Username: " + user.Username + "\nTime: " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + "\nTotal Members: " + bot.GetGuild(BotConfig.Load().serverId).MemberCount.ToString());
            embed.WithFooter(footer);

            var logchannel = bot.GetChannel(BotConfig.Load().LogChannel) as SocketTextChannel;
            await logchannel.SendMessageAsync("", false, embed);

        }

        public async Task AnnounceUserJoined(SocketGuildUser user)
        {

            var server = bot.Guilds.FirstOrDefault(x => x.Id == BotConfig.Load().serverId);
            var guild = server as IGuild;
            await user.AddRoleAsync(guild.Roles.FirstOrDefault(x => x.Name == BotConfig.Load().NewMemberRank));

            var embed = new EmbedBuilder() { Color = Colors.adminCol };
            var footer = new EmbedFooterBuilder() { Text = "SkrrrtBot By SkrrrtSkrrrtStudios" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "SkrrrtSkrrrtStudios.xyz" };
            embed.Title = ("**User Joined The Discord**");
            embed.Description = ("Username: " + user.Username + "\nTime: " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + "\nTotal Members: " + bot.GetGuild(BotConfig.Load().serverId).MemberCount.ToString());
            embed.WithFooter(footer);
            

            var logchannel = bot.GetChannel(BotConfig.Load().LogChannel) as SocketTextChannel;
            await logchannel.SendMessageAsync("", false, embed);

            if (!File.Exists(appdir + "bank/" + Context.User.Id.ToString() + ".json"))
            {
                var newServer = File.Create(Path.Combine(appdir, "bank/" + Context.User.Id.ToString() + ".json"));

                newServer.Close();

                save.userID = Context.User.Id.ToString();
                save.currentMoney = 100;
                save.currentPoints = 0;
                save.Save("bank/" + Context.User.Id.ToString() + ".json");

            }

        }

        public async Task SetGame()
        {
            await bot.SetGameAsync(BotConfig.Load().Prefix + "help" + " | " + "SkrrrtSkrrrtStudios.xyz");
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
                {
                    var embed = new EmbedBuilder() { Color = Colors.errorcol };
                    var footer = new EmbedFooterBuilder() { Text = "SkrrrtBot By SkrrrtSkrrrtStudios" + " | " + DateTime.Now.Hour + " | " + DateTime.Now.Minute };
                    embed.Title = ("**Error**");
                    embed.Description = ($"{result.ErrorReason}");
                    embed.WithFooter(footer);
                    await message.Channel.SendMessageAsync("", false, embed);
                }
            }
        }

        public static DiscordSocketClient GetBot() { return bot; }

    }
}