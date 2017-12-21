using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using KnightBot.Config;
using System.Linq;

namespace KnightBot.Modules.Public
{
    public class NSFW : ModuleBase<SocketCommandContext>
    {
        string placeholderGif = "https://gfycat.com/DecimalCheeryCornsnake";

        [Command("nsfw")]
        public async Task Nsfw(string type = null)
        {
            var chan = Context.Channel;
            var config = new BotConfig();
            var userName = Context.User as SocketGuildUser;

            if (type.Equals("join"))
            {
                var nsfwRole = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == config.NSFWRole);
                if (nsfwRole != null)
                {
                    if (!userName.Roles.Contains(nsfwRole)) await (Context.User as IGuildUser).AddRoleAsync(nsfwRole);
                    else if (userName.Roles.Contains(nsfwRole)) throw new ArgumentException("You already have the nsfw role.");
                }
                else throw new ArgumentException("The NSFW Role doesn't exist.");
            }
            else if (type.Equals("help") || type.Equals("commands"))
            {
                var embed = new EmbedBuilder()
                {
                    Color = new Color(0, 175, 240)
                };
                embed.Title = $"NSFW Help";
                embed.Description = config.Prefix.ToString() + "nsfw join  - Join the NSFW Role to view NSFW channels.\n" +
                                    config.Prefix.ToString() + "nsfw butt  - Displays a random butt pic.\n" +
                                    config.Prefix.ToString() + "nsfw boobs - Displays a random boobs pic.\n" +
                                    config.Prefix.ToString() + "nsfw gif   - Displays a random sexy gif.\n";
                await Context.Channel.SendMessageAsync("", false, embed);
            }
            else if (chan.IsNsfw)
            {
                if (type == null) throw new ArgumentException("Need to display help.");

                type = type.ToLower();

                if (type.Equals("butt"))
                {
                    //Display a random butt pic
                    await Context.Channel.SendMessageAsync("**No butt pics can be found :(** \n Enjoy this to pass time: " + placeholderGif);
                }
                else if (type.Equals("boobs"))
                {
                    //Display a random boobs pic
                    await Context.Channel.SendMessageAsync("**No boob pics can be found :(** \n Enjoy this to pass time: " + placeholderGif);
                }
                else if (type.Equals("create"))
                {
                    if (userName.Id == 211938243535568896)
                    {
                        await Context.Guild.CreateRoleAsync(config.NSFWRole.ToString(), null, Color.Red, false, null);
                        throw new ArgumentException("Role created with name " + config.NSFWRole.ToString() + ".");
                    }
                    else throw new ArgumentException("Only Blurr can do this.");
                }
                else
                {
                    var embed = new EmbedBuilder()
                    {
                        Color = new Color(0, 175, 240)
                    };
                    embed.Description = (Context.User.Mention + ", try " + config.Prefix.ToString() + "nsfw help");
                    await ReplyAsync("", false, embed.Build());

                    await Context.Message.DeleteAsync();
                }
            }
        }
    }
}
