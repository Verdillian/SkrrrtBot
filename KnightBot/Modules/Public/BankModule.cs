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
    public class BankModule : ModuleBase
    {
        Errors errors = new Errors();

        [Command("bank")]
        public async Task Bank(string type = null, IGuildUser user = null, int amt = 0)
        {
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
                        Color = Colors.moneyCol,
                        Author = auth
                    };


                    embed.Title = $"{Context.User.Username} Has Opened A Bank Account!";
                    embed.Description = $"\n:money_with_wings: **Welcome To The Bank!** :\n\n:moneybag: **Bank : 100DollaBill**\n";
                    await ReplyAsync("", false, embed.Build());
                }
                else await errors.sendError(chan, "User already has a bank account", Colors.moneyCol);
            }
            else if (type.Equals("balance"))
            {
                var application = await Context.Client.GetApplicationInfoAsync();
                var auth = new EmbedAuthorBuilder();

                var econ = Database.GetUserMoney(Context.User);

                var embed = new EmbedBuilder()
                {
                    Color = Colors.moneyCol,
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
                            var embed = new EmbedBuilder() { Color = Colors.moneyCol };
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
                            await errors.sendError(chan, "You need to enter an amount higher than 0 but lower than " + int.MaxValue + "!", Colors.moneyCol);
                        }
                    }
                    else
                    {
                        await errors.sendError(chan, "Erm, who are you trying to transfer money to?", Colors.moneyCol);
                    }
                }
                else
                {
                    await errors.sendError(chan, "You do not have enough money to do this.", Colors.moneyCol);
                }
            }
            else
            {
                await errors.sendError(chan, "This bank command doesn't exist.", Colors.moneyCol);
            }
        }
    }
}
