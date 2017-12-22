using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Discord.Audio;
using System.Diagnostics;
using System.Linq;
using KnightBot.Config;
using KnightBot.util;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Linq;

namespace KnightBot.Modules.Public
{
    public class PublicModule : ModuleBase
    {
        private ImageSharp.Image image = null;
        private string randomString = "";
        
        [Command("setgame")]
        [Remarks("Sets the game the bot is currently playing")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Setgame()
        {
            await (Context.Client as DiscordSocketClient).SetGameAsync("KnightBot.xyz");


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
        public async Task Bank(string type = null, IGuildUser user = null, int amt = 0)
        {
            Errors errors = new Errors();
            var chan = Context.Channel;
            if (type.Equals("open"))
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
                else await errors.sendError(chan, "User already has a bank account", Colours.moneyCol);
            }
            else if (type.Equals("balance"))
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
            else if (type.Equals("transfer"))
            {
                var balance = Database.GetUserMoney(Context.User).FirstOrDefault().Money;
                if (amt <= balance)
                {
                    if (user != null)
                    {
                        if (amt > 0 && amt < int.MaxValue)
                        {
                            Database.updMoney(Context.User, -amt);
                            Database.updMoney(user, amt);

                            balance = Database.GetUserMoney(Context.User).FirstOrDefault().Money;
                            var balance1 = Database.GetUserMoney(user).FirstOrDefault().Money;
                            var embed = new EmbedBuilder() { Color = Colours.moneyCol };
                            var fromField = new EmbedFieldBuilder() { Name = Context.User.Username + "'s new balance:", Value = "$" + balance };
                            var toField = new EmbedFieldBuilder() { Name = user.Username + "'s new balance:", Value = "$" + balance1 };

                            embed.Title = ("Bank Transfer");
                            embed.Description = ("Successfully transferred money!");
                            embed.AddField(fromField);
                            embed.AddField(toField);

                            await Context.Channel.SendMessageAsync("", false, embed);
                        }
                        else
                        {
                            await errors.sendError(chan, "You need to enter an amount higher than 0 but lower than " + int.MaxValue + "!", Colours.moneyCol);
                        }
                    }
                    else
                    {
                        await errors.sendError(chan, "Erm, who are you trying to transfer money to?", Colours.moneyCol);
                    }
                }
                else
                {
                    await errors.sendError(chan, "You do not have enough money to do this.", Colours.moneyCol);
                }
            }
            else
            {
                await errors.sendError(chan, "This bank command doesn't exist.", Colours.moneyCol);
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
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == BotConfig.Load().AcceptedMemberRole);

            var remrole = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == BotConfig.Load().NewMemberRank);

            await (user as IGuildUser).AddRoleAsync(role);
            await (user as IGuildUser).RemoveRoleAsync(remrole);

            var message = await ReplyAsync("You Have Accepted The Rules! Enjoy Being A Full Member!");
            await DelayDeleteMessage(message, 10);

            await Context.Message.DeleteAsync();
        }
    }
}