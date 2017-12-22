using System;
using Discord;
using Discord.Commands;
using System.Linq;
using KnightBot.Config;
using KnightBot.util;
using System.Threading.Tasks;

namespace KnightBot.Modules.Public
{
    public class FightModule : ModuleBase<SocketCommandContext>
    {
        Errors errors = new Errors();

        static string player1;
        static string player2;
        static string whosTurn;
        static string whoWaits;
        static string placeHolder;
        static int maxHealth = 100;
        static int minDamage = 2, maxDamage = 30;
        static int health1 = maxHealth;
        static int health2 = maxHealth;
        static string SwitchCaseString = "nofight";

        [Command("fight")]
        [Remarks("Starts a fight with the @mention user (example: !fight @Knight#1234")]
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
                health1 = maxHealth;
                health2 = maxHealth;
            }
            else
            {
                var embed = new EmbedBuilder()
                {
                    Color = Colours.fightCol
                };
                embed.Description = (Context.User.Mention + ", There is no fight to stop.");
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

                        int randomIndex2 = rand2.Next(minDamage, maxDamage);

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
                                Database.updPoints(Context.User, 3);
                                await ReplyAsync("", false, embed.Build());

                                SwitchCaseString = "nofight;";
                                health1 = maxHealth;
                                health2 = maxHealth;
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
                                Database.updPoints(Context.User, 3);
                                await ReplyAsync("", false, embed.Build());


                                SwitchCaseString = "nofight";
                                health1 = maxHealth;
                                health2 = maxHealth;
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

        [Command("points")]
        public async Task Points()
        {
            var points = Database.GetUserPoints(Context.User).FirstOrDefault().Points;
            if (points >= 0)
            {
                var embed = new EmbedBuilder() { Color = Colours.fightCol };

                embed.Title = $"{Context.User.Username}'s Points";
                embed.Description = $"You have " + points + " points from your fights!";
                await ReplyAsync("", false, embed.Build());
            }
            else await errors.sendError(Context.Channel, "You appear to have negative points, contact a developer.", Colours.fightCol);
        }
    }
}
