using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Linq;
using KnightBot.util;

namespace KnightBot.Modules.Public
{
    public class GamesModule : ModuleBase
    {
        Errors errors = new Errors();

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
            embed.Description = Environment.NewLine + Context.User.Mention + ", " + text;

            await Context.Channel.SendMessageAsync("", false, embed);

            await Context.Message.DeleteAsync();
        }

        [Command("roll")]
        public async Task betcmd(int bet)
        {
            var econ = Database.GetUserMoney(Context.User);

            if (econ.FirstOrDefault().Money < bet)
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

                int userRoll = rand2.Next(1, 6);
                int rolled = rand.Next(1, 9);

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

                    Database.updMoney(Context.User, bet);
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

                    Database.updMoney(Context.User, betremove);
                }
            }
        }
    }
}
