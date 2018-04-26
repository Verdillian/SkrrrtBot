using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using KnightBot.Config;
using KnightBot.util;

namespace KnightBot.Modules.Public
{
    public class PublicModule : ModuleBase
    {
        Errors errors = new Errors();
        public DiscordSocketClient bot;

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

        [Command("pubg")]
        public async Task AddPubgRoleAsync()
        {
            var chan = Context.Channel;
            var userName = Context.User as SocketGuildUser;
            var pubgRole = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == "PlayerUnknowns Battlegrounds");

            if (!userName.Roles.Contains(pubgRole))
            {
                var user = Context.User;

                await (user as IGuildUser).AddRoleAsync(pubgRole);

                var embed = new EmbedBuilder() { Color = Colors.generalCol };
                var footer = new EmbedFooterBuilder() { Text = "KnightBotV2 By KnightDev" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "KnightDev.xyz" };
                embed.Title = ("**" + user.Username + " Was Added To The PUBG Role**");
                embed.WithFooter(footer);

                var messageToDel = await Context.Channel.SendMessageAsync("", false, embed);
                await Delete.DelayDeleteMessage(Context.Message, 10);
                await Delete.DelayDeleteMessage(messageToDel, 10);

                await Context.Message.DeleteAsync();

            }
            else
            {
                var user = Context.User;

                await (user as IGuildUser).RemoveRoleAsync(pubgRole);

                var embed = new EmbedBuilder() { Color = Colors.generalCol };
                var footer = new EmbedFooterBuilder() { Text = "KnightBotV2 By KnightDev" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "KnightDev.xyz" };
                embed.Title = ("**" + user.Username + " Was Removed From The PUBG Role**");
                embed.WithFooter(footer);

                var messageToDel = await Context.Channel.SendMessageAsync("", false, embed);
                await Delete.DelayDeleteMessage(Context.Message, 10);
                await Delete.DelayDeleteMessage(messageToDel, 10);

                await Context.Message.DeleteAsync();

            }
        }

        [Command("gtav")]
        public async Task AddGtavRoleAsync()
        {
            var chan = Context.Channel;
            var userName = Context.User as SocketGuildUser;
            var gtavRole = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == "Grand Theft Auto Five");

            if (!userName.Roles.Contains(gtavRole))
            {
                var user = Context.User;

                await (user as IGuildUser).AddRoleAsync(gtavRole);

                var embed = new EmbedBuilder() { Color = Colors.generalCol };
                var footer = new EmbedFooterBuilder() { Text = "KnightBotV2 By KnightDev" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "KnightDev.xyz" };
                embed.Title = ("**" + user.Username + " Was Added To The GTAV Role**");
                embed.WithFooter(footer);

                var messageToDel = await Context.Channel.SendMessageAsync("", false, embed);
                await Delete.DelayDeleteMessage(Context.Message, 10);
                await Delete.DelayDeleteMessage(messageToDel, 10);

                await Context.Message.DeleteAsync();

            }
            else
            {
                var user = Context.User;

                await (user as IGuildUser).RemoveRoleAsync(gtavRole);

                var embed = new EmbedBuilder() { Color = Colors.generalCol };
                var footer = new EmbedFooterBuilder() { Text = "KnightBotV2 By KnightDev" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "KnightDev.xyz" };
                embed.Title = ("**" + user.Username + " Was Removed From The GTAV Role**");
                embed.WithFooter(footer);

                var messageToDel = await Context.Channel.SendMessageAsync("", false, embed);
                await Delete.DelayDeleteMessage(Context.Message, 10);
                await Delete.DelayDeleteMessage(messageToDel, 10);

                await Context.Message.DeleteAsync();

            }
        }

        [Command("minecraft")]
        public async Task AddMinecraftRoleAsync()
        {
            var chan = Context.Channel;
            var userName = Context.User as SocketGuildUser;
            var mcRole = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == "Minecraft");

            if (!userName.Roles.Contains(mcRole))
            {
                var user = Context.User;

                await (user as IGuildUser).AddRoleAsync(mcRole);

                var embed = new EmbedBuilder() { Color = Colors.generalCol };
                var footer = new EmbedFooterBuilder() { Text = "KnightBotV2 By KnightDev" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "KnightDev.xyz" };
                embed.Title = ("**" + user.Username + " Was Added To The Minecraft Role**");
                embed.WithFooter(footer);

                var messageToDel = await Context.Channel.SendMessageAsync("", false, embed);
                await Delete.DelayDeleteMessage(Context.Message, 10);
                await Delete.DelayDeleteMessage(messageToDel, 10);

                await Context.Message.DeleteAsync();

            }
            else
            {
                var user = Context.User;

                await (user as IGuildUser).RemoveRoleAsync(mcRole);

                var embed = new EmbedBuilder() { Color = Colors.generalCol };
                var footer = new EmbedFooterBuilder() { Text = "KnightBotV2 By KnightDev" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "KnightDev.xyz" };
                embed.Title = ("**" + user.Username + " Was Removed From The Minecraft Role**");
                embed.WithFooter(footer);

                var messageToDel = await Context.Channel.SendMessageAsync("", false, embed);
                await Delete.DelayDeleteMessage(Context.Message, 10);
                await Delete.DelayDeleteMessage(messageToDel, 10);

                await Context.Message.DeleteAsync();

            }
        }

        [Command("r6")]
        public async Task AddR6RoleAsync()
        {
            var chan = Context.Channel;
            var userName = Context.User as SocketGuildUser;
            var r6Role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == "Rainbow Six Seige");

            if (!userName.Roles.Contains(r6Role))
            {
                var user = Context.User;

                await (user as IGuildUser).AddRoleAsync(r6Role);

                var embed = new EmbedBuilder() { Color = Colors.generalCol };
                var footer = new EmbedFooterBuilder() { Text = "KnightBotV2 By KnightDev" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "KnightDev.xyz" };
                embed.Title = ("**" + user.Username + " Was Added To The Rainbow Six Seige Role**");
                embed.WithFooter(footer);

                var messageToDel = await Context.Channel.SendMessageAsync("", false, embed);
                await Delete.DelayDeleteMessage(Context.Message, 10);
                await Delete.DelayDeleteMessage(messageToDel, 10);

                await Context.Message.DeleteAsync();

            }
            else
            {
                var user = Context.User;

                await (user as IGuildUser).RemoveRoleAsync(r6Role);

                var embed = new EmbedBuilder() { Color = Colors.generalCol };
                var footer = new EmbedFooterBuilder() { Text = "KnightBotV2 By KnightDev" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "KnightDev.xyz" };
                embed.Title = ("**" + user.Username + " Was Removed From The Rainbow Six Seige Role**");
                embed.WithFooter(footer);

                var messageToDel = await Context.Channel.SendMessageAsync("", false, embed);
                await Delete.DelayDeleteMessage(Context.Message, 10);
                await Delete.DelayDeleteMessage(messageToDel, 10);

                await Context.Message.DeleteAsync();

            }
        }

        [Command("csgo")]
        public async Task AddcsgoRoleAsync()
        {
            var chan = Context.Channel;
            var userName = Context.User as SocketGuildUser;
            var r6Role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == "CS:GO");

            if (!userName.Roles.Contains(r6Role))
            {
                var user = Context.User;

                await (user as IGuildUser).AddRoleAsync(r6Role);

                var embed = new EmbedBuilder() { Color = Colors.generalCol };
                var footer = new EmbedFooterBuilder() { Text = "KnightBotV2 By KnightDev" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "KnightDev.xyz" };
                embed.Title = ("**" + user.Username + " Was Added To The CS:GO Role**");
                embed.WithFooter(footer);

                var messageToDel = await Context.Channel.SendMessageAsync("", false, embed);
                await Delete.DelayDeleteMessage(Context.Message, 10);
                await Delete.DelayDeleteMessage(messageToDel, 10);

                await Context.Message.DeleteAsync();

            }
            else
            {
                var user = Context.User;

                await (user as IGuildUser).RemoveRoleAsync(r6Role);

                var embed = new EmbedBuilder() { Color = Colors.generalCol };
                var footer = new EmbedFooterBuilder() { Text = "KnightBotV2 By KnightDev" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "KnightDev.xyz" };
                embed.Title = ("**" + user.Username + " Was Removed From The CS:GO Role**");
                embed.WithFooter(footer);

                var messageToDel = await Context.Channel.SendMessageAsync("", false, embed);
                await Delete.DelayDeleteMessage(Context.Message, 10);
                await Delete.DelayDeleteMessage(messageToDel, 10);

                await Context.Message.DeleteAsync();

            }
        }

        [Command("rktlgue")]
        [Alias("rocketleague")]
        public async Task AddRocketRoleAsync()
        {
            var chan = Context.Channel;
            var userName = Context.User as SocketGuildUser;
            var r6Role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == "Rocket League");

            if (!userName.Roles.Contains(r6Role))
            {
                var user = Context.User;

                await (user as IGuildUser).AddRoleAsync(r6Role);

                var embed = new EmbedBuilder() { Color = Colors.generalCol };
                var footer = new EmbedFooterBuilder() { Text = "KnightBotV2 By KnightDev" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "KnightDev.xyz" };
                embed.Title = ("**" + user.Username + " Was Added To The Rocket League Role**");
                embed.WithFooter(footer);

                var messageToDel = await Context.Channel.SendMessageAsync("", false, embed);
                await Delete.DelayDeleteMessage(Context.Message, 10);
                await Delete.DelayDeleteMessage(messageToDel, 10);

                await Context.Message.DeleteAsync();

            }
            else
            {
                var user = Context.User;

                await (user as IGuildUser).RemoveRoleAsync(r6Role);

                var embed = new EmbedBuilder() { Color = Colors.generalCol };
                var footer = new EmbedFooterBuilder() { Text = "KnightBotV2 By KnightDev" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "KnightDev.xyz" };
                embed.Title = ("**" + user.Username + " Was Removed From The Rocket League Role**");
                embed.WithFooter(footer);

                var messageToDel = await Context.Channel.SendMessageAsync("", false, embed);
                await Delete.DelayDeleteMessage(Context.Message, 10);
                await Delete.DelayDeleteMessage(messageToDel, 10);

                await Context.Message.DeleteAsync();

            }
        }

        [Command("movie")]
        public async Task AddMovieRoleAsync()
        {
            var chan = Context.Channel;
            var userName = Context.User as SocketGuildUser;
            var r6Role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == "Movies");

            if (!userName.Roles.Contains(r6Role))
            {
                var user = Context.User;

                await (user as IGuildUser).AddRoleAsync(r6Role);

                var embed = new EmbedBuilder() { Color = Colors.generalCol };
                var footer = new EmbedFooterBuilder() { Text = "KnightBotV2 By KnightDev" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "KnightDev.xyz" };
                embed.Title = ("**" + user.Username + " Was Added To The Movies Role**");
                embed.WithFooter(footer);

                var messageToDel = await Context.Channel.SendMessageAsync("", false, embed);
                await Delete.DelayDeleteMessage(Context.Message, 10);
                await Delete.DelayDeleteMessage(messageToDel, 10);

                await Context.Message.DeleteAsync();

            }
            else
            {
                var user = Context.User;

                await (user as IGuildUser).RemoveRoleAsync(r6Role);

                var embed = new EmbedBuilder() { Color = Colors.generalCol };
                var footer = new EmbedFooterBuilder() { Text = "KnightBotV2 By KnightDev" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "KnightDev.xyz" };
                embed.Title = ("**" + user.Username + " Was Removed From The Movies Role**");
                embed.WithFooter(footer);

                var messageToDel = await Context.Channel.SendMessageAsync("", false, embed);
                await Delete.DelayDeleteMessage(Context.Message, 10);
                await Delete.DelayDeleteMessage(messageToDel, 10);

                await Context.Message.DeleteAsync();

            }
        }

        [Command("developer")]
        [Alias("dev")]
        public async Task DeveloperEmbed()
        {
            var embed = new EmbedBuilder() { Color = Colors.generalCol };
            var footer = new EmbedFooterBuilder() { Text = "KnightBotV2 By KnightDev" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "KnightDev.xyz" };

            var blank = new EmbedFieldBuilder() { Name = "\u200b", Value = "\u200b" };
            var devField = new EmbedFieldBuilder() { Name = "Developer", Value = "Knight", IsInline = true };
            var botInfoField = new EmbedFieldBuilder() { Name = "Bot Info", Value = "This is a simple moderation/fun bot for my discord server. If you are interested in having one made for you feel free to contact me via my website below.", IsInline = true };
            var websiteField = new EmbedFieldBuilder() { Name = "Website", Value = "Https://www.KnightDev.xyz/", IsInline = true };
            var githubField = new EmbedFieldBuilder() { Name = "Github", Value = "https://github.com/Knight-Dev", IsInline = true };

            embed.Title = ("**Developer Information**");
            embed.Description = ("Info About The Developer");
            embed.WithThumbnailUrl("https://www.knightdev.xyz/forums/gifimages/Logo2.png");
            embed.WithFooter(footer);
            embed.AddField(devField);
            embed.AddField(botInfoField);
            embed.AddField(blank);
            embed.AddField(websiteField);
            embed.AddField(githubField);

            await Context.Channel.SendMessageAsync("", false, embed);
        }



    }
}