using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using KnightBot.Config;

namespace KnightBot.Modules.Public
{
    public class NSFW : ModuleBase<SocketCommandContext>
    {
        string placeholderGif = "https://gfycat.com/DecimalCheeryCornsnake";

        [Command("nsfw")]
        public async Task Nsfw(string type = null)
        {
            var chan = Context.Channel;

            if (chan.IsNsfw)
            {
            if (type == null) throw new ArgumentException("Please use the command as follows ( " + "nsfw <butt/boobs> )");

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
                else
                {
                    var embed = new EmbedBuilder()
                    {
                        Color = new Color(0, 175, 240)
                    };
                    embed.Description = (Context.User.Mention + ", I do not recognise that parameter. Try Butt or Boobs!");
                    await ReplyAsync("", false, embed.Build());

                    await Context.Message.DeleteAsync();
                }
            }
        }
    }
}
