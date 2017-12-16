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

namespace KnightBot.Modules.Public
{
    public class PublicModule : ModuleBase
    {



        [Command("test")] //Command Name
        [Remarks("Tests your bot to see if it worked ;)")] //Summary for your command. it will not add anything.
        public async Task Test()
        {
            try // Try the following code, if failed - move to "catch"
            {
                await ReplyAsync("Test Command Successful"); //makes the bot reply back!      
            }
            catch (Exception e) // catch exception error, reply with the error in string format
            {
                await ReplyAsync(e.ToString());
            }
        }

        [Command("ping")]
        [Remarks("Play ping pong with the bot")]
        public async Task Ping()
        {
            try
            {
                await ReplyAsync("Pong!");
            }
            catch (Exception e)
            {
                await ReplyAsync(e.ToString());
            }
        }



        [Command("setgame")]
        [Remarks("Sets the game the bot is currently playing")]
        public async Task Setgame([Remainder] string game)
        {
            var GuildUser = await Context.Guild.GetUserAsync(Context.User.Id);
            if (!(GuildUser.Id == 146377960360902656))
            {
                await Context.Channel.SendMessageAsync("You Cannot Change What Game I Play!");
            }
            else
            {
                await (Context.Client as DiscordSocketClient).SetGameAsync(game);
                await Context.Channel.SendMessageAsync($"Successfully Set The Game To *{game}*");
                Console.WriteLine($"{DateTime.Now} : Game was changed to {game}");
            }
        }

        string[] predictionTexts = new string[]
        {
            "It Is Very Unlikely.",
            "I Don't Think So...",
            "Yes!",
            "I Don't Know",
            "No!",
        };

        Random rand = new Random();

        [Command("8ball")]
        [Remarks("Gives A Prediction")]
        public async Task EightBall([Remainder] string input)
        {
            int randomIndex = rand.Next(predictionTexts.Length);
            string text = predictionTexts[randomIndex];
            await ReplyAsync(Context.User.Mention + ", " + text);
        }

        static string player1;
        static string player2;
        static string whosTurn;
        static string whoWaits;
        static string placeHolder;
        static int health1 = 100;
        static int health2 = 100;
        static string SwitchCaseString = "nofight";

        [Command("fight")]
        [Remarks("Starts a fight with the @mention user (example: !fight knight")]
        public async Task Fight(IUser user)
        {
            if (Context.User.Mention != user.Mention && SwitchCaseString == "nofight")
            {
                SwitchCaseString = "fight_p1";
                player1 = Context.User.Mention;
                player2 = user.Mention;

                string[] whoStarts = new string[]
                {
                    Context.User.Mention,
                user.Mention
                };

                Random rand = new Random();

                int randomIndex = rand.Next(whoStarts.Length);
                string text = whoStarts[randomIndex];

                whosTurn = text;
                if (text == Context.User.Mention)
                {
                    whoWaits = user.Mention;
                }
                else
                {
                    whoWaits = Context.User.Mention;
                }

                await ReplyAsync("Fight started between " + Context.User.Mention + " and " + user.Mention + "!\n\n" + player1 + " you have " + health1 + " health!\n" + player2 + " you have " + health2 + " health!\n\n" + text + " your turn!");
            }
            else
            {
                await ReplyAsync(Context.User.Mention + " Sorry but there is already a fight going on, or  you simply tried to fight yourself.");
            }
        }

        [Command("giveup")]
        [Alias("GiveUp", "Giveup", "giveup")]
        [Remarks("Stops the fight and gives up.")]
        public async Task GiveUp()
        {
            if (SwitchCaseString == "fight_p1")
            {
                await ReplyAsync("The Fight Has Stopped!");
                SwitchCaseString = "nofight";
                health1 = 100;
                health2 = 100;
            }
            else
            {
                await ReplyAsync("There is no fight to stop.");
            }
        }

        [Command("slash")]
        [Alias("Slash")]
        [Remarks("Slashes your foe with a sword. Good accuracy and medium damage.")]
        public async Task Slash()
        {
            if (SwitchCaseString == "fight_p1")
            {
                if (whosTurn == Context.User.Mention)
                {
                    Random rand = new Random();

                    int randomIndex = rand.Next(1, 6);
                    if (randomIndex != 1)
                    {
                        Random rand2 = new Random();

                        int randomIndex2 = rand2.Next(7, 15);

                        if (Context.User.Mention != player1)
                        {
                            health1 = health1 - randomIndex2;
                            if (health1 > 0)
                            {
                                placeHolder = whosTurn;
                                whosTurn = whoWaits;
                                whoWaits = placeHolder;
                                await ReplyAsync(Context.User.Mention + " you hit and did " + randomIndex2 + " damage!\n\n" + player1 + " has " + health1 + " health left!\n" + player2 + " has " + health2 + " health left!\n" + whosTurn + " its your turn!");
                            }
                            else
                            {
                                await ReplyAsync(Context.User.Mention + " you hit and did " + randomIndex2 + " damage!\n\n" + player1 + " died. " + player2 + " won!");
                                SwitchCaseString = "nofight;";
                                health1 = 100;
                                health2 = 100;
                            }
                        }
                        else if (Context.User.Mention == player1)
                        {
                            health2 = health2 - randomIndex2;
                            if (health2 > 0)
                            {
                                placeHolder = whosTurn;
                                whosTurn = whoWaits;
                                whoWaits = placeHolder;
                                await ReplyAsync(Context.User.Mention + " you hit and did " + randomIndex2 + " damage!\n\n" + player1 + " has " + health1 + " health left!\n" + player2 + " has " + health2 + " health left!\n" + whosTurn + " its your turn!");

                            }
                            else
                            {
                                await ReplyAsync(Context.User.Mention + " you hit and did " + randomIndex2 + " damage!\n\n" + player2 + " died. " + player1 + " won!");
                                SwitchCaseString = "nofight";
                                health1 = 100;
                                health2 = 100;
                            }
                        }
                        else
                        {
                            await ReplyAsync("Sorry it seems like something went wrong! Please type !giveup");
                        }
                    }
                    else
                    {
                        placeHolder = whosTurn;
                        whosTurn = whoWaits;
                        whoWaits = placeHolder;

                        await ReplyAsync(Context.User.Mention + " sorry you missed!\n" + whosTurn + " your turn!");
                    }
                }
                else
                {
                    await ReplyAsync(Context.User.Mention + " it is not your turn!");
                }
            }
            else
            {
                await ReplyAsync("There is no fight at the moment. Sorry :/");
            }
        }

        [Command("clear")]
        [Alias("c")]
        [Remarks("Clears all messages in a channel")]
        public async Task Clear()
        {
            var items = await Context.Channel.GetMessagesAsync().Flatten();
            await Context.Channel.DeleteMessagesAsync(items);
        }

        private Process CreateStream(string url)
        {
            Process currentsong = new Process();

            currentsong.StartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/C youtube-dl.exe -o - {url} | ffmpeg -i pipe:0 -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            currentsong.Start();
            return currentsong;

        }



        




        [Command("play", RunMode = RunMode.Async)]
        public async Task play(string url)
        {
            var embed = new EmbedBuilder()
            {
                Color = new Color(0, 175, 240)
            };
            embed.Description = (Context.User.Mention + ", Has decided to listen to music!");

            await Context.Channel.SendMessageAsync("", false, embed);

            await SendLinkAsync(Context.Guild, url);

        }


        public async Task SendLinkAsync(IGuild guild, string url)
        {

            IVoiceChannel channel = (Context.User as IVoiceState).VoiceChannel;
            IAudioClient client = await channel.ConnectAsync();

            var output = CreateStream(url).StandardOutput.BaseStream;
            var stream = client.CreatePCMStream(AudioApplication.Music, 128 * 1024);
            await output.CopyToAsync(stream);
            await stream.FlushAsync().ConfigureAwait(false);
        }

        [Command("stop", RunMode = RunMode.Async)]
        public async Task StopCmd()
        {
            await StopAudio(Context.Guild);
        }


        public async Task StopAudio(IGuild guild)
        {
            IVoiceChannel channel = (Context.User as IVoiceState).VoiceChannel;
            IAudioClient client = await channel.ConnectAsync();

            await client.StopAsync();
            return;
        }


        [Command("status")]
        public async Task Status([Remainder] IUser user = null)
        {
            var embed = new EmbedBuilder()
            {
                Color = new Color(0, 175, 240)
            };
            if (user == null)
            {
                user = Context.User;
            }

            var result = Database.CheckExistingUser(user);

            if (result.Count() <= 0)
            {
                Database.EnterUser(user);
            }

            var tableName = Database.GetUserStatus(user);

            embed.Description = (Context.User.Mention + "'s current status is as follows: \n" + ":small_blue_diamond:" + "UserID: " + tableName.FirstOrDefault().UserId + "\n" + ":small_blue_diamond:" + tableName.FirstOrDefault().Tokens + " to spend!");

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("award")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Award(SocketGuildUser user, [Remainder] int tokens)
        {

            var embed = new EmbedBuilder()
            {
                Color = new Color(0, 175, 240)
            };

            Database.ChangeTokens(user, tokens);
            embed.Description = (user.Mention + ", was awarded " + tokens + " tokens!");

            await Context.Channel.SendMessageAsync("", false, embed);
        }

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
                Color = new Color(0, 175, 240)
            };

            embed.Description = $"{dm}";
            embed.WithFooter(new EmbedFooterBuilder().WithText($"Message From: {Context.User.Username} | Guild: {Context.Guild.Name}"));
            await message.SendMessageAsync("", false, embed);
            embed.Description = $"You have sent a message to {myId} he will read the message soon.";
            embed.WithFooter(new EmbedFooterBuilder().WithText($"Message Has Been Sent Succesfully!"));

            await Context.Channel.SendMessageAsync("", false, embed);

        }



        // Start Auction System

        static string auctionCheck = "";
        static ulong currentAuction = 0;
        static int hightBid;
        static SocketGuildUser highBidder = null;
        static string currentItem;

        [Command("auction")]
        public async Task Auction(int startingBid, int amount, string item, [Remainder]string info = null)
        {

            if (auctionCheck.Equals("live"))
            {
                await ReplyAsync("A auction is currently live!");
            }
            else
            {

                if (auctionCheck.Equals("") || auctionCheck.Equals("over")) auctionCheck.Equals("Live");
                else if (auctionCheck == "live")
                {
                    var messageToDel = await ReplyAsync("Auction already started, only auction can be held at a time!");
                    await DelayDeleteMessage(Context.Message, 10);
                    await DelayDeleteMessage(messageToDel, 10);
                    return;
                }
                var embed = new EmbedBuilder();

                embed.AddField(x =>
                {
                    x.Name = $"Auction for {item} started {DateTime.UtcNow} UTC ";
                    x.Value = $"{amount} x {item} is up for auction with a starting bid of {startingBid}\nType !bid [amount] to bid.";
                });
                var message = await ReplyAsync("", embed: embed);
                hightBid = startingBid - 1;

                currentItem = item;

                currentAuction = message.Id;
            }
        }

        [Command("auctionend")]
        public async Task Auctionover()
        {
            if (auctionCheck.Equals("over"))
            {
                await ReplyAsync("There is no auction right now.");
            }
            else
            {

                if (highBidder == null)
                {
                    await Context.Channel.SendMessageAsync("The auction has ended with 0 bids.");
                    auctionCheck = "over";
                    hightBid = 0;
                    currentItem = null;
                    currentAuction = 0;
                    return;
                }
                var embed = new EmbedBuilder();
                embed.Title = $"{highBidder} won the auction for {currentItem}";
                embed.AddField(x =>
                {
                    x.Name = $"Auction ended at {DateTime.UtcNow} UTC";
                    x.Value = $"Once you pay {hightBid}, we will arrage payment and delivery of {currentItem} soon after. \n Congrats! :tada: ";
                    x.IsInline = false;
                });
                auctionCheck = "over";
                highBidder = null;
                hightBid = 0;
                currentItem = null;
                currentAuction = 0;
                await Context.Channel.SendMessageAsync("", false, embed);
            }
        }

        [Command("auctioncheck")]
        public async Task Auctioncheck()
        {
            var DM = await Context.User.GetOrCreateDMChannelAsync();
            if (auctionCheck.Equals("over") || auctionCheck.Equals(""))
            {
                var message = await DM.SendMessageAsync("Sorry there isn't an auction at this time.");
                await DelayDeleteMessage(Context.Message, 10);
                await DelayDeleteMessage(message, 10);
                return;
            }
            var auctionStatus = Context.Channel.GetMessageAsync(currentAuction);

            var embed = auctionStatus.Result.Embeds.FirstOrDefault() as Embed;
            await DM.SendMessageAsync("", false, embed);
            await DelayDeleteMessage(Context.Message, 10);
        }

        [Command("bid")]
        public async Task Bid(string amount)
        {
            int bid;
            if (auctionCheck.Equals("live"))
            {
                var message = await ReplyAsync("There's no auction at this time, ask someone to start one.");
                await DelayDeleteMessage(Context.Message, 10);
                await DelayDeleteMessage(message, 10);
                return;
            }
            else
            {
                try
                {
                    bid = Math.Abs(int.Parse(amount));
                }
                catch
                {
                    var message = await ReplyAsync("That is not a valid bid, I am not stupid!");
                    await DelayDeleteMessage(Context.Message, 10);
                    await DelayDeleteMessage(message, 10);
                    return;
                }
                if (bid <= hightBid)
                {
                    var message = await ReplyAsync("Your bid is too low, increase it and try again.");
                    await DelayDeleteMessage(Context.Message, 10);
                    await DelayDeleteMessage(message, 10);
                    return;
                }

                hightBid = bid;
                highBidder = Context.User as SocketGuildUser;

                await UpDateHighBidder(Context.Message as SocketUserMessage, bid);

                var message2 = await ReplyAsync($"The current highest bidder is {highBidder.Mention} with a bid of {hightBid} :moneybag: ");
                await DelayDeleteMessage(Context.Message, 10);
                await DelayDeleteMessage(message2, 10);
            }
        }

        private async Task UpDateHighBidder(SocketUserMessage messageDetails, int bid)
        {
            var exactMessage = await messageDetails.Channel.GetMessageAsync(currentAuction) as IUserMessage;
            var embed2 = new EmbedBuilder();
            var oldField = exactMessage.Embeds.FirstOrDefault().Fields.FirstOrDefault();

            embed2.AddField(x =>
            {
                x.Name = oldField.Name;
                x.Value = oldField.Value;
                x.IsInline = oldField.Inline;
            });

            embed2.AddField(x =>
            {
                x.Name = "New Highest Bid!";
                x.IsInline = false;
                x.Value = $"{currentItem} highest bid is {bid} by {highBidder}.";
            });
        }

        private async Task DelayDeleteMessage(IUserMessage message, int time = 0)
        {
            var delete = Task.Run(async () =>
            {
                if (time == 0) await Task.Delay(2500);
                else await Task.Delay(time * 1000);

                await message.DeleteAsync();
            });
        }

        // End Auction System




        // Start help

        [Command("help")]
        [Remarks("This shows what commands you can use with the bot")]
        public async Task Help()
        {
            var embed = new EmbedBuilder()
            {
                Color = new Color(0, 175, 240)
            };


            embed.Title = ("**╋━━━━━━◥◣ Help ◢◤━━━━━━╋**");
            embed.Description = ("**\n!dm** : Send a DM to the server owner.\n**!fight** : Start a fight with someone. \n**   - !slash** : Slash your enemy whilst fighting! \n**   - !giveup** : Give up the fight... \n**!status** : See how many coins you have!\n**!play** : Lets you play music in a voice channel!\n\n**╋━━━━━━◥◣ Help ◢◤━━━━━━╋**");

            await Context.Channel.SendMessageAsync("", false, embed);

        }

        // End help


        










    }
}