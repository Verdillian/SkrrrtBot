using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using KnightBot.Config;
using System.Linq;
using KnightBot.util;

namespace KnightBot.Modules.Public
{
    public class NSFW : ModuleBase<SocketCommandContext>
    {
        string placeholderGif = "https://gfycat.com/DecimalCheeryCornsnake";

        [Command("nsfw")]
        public async Task Nsfw(string type = null)
        {
            Errors errors = new Errors();
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
            else if (chan.IsNsfw)
            {
                if (type == null) await errors.sendError(chan, "The parameter entered is not used. Try the help command to see all possible parameters.", Colours.nsfwCol);

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
                    await errors.sendError(chan, "The parameter entered is not used. Try the help command to see all possible parameters.", Colours.nsfwCol);
                    await Context.Message.DeleteAsync();
                }
            }
        }
    }
}
