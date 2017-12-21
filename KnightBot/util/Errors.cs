
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KnightBot.util
{
    class Errors : ModuleBase<SocketCommandContext>
    {
        public async Task sendError(ISocketMessageChannel channel, string error, Color color)
        {
            Console.WriteLine("ERROR: " + error);

            var embed = new EmbedBuilder() { Color = color };
            embed.Title = ("ERROR");
            embed.Description = (error);
            await channel.SendMessageAsync("", false, embed);

            Console.WriteLine("Error message was sent to the user.");

        }
    }
}
