using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using KnightBot.Config;
using System.Linq;
using SkrrrtBot.util;
using SkrrrtBot.Nsfw;

namespace SkrrrtBot.Modules.Nsfw
{
    [Group("nsfw")]
    public class NsfwModule : ModuleBase<SocketCommandContext>
    {
        Errors errors = new Errors();

        [Command]
        public async Task joinNSFW()
        {
            var chan = Context.Channel;
            var userName = Context.User as SocketGuildUser;

            var nsfwRole = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == BotConfig.Load().NSFWRole);
            if (nsfwRole != null)
            {
                if (!userName.Roles.Contains(nsfwRole))
                {
                    await (Context.User as IGuildUser).AddRoleAsync(nsfwRole);
                    var embed = new EmbedBuilder() { Color = Colors.nsfwCol };
                    embed.Title = ("NSFW Join");
                    embed.Description = ("You have been given the nsfw role!");
                    await Context.Channel.SendMessageAsync("", false, embed);
                }
                else if (userName.Roles.Contains(nsfwRole)) await errors.sendError(chan, "You already have the nsfw role.", Colors.nsfwCol);
            }
        }
    }
}
