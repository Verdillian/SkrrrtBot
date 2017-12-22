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
        Errors errors = new Errors();
        string placeholderGif = "https://gfycat.com/DecimalCheeryCornsnake";

        [Command("nsfw")]
        public async Task Nsfw(string type = null)
        {
            var chan = Context.Channel;
            var userName = Context.User as SocketGuildUser;

            if (type.Equals("join"))
            {
                var nsfwRole = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == BotConfig.Load().NSFWRole);
                if (nsfwRole != null)
                {
                    if (!userName.Roles.Contains(nsfwRole))
                    {
                        await (Context.User as IGuildUser).AddRoleAsync(nsfwRole);
                        var embed = new EmbedBuilder() { Color = Colours.nsfwCol };
                        embed.Title = ("NSFW Join");
                        embed.Description = ("You have been given the nsfw role!");
                        await Context.Channel.SendMessageAsync("", false, embed);
                    }
                    else if (userName.Roles.Contains(nsfwRole)) await errors.sendError(chan, "You already have the nsfw role.", Colours.nsfwCol);
                }
                else await errors.sendError(chan, "The nsfw role does not exist.", Colours.nsfwCol);
            }
            if (type.Equals("leave"))
            {
                var nsfwRole = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == BotConfig.Load().NSFWRole);
                if (nsfwRole != null)
                {
                    if (userName.Roles.Contains(nsfwRole))
                    {
                        await (Context.User as IGuildUser).RemoveRoleAsync(nsfwRole);
                        var embed = new EmbedBuilder() { Color = Colours.nsfwCol };
                        embed.Title = ("NSFW Leave");
                        embed.Description = ("You have been removed from the nsfw role!");
                        await Context.Channel.SendMessageAsync("", false, embed);
                    }
                    else if (!userName.Roles.Contains(nsfwRole)) await errors.sendError(chan, "You do not have the nsfw role already.", Colours.nsfwCol);
                }
                else await errors.sendError(chan, "The nsfw role does not exist.", Colours.nsfwCol);
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
                        await Context.Guild.CreateRoleAsync(BotConfig.Load().NSFWRole.ToString(), null, Color.Red, false, null);
                        var embed = new EmbedBuilder() { Color = Colours.nsfwCol };
                        embed.Title = ("NSFW Create");
                        embed.Description = ("You have created the nsfw role!");
                        await Context.Channel.SendMessageAsync("", false, embed);
                    }
                    else await errors.sendError(chan, "Only Blurr can do this command.", Colours.nsfwCol);
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
