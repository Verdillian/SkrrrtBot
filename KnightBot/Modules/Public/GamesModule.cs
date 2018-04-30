using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Linq;
using KnightBot.util;
using KnightBot.Modules.Economy;
using System.IO;

namespace KnightBot.Modules.Public
{
    public class GamesModule : ModuleBase
    {
        Errors errors = new Errors();


        private int total;


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
                Color = Colors.ballCol
            };

            embed.Title = "**╋━━━━━━◥◣ Magic 8 Ball ◢◤━━━━━━╋**";
            embed.Description = Environment.NewLine + Context.User.Mention + ", " + text + Environment.NewLine + "User Question: " + input;

            await Context.Channel.SendMessageAsync("", false, embed);

            await Context.Message.DeleteAsync();


            var result = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").currentMoney;

            int bal = 10;

            total = result + bal;

            save.userID = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").userID;
            save.currentMoney = total;
            save.currentPoints = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").currentPoints;
            save.Save("bank/" + Context.User.Id.ToString() + ".json");


        }

        private BankConfig save = new BankConfig();

        [Command("roll")]
        public async Task betcmd(int bet)
        {


            if (!File.Exists(BankModule.appdir + "bank/" + Context.User.Id.ToString() + ".json"))
            {
                var newServer = File.Create(Path.Combine(BankModule.appdir, "bank/" + Context.User.Id.ToString() + ".json"));

                newServer.Close();

                save.userID = Context.User.Id.ToString();
                save.currentMoney = 100;
                save.currentPoints = 0;
                save.Save("bank/" + Context.User.Id.ToString() + ".json");


                var embed = new EmbedBuilder()

                {
                    Color = Colors.moneyCol,
                };


                embed.Title = $"{Context.User.Username} Has Opened A Bank Account!";
                embed.Description = $"\n:money_with_wings: **Welcome To The Bank!** :\n\n:moneybag: **Bank Name: ThisIsNotABank**\n";
                await ReplyAsync("", false, embed.Build());

            }

            //save the users file
            save.userID = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").userID;
            save.currentMoney = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").currentMoney;
            save.currentPoints = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").currentPoints;

            save.Save("bank/" + Context.User.Id.ToString() + ".json");



            var econ = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").currentMoney;

            if (econ < bet)
            {
                var embed = new EmbedBuilder()
                {
                    Color = Colors.moneyCol
                };
                embed.Description = (Context.User.Mention + ", you do not have enough money to roll the dice!");
                await ReplyAsync("", false, embed.Build());
            }
            else
            {
                Random rand = new Random();
                Random rand2 = new Random();

                int userRoll = rand2.Next(1, 12);
                int rolled = rand.Next(1, 15);

                Console.WriteLine("User Rolled : " + userRoll);
                Console.WriteLine("Bot Rolled : " + rolled);

                if (userRoll.Equals(rolled))
                {


                    var embed = new EmbedBuilder()
                    {
                        Color = Colors.moneyCol,
                    };

                    embed.Title = $"Congrats {Context.User.Username}!";
                    embed.Description = $"You Have Made ${bet}!\n\n {Context.User.Mention} You Rolled **{userRoll}** and I rolled **{rolled}**";
                    await ReplyAsync("", false, embed.Build());


                    save.currentMoney = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").currentMoney + bet;
                    save.currentPoints = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").currentPoints;
                    save.userID = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").userID;
                    save.Save("bank/" + Context.User.Id.ToString() + ".json");

                }
                else
                {

                    int betremove = -bet;

                    var embed = new EmbedBuilder()
                    {
                        Color = Colors.moneyCol,
                    };

                    embed.Title = $"Sorry **{Context.User.Username}**!";
                    embed.Description = $"You Have Lost ${bet}!\n\n {Context.User.Mention} You Rolled **{userRoll}** and I rolled **{rolled}**";
                    await ReplyAsync("", false, embed.Build());

                    save.currentMoney = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").currentMoney - bet;
                    save.currentPoints = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").currentPoints;
                    save.userID = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").userID;
                    save.Save("bank/" + Context.User.Id.ToString() + ".json");
                }
            }

        }
    }
}
