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
using KnightBot.util;
using KnightBot;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Linq;

namespace KnightBot.Modules.Public
{
    public class PublicModule : ModuleBase
    {
        private ImageSharp.Image image = null;
        private string randomString = "";

        [Command("help")]
        public async Task Help(string type = null)
        {
            var chan = Context.Channel;
            var config = new BotConfig();
            var userName = Context.User as SocketGuildUser;

            if (type == null)
            {
                var embed = new EmbedBuilder()
                {
                    Color = new Color(0, 0, 230)
                };
                embed.Title = $"Knight Help";
                embed.Description = config.Prefix.ToString() + "help general\n" +
                                    config.Prefix.ToString() + "help music\n" +
                                    config.Prefix.ToString() + "help bank\n" +
                                    config.Prefix.ToString() + "help auction\n" +
                                    config.Prefix.ToString() + "help admin\n" +
                                    config.Prefix.ToString() + "help nsfw\n";
                await Context.Channel.SendMessageAsync("", false, embed);
            }
            //
            else if (type.Equals("general") || type.Equals("gen"))
            {
                var embed = new EmbedBuilder()
                {
                    Color = Colours.generalCol
                };
                embed.Title = $"General Help";
                embed.Description = config.Prefix.ToString() + "doggo\n" +
                                    config.Prefix.ToString() + "cat\n" +
                                    config.Prefix.ToString() + "commands to be added to help.";
                await Context.Channel.SendMessageAsync("", false, embed);
            }
            //
            else if (type.Equals("music") || type.Equals("songs"))
            {
                var embed = new EmbedBuilder()
                {
                    Color = Colours.musicCol
                };
                embed.Title = $"Music Help";
                embed.Description = config.Prefix.ToString() + "play\n" +
                                    config.Prefix.ToString() + "stop\n" +
                                    config.Prefix.ToString() + "commands to be added to help.";
                await Context.Channel.SendMessageAsync("", false, embed);
            }
            //
            else if (type.Equals("bank") || type.Equals("money"))
            {
                var embed = new EmbedBuilder()
                {
                    Color = Colours.moneyCol
                };
                embed.Title = $"Bank Help";
                embed.Description = config.Prefix.ToString() + "commands to be added to help.";
                await Context.Channel.SendMessageAsync("", false, embed);
            }
            //
            else if (type.Equals("auction") || type.Equals("bids"))
            {
                var embed = new EmbedBuilder()
                {
                    Color = Colours.moneyCol
                };
                embed.Title = $"Auction Help";
                embed.Description = config.Prefix.ToString() + "commands to be added to help.";
                await Context.Channel.SendMessageAsync("", false, embed);
            }
            //
            else if (type.Equals("admin") || type.Equals("administrative"))
            {
                var embed = new EmbedBuilder()
                {
                    Color = Colours.adminCol
                };
                embed.Title = $"Admin Help";
                embed.Description = config.Prefix.ToString() + "commands to be added to help.";
                await Context.Channel.SendMessageAsync("", false, embed);
            }
            //
            else if (type.Equals("nsfw") || type.Equals("18"))
            {
                var embed = new EmbedBuilder()
                {
                    Color = Colours.nsfwCol
                };
                embed.Title = $"NSFW Help";
                embed.Description = config.Prefix.ToString() + "nsfw join        -  Join the NSFW Role to view NSFW channels.\n" +
                                    config.Prefix.ToString() + "nsfw butt       -  Displays a random butt pic.\n" +
                                    config.Prefix.ToString() + "nsfw boobs   -  Displays a random boobs pic.\n" +
                                    config.Prefix.ToString() + "nsfw gif          -  Displays a random sexy gif.\n";
                await Context.Channel.SendMessageAsync("", false, embed);
            }

        }


        [Command("setgame")]
        [Remarks("Sets the game the bot is currently playing")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Setgame()
        {
            await (Context.Client as DiscordSocketClient).SetGameAsync("KnightBot.xyz");


            await Context.Message.DeleteAsync();
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

            var embed = new EmbedBuilder()
            {
                Color = Colours.ballCol
            };

            embed.Title = "**╋━━━━━━◥◣ Magic 8 Ball ◢◤━━━━━━╋**";
            embed.Description = Environment.NewLine + Context.User.Mention + ", " + text;

            await Context.Channel.SendMessageAsync("", false, embed);

            await Context.Message.DeleteAsync();
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

                var embed = new EmbedBuilder()
                {
                    Color = Colours.fightCol
                };
                embed.Description = ("Fight started between " + Context.User.Mention + " and " + user.Mention + "!\n\n" + player1 + " you have " + health1 + " health!\n" + player2 + " you have " + health2 + " health!\n\n" + text + " your turn!");
                await ReplyAsync("", false, embed.Build());

            }
            else
            {

                var embed = new EmbedBuilder()
                {
                    Color = Colours.fightCol
                };
                embed.Description = (Context.User.Mention + " Sorry but there is already a fight going on, or  you simply tried to fight yourself.");
                await ReplyAsync("", false, embed.Build());
            }
            await Context.Message.DeleteAsync();
        }

        [Command("giveup")]
        [Alias("GiveUp", "Giveup", "giveup")]
        [Remarks("Stops the fight and gives up.")]
        public async Task GiveUp()
        {
            if (SwitchCaseString == "fight_p1")
            {
                var embed = new EmbedBuilder()
                {
                    Color = Colours.fightCol
                };
                embed.Description = ("The Fight Has Stopped!");
                await ReplyAsync("", false, embed.Build());

                SwitchCaseString = "nofight";
                health1 = 100;
                health2 = 100;
            }
            else
            {
                var embed = new EmbedBuilder()
                {
                    Color = Colours.fightCol
                };
                embed.Description = (Context.User.Mention  + ", There is no fight to stop.");
                await ReplyAsync("", false, embed.Build());
            }
            await Context.Message.DeleteAsync();
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


                                var embed = new EmbedBuilder()
                                {
                                    Color = Colours.fightCol
                                };
                                embed.Description = (Context.User.Mention + " you hit and did " + randomIndex2 + " damage!\n\n" + player1 + " has " + health1 + " health left!\n" + player2 + " has " + health2 + " health left!\n" + whosTurn + " its your turn!");
                                await ReplyAsync("", false, embed.Build());

                            }
                            else
                            {

                                var embed = new EmbedBuilder()
                                {
                                    Color = Colours.fightCol
                                };
                                embed.Description = (Context.User.Mention + " you hit and did " + randomIndex2 + " damage!\n\n" + player1 + " died. " + player2 + " won!");
                                await ReplyAsync("", false, embed.Build());

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


                                var embed = new EmbedBuilder()
                                {
                                    Color = Colours.fightCol
                                };
                                embed.Description = (Context.User.Mention + " you hit and did " + randomIndex2 + " damage!\n\n" + player1 + " has " + health1 + " health left!\n" + player2 + " has " + health2 + " health left!\n" + whosTurn + " its your turn!");
                                await ReplyAsync("", false, embed.Build());
                            }
                            else
                            {

                                var embed = new EmbedBuilder()
                                {
                                    Color = Colours.fightCol
                                };
                                embed.Description = (Context.User.Mention + " you hit and did " + randomIndex2 + " damage!\n\n" + player2 + " died. " + player1 + " won!");
                                await ReplyAsync("", false, embed.Build());


                                SwitchCaseString = "nofight";
                                health1 = 100;
                                health2 = 100;
                            }
                        }
                        else
                        {
                            var embed = new EmbedBuilder()
                            {
                                Color = Colours.fightCol
                            };
                            embed.Description = ("Sorry it seems like something went wrong! Please type " + BotConfig.Load().Prefix + "giveup");
                            await ReplyAsync("", false, embed.Build());
                        }
                    }
                    else
                    {
                        placeHolder = whosTurn;
                        whosTurn = whoWaits;
                        whoWaits = placeHolder;


                        var embed = new EmbedBuilder()
                        {
                            Color = Colours.fightCol
                        };
                        embed.Description = (Context.User.Mention + ", sorry you missed!\n" + whosTurn + " your turn!");
                        await ReplyAsync("", false, embed.Build());
                    }
                }
                else
                {
                    var embed = new EmbedBuilder()
                    {
                        Color = Colours.fightCol
                    };
                    embed.Description = (Context.User.Mention + ", It Is Not Your Turn!");
                    await ReplyAsync("", false, embed.Build());
                }
            }
            else
            {
                var embed = new EmbedBuilder()
                {
                    Color = Colours.fightCol
                };
                embed.Description = (Context.User.Mention + ", There is no fight at the moment. Sorry :/");
                await ReplyAsync("", false, embed.Build());
            }
            await Context.Message.DeleteAsync();
        }

        [Command("clear")]
        [Alias("c")]
        [Remarks("Clears all messages in a channel")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task Clear()
        {
            var items = await Context.Channel.GetMessagesAsync().Flatten();
            await Context.Channel.DeleteMessagesAsync(items);
        }

        [Command("bank")]
        public async Task bankc()
        {
            var application = await Context.Client.GetApplicationInfoAsync();
            var auth = new EmbedAuthorBuilder();



            var result = Database.CheckExistingUser(Context.User);
            if (result.Count().Equals(0))
            {
                Database.Cbank(Context.User);


                var embed = new EmbedBuilder()

                {
                    Color = Colours.moneyCol,
                    Author = auth
                };


                embed.Title = $"{Context.User.Username} Has Opened A Bank Account!";
                embed.Description = $"\n:money_with_wings: **Welcome To The Bank!** :\n\n:moneybag: **Bank : 100DollaBill**\n";
                await ReplyAsync("", false, embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder()

                {
                    Color = Colours.moneyCol,
                    Author = auth
                };


                embed.Description = $":x: **{Context.User.Username} Already Has A Bank Account!**";
                await ReplyAsync("", false, embed.Build());
            }
        }

        [Command("money")]
        public async Task moneyol()
        {
            var application = await Context.Client.GetApplicationInfoAsync();
            var auth = new EmbedAuthorBuilder();

            var econ = Database.GetUserMoney(Context.User);

            var embed = new EmbedBuilder()
            {
                Color = Colours.moneyCol,
                Author = auth
            };

            embed.Title = $"{Context.User.Username}'s Balance";
            embed.Description = $"\n:money_with_wings: **Balance** :\n\n:moneybag: **{econ.FirstOrDefault().Money}**\n";
            await ReplyAsync("", false, embed.Build());
        }

        [Command("roll")]
        public async Task betcmd( int bet)
        {
            var econ = Database.GetUserMoney(Context.User);

            if (econ.FirstOrDefault().Money < bet)
            {
                var embed = new EmbedBuilder()
                {
                    Color = Colours.moneyCol
                };
                embed.Description = (Context.User.Mention + ", you do not have enough money to roll the dice!");
                await ReplyAsync("", false, embed.Build());
            }
            else
            {
                Random rand = new Random();
                Random rand2 = new Random();

                int userRoll = rand2.Next(1, 6);
                int rolled = rand.Next(1, 9);

                Console.WriteLine("User Rolled : " + userRoll);
                Console.WriteLine("Bot Rolled : " + rolled);

                if (userRoll.Equals(rolled))
                {


                    var embed = new EmbedBuilder()
                    {
                        Color = Colours.moneyCol,
                    };

                    embed.Title = $"Congrats {Context.User.Username}!";
                    embed.Description = $"You Have Made ${bet}!\n\n {Context.User.Mention} You Rolled **{userRoll}** and I rolled **{rolled}**";
                    await ReplyAsync("", false, embed.Build());

                    Database.updMoney(Context.User, bet);
                }
                else
                {

                    int betremove = -bet;

                    var embed = new EmbedBuilder()
                    {
                        Color = Colours.moneyCol,
                    };

                    embed.Title = $"Sorry **{Context.User.Username}**!";
                    embed.Description = $"You Have Lost ${bet}!\n\n {Context.User.Mention} You Rolled **{userRoll}** and I rolled **{rolled}**";
                    await ReplyAsync("", false, embed.Build());

                    Database.updMoney(Context.User, betremove);
                }
            }
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
                Color = Colours.adminCol
            };

            embed.Description = $"{dm}";
            embed.WithFooter(new EmbedFooterBuilder().WithText($"Message From: {Context.User.Username} | Guild: {Context.Guild.Name}"));
            await message.SendMessageAsync("", false, embed);
            embed.Description = $"You have sent a message to {myId} he will read the message soon.";
            embed.WithFooter(new EmbedFooterBuilder().WithText($"Message Has Been Sent Succesfully!"));

            await Context.Channel.SendMessageAsync("", false, embed);

            await Context.Message.DeleteAsync();
        }


        /** Start help

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
            await Context.Message.DeleteAsync();
        }

         End help**/

        //Start Music Bot

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



        public async Task SendLinkAsync(IGuild guild, string url)
        {

            IVoiceChannel channel = (Context.User as IVoiceState).VoiceChannel;
            IAudioClient client = await channel.ConnectAsync();

            var output = CreateStream(url).StandardOutput.BaseStream;
            var stream = client.CreatePCMStream(AudioApplication.Music, 128 * 1024);
            await output.CopyToAsync(stream);
            await stream.FlushAsync().ConfigureAwait(false);
        }




        public async Task StopAudio(IGuild guild)
        {
            IVoiceChannel channel = (Context.User as IVoiceState).VoiceChannel;
            IAudioClient client = await channel.ConnectAsync();

            await client.StopAsync();
            return;
        }



        [Command("play", RunMode = RunMode.Async)]
        public async Task play(string url)
        {
            var embed = new EmbedBuilder()
            {
                Color = Colours.musicCol
            };
            embed.Description = (Context.User.Mention + ", Has decided to listen to music!");

            await Context.Channel.SendMessageAsync("", false, embed);

            await SendLinkAsync(Context.Guild, url);

            await Context.Message.DeleteAsync();
        }

        [Command("stop", RunMode = RunMode.Async)]
        public async Task StopCmd()
        {
            await StopAudio(Context.Guild);

            await Context.Message.DeleteAsync();
        }

        // End Music Bot




        [Command("doggo")]
        [Summary("Posts random doggo pictures!")]
        public async Task Dog()
        {
            Console.WriteLine("Making API Call...");
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                string websiteurl = "https://random.dog/woof.json";
                client.BaseAddress = new Uri(websiteurl);
                HttpResponseMessage response = client.GetAsync("").Result;
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(result);


                Random randal;
                string[] doggers;
                randal = new Random();
                doggers = new string[]
                {
                        "doggers/dog1.jpeg",
                        "doggers/dog2.jpg",
                        "doggers/dog3.jpg",
                        "doggers/dog4.jpg",
                        "doggers/dog5.jpg",
                        "doggers/dog6.jpg",
                        "doggers/dog7.jpg",
                        "doggers/dog8.jpg",
                        "doggers/dog9.jpg",
                        "doggers/dog10.jpg",
                };
                string DogImage = json["url"].ToString();
                if (DogImage.Contains("mp4"))
                {
                    int RandomDoggoIndex = randal.Next(doggers.Length);
                    string DoggerToPost = doggers[RandomDoggoIndex];
                    await Context.Channel.SendFileAsync(DoggerToPost);
                }
                else
                {
                    await ReplyAsync(DogImage);
                }
            }
            await Context.Message.DeleteAsync();
        }

        [Command("cat")]
        [Summary("Posts random cat pictures!")]
        public async Task Cat()
        {
            Console.WriteLine("Making API Call...");
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                string websiteurl = "http://random.cat/meow";
                client.BaseAddress = new Uri(websiteurl);
                HttpResponseMessage response = client.GetAsync("").Result;
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(result);
                string CatImage = json["file"].ToString();
                await ReplyAsync(CatImage);
            }
            await Context.Message.DeleteAsync();
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


        [Command("accept")]
        public async Task Accept()
        {
            // Should require the new member role
            var user = Context.User;

            var config = new BotConfig();
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == config.AcceptedMemberRole);

            var remrole = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == config.NewMemberRank);

            await (user as IGuildUser).AddRoleAsync(role);
            await (user as IGuildUser).RemoveRoleAsync(remrole);

            var message = await ReplyAsync("You Have Accepted The Rules! Enjoy Being A Full Member!");
            await DelayDeleteMessage(message, 10);

            await Context.Message.DeleteAsync();
        }



    }
}