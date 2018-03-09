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
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;

namespace KnightBot.Modules.Public
{
    public class MusicModule : ModuleBase
    {

        // Broken Not Sure Why Not Looking Into It.


        IVoiceChannel channel;
        IAudioClient client;

        private Process CreateStream(string url)
        {
            Process currentsong = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C youtube-dl.exe -o - {url} | ffmpeg -i pipe:0 -ac 2 -f s16le -ar 48000 pipe:1",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            currentsong.Start();
            return currentsong;
        }

        [Command("play", RunMode = RunMode.Async)]
        public async Task play(string url)
        {
            IVoiceChannel channel = (Context.User as IVoiceState).VoiceChannel;
            IAudioClient client = await channel.ConnectAsync();

            var output = CreateStream(url).StandardOutput.BaseStream;
            var stream = client.CreatePCMStream(AudioApplication.Music, 128 * 1024);
            await output.CopyToAsync(stream);
            await stream.FlushAsync().ConfigureAwait(false);

            var messageToDel = await ReplyAsync(Context.User.Mention + "Has Decided To Listen To" + url);
            await Delete.DelayDeleteMessage(messageToDel, 10);
        }


        private async Task StopAudio(IGuild guild)
        {
            channel = (Context.User as IVoiceState).VoiceChannel;
            client = await channel.ConnectAsync();

            await client.StopAsync();

        }

        [Command("stop", RunMode = RunMode.Async)]
        public async Task StopCmd()
        {
            await StopAudio(Context.Guild);

            await Context.Message.DeleteAsync();
        }


    }
}
