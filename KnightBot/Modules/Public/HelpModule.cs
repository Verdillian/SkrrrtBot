using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using KnightBot.Config;
using KnightBot.util;

namespace KnightBot.Modules.Public
{
    public class HelpModule : ModuleBase
    {
        Errors errors = new Errors();
        
        [Command("help")]
        public async Task Help(string type = null)
        {
            var chan = Context.Channel;
            var userName = Context.User as SocketGuildUser;

            if (type == null)
            {
                var embed = new EmbedBuilder() { Color = new Color(0, 0, 230) };
                var footer = new EmbedFooterBuilder() { Text = "Requested by " + Context.User.Username };

                embed.Title = $"Knight Help";
                embed.Description = BotConfig.Load().Prefix + "help general\n" +
                                    BotConfig.Load().Prefix + "help music\n" +
                                    BotConfig.Load().Prefix + "help bank\n" +
                                    BotConfig.Load().Prefix + "help auction\n" +
                                    BotConfig.Load().Prefix + "help admin\n" +
                                    BotConfig.Load().Prefix + "help nsfw\n";
                embed.WithFooter(footer);
                embed.WithCurrentTimestamp();
                await Context.Channel.SendMessageAsync("", false, embed);
            }
            //
            else if (type.Equals("general") || type.Equals("gen"))
            {
                var embed = new EmbedBuilder() { Color = Colours.generalCol };
                var footer = new EmbedFooterBuilder() { Text = "Requested by " + Context.User.Username };
                var helpField = new EmbedFieldBuilder() { Name = BotConfig.Load().Prefix + "help", Value = "Help for using KnightBot." };
                var doggoField = new EmbedFieldBuilder() { Name = BotConfig.Load().Prefix + "doggo", Value = "Displays a random image of a dog." };
                var catField = new EmbedFieldBuilder() { Name = BotConfig.Load().Prefix + "cat", Value = "Displays a random image of a cat." };

                embed.Title = $"General Help";
                embed.Description = "More commands to be added to this list soon!";
                embed.WithFooter(footer);
                embed.WithCurrentTimestamp();
                embed.AddField(helpField);
                embed.AddField(doggoField);
                embed.AddField(catField);

                await Context.Channel.SendMessageAsync("", false, embed);
            }
            //
            else if (type.Equals("music") || type.Equals("songs"))
            {
                var embed = new EmbedBuilder() { Color = Colours.musicCol };
                var footer = new EmbedFooterBuilder() { Text = "Requested by " + Context.User.Username };
                var playField = new EmbedFieldBuilder() { Name = BotConfig.Load().Prefix + "play <link to song on youtube (or pornhub)>", Value = "Plays the song in your voice channel." };
                var stopField = new EmbedFieldBuilder() { Name = BotConfig.Load().Prefix + "stop", Value = "Stops the song that is currently playing." };

                embed.Title = $"Music Help";
                embed.Description = "Here are all the music commands.";
                embed.WithFooter(footer);
                embed.WithCurrentTimestamp();
                embed.AddField(playField);
                embed.AddField(stopField);
                await Context.Channel.SendMessageAsync("", false, embed);
            }
            //
            else if (type.Equals("bank") || type.Equals("money"))
            {
                var embed = new EmbedBuilder() { Color = Colours.moneyCol };
                var footer = new EmbedFooterBuilder() { Text = "Requested by " + Context.User.Username };
                var bankField = new EmbedFieldBuilder() { Name = BotConfig.Load().Prefix + "bank open", Value = "Opens a bank account in your name." };
                var moneyField = new EmbedFieldBuilder() { Name = BotConfig.Load().Prefix + "bank balance", Value = "Displays your bank balance." };
                var transferField = new EmbedFieldBuilder() { Name = BotConfig.Load().Prefix + "bank transfer <user> <amount>", Value = "Transfer money to another player." };

                embed.Title = $"Bank Help";
                embed.Description = "All of the bank commands.";
                embed.WithFooter(footer);
                embed.WithCurrentTimestamp();
                embed.AddField(bankField);
                embed.AddField(moneyField);
                embed.AddField(transferField);

                await Context.Channel.SendMessageAsync("", false, embed);
            }
            //
            else if (type.Equals("auction") || type.Equals("bids"))
            {
                var embed = new EmbedBuilder() { Color = Colours.moneyCol };
                var footer = new EmbedFooterBuilder() { Text = "Requested by " + Context.User.Username };
                var auctionField = new EmbedFieldBuilder() { Name = BotConfig.Load().Prefix + "auction <amount> <quantity> <item>", Value = "Starts a new auction." };
                var auctionEndField = new EmbedFieldBuilder() { Name = BotConfig.Load().Prefix + "auctionend", Value = "Ends the current auction." };
                var auctionCheckField = new EmbedFieldBuilder() { Name = BotConfig.Load().Prefix + "auctioncheck", Value = "Checks if there is a current auction." };
                var bidField = new EmbedFieldBuilder() { Name = BotConfig.Load().Prefix + "bid <amount>", Value = "Bid on the current auction." };

                embed.Title = $"Auction Help";
                embed.Description = "All of the auction commands.";
                embed.WithFooter(footer);
                embed.WithCurrentTimestamp();
                embed.AddField(auctionField);
                embed.AddField(auctionEndField);
                embed.AddField(auctionCheckField);
                embed.AddField(bidField);

                await Context.Channel.SendMessageAsync("", false, embed);
            }
            //
            else if (type.Equals("admin") || type.Equals("administrative"))
            {
                var embed = new EmbedBuilder() { Color = Colours.adminCol };
                var footer = new EmbedFooterBuilder() { Text = "Requested by " + Context.User.Username };
                var kickField = new EmbedFieldBuilder() { Name = BotConfig.Load().Prefix + "kick <user> <reason>", Value = "Kicks the user specified." };
                var banField = new EmbedFieldBuilder() { Name = BotConfig.Load().Prefix + "ban <user> <reason>", Value = "Bans the user specified." };

                embed.Title = $"Admin Help";
                embed.Description = "All of the admin commands.";
                embed.WithFooter(footer);
                embed.WithCurrentTimestamp();
                embed.AddField(kickField);
                embed.AddField(banField);

                await Context.Channel.SendMessageAsync("", false, embed);
            }
            //
            else if (type.Equals("nsfw") || type.Equals("18"))
            {
                var embed = new EmbedBuilder() { Color = Colours.nsfwCol };
                var footer = new EmbedFooterBuilder() { Text = "Requested by " + Context.User.Username };
                var joinField = new EmbedFieldBuilder() { Name = BotConfig.Load().Prefix + "nsfw join", Value = "Adds the nsfw role to you." };
                var leaveField = new EmbedFieldBuilder() { Name = BotConfig.Load().Prefix + "nsfw leave", Value = "Removes the nsfw role from you." };
                var buttField = new EmbedFieldBuilder() { Name = BotConfig.Load().Prefix + "nsfw butt", Value = "Perfect for any butt lovers." };
                var boobsField = new EmbedFieldBuilder() { Name = BotConfig.Load().Prefix + "nsfw boobs", Value = "Perfect for any boobs lovers." };
                var gifField = new EmbedFieldBuilder() { Name = BotConfig.Load().Prefix + "nsfw gif", Value = "Sends a random gif ;)" };

                embed.Title = $"NSFW Help";
                embed.Description = "Help for all the nsfw commands.";
                embed.WithFooter(footer);
                embed.WithCurrentTimestamp();
                embed.AddField(joinField);
                embed.AddField(leaveField);
                embed.AddField(buttField);
                embed.AddField(boobsField);
                embed.AddField(gifField);
                await Context.Channel.SendMessageAsync("", false, embed);
            }
        }
    }
}
