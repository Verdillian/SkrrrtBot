using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System.Linq;
using KnightBot.util;
using KnightBot.Modules.Economy;

namespace KnightBot.Modules.Public
{
    public class AuctionModule : ModuleBase
    {
        Errors errors = new Errors();


        private int total;

        private BankConfig save = new BankConfig();


        //Doesnt actually do anything? Legit just announces shit.

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
                    await Delete.DelayDeleteMessage(Context.Message, 10);
                    await Delete.DelayDeleteMessage(messageToDel, 10);
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


            var result = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").currentMoney;

            int bal = 10;

            total = result + bal;

            save.userID = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").userID;
            save.currentMoney = total;
            save.currentPoints = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").currentPoints;
            save.Save("bank/" + Context.User.Id.ToString() + ".json");
            await Context.Message.DeleteAsync();

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


            var result = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").currentMoney;

            int bal = 10;

            total = result + bal;

            save.userID = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").userID;
            save.currentMoney = total;
            save.currentPoints = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").currentPoints;
            save.Save("bank/" + Context.User.Id.ToString() + ".json");
            await Context.Message.DeleteAsync();

        }

        [Command("auctioncheck")]
        public async Task Auctioncheck()
        {
            var DM = await Context.User.GetOrCreateDMChannelAsync();
            if (auctionCheck.Equals("over") || auctionCheck.Equals(""))
            {
                var message = await DM.SendMessageAsync("Sorry there isn't an auction at this time.");
                await Delete.DelayDeleteMessage(Context.Message, 10);
                await Delete.DelayDeleteMessage(message, 10);
                return;
            }
            var auctionStatus = Context.Channel.GetMessageAsync(currentAuction);

            var embed = auctionStatus.Result.Embeds.FirstOrDefault() as Embed;
            await DM.SendMessageAsync("", false, embed);
            await Delete.DelayDeleteMessage(Context.Message, 10);


            var result = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").currentMoney;

            int bal = 10;

            total = result + bal;

            save.userID = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").userID;
            save.currentMoney = total;
            save.currentPoints = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").currentPoints;
            save.Save("bank/" + Context.User.Id.ToString() + ".json");
            await Context.Message.DeleteAsync();

        }

        [Command("bid")]
        public async Task Bid(string amount)
        {
            int bid;
            if (auctionCheck.Equals("live"))
            {
                var message = await ReplyAsync("There's no auction at this time, ask someone to start one.");
                await Delete.DelayDeleteMessage(Context.Message, 10);
                await Delete.DelayDeleteMessage(message, 10);
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
                    await Delete.DelayDeleteMessage(Context.Message, 10);
                    await Delete.DelayDeleteMessage(message, 10);
                    return;
                }
                if (bid <= hightBid)
                {
                    var message = await ReplyAsync("Your bid is too low, increase it and try again.");
                    await Delete.DelayDeleteMessage(Context.Message, 10);
                    await Delete.DelayDeleteMessage(message, 10);
                    return;
                }

                hightBid = bid;
                highBidder = Context.User as SocketGuildUser;

                await UpDateHighBidder(Context.Message as SocketUserMessage, bid);

                var message2 = await ReplyAsync($"The current highest bidder is {highBidder.Mention} with a bid of {hightBid} :moneybag: ");

            }


            var result = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").currentMoney;

            int bal = 10;

            total = result + bal;

            save.userID = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").userID;
            save.currentMoney = total;
            save.currentPoints = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").currentPoints;
            save.Save("bank/" + Context.User.Id.ToString() + ".json");
            await Context.Message.DeleteAsync();

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


            var result = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").currentMoney;

            int bal = 10;

            total = result + bal;

            save.userID = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").userID;
            save.currentMoney = total;
            save.currentPoints = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").currentPoints;
            save.Save("bank/" + Context.User.Id.ToString() + ".json");

            await Context.Message.DeleteAsync();
        }
    }
}
