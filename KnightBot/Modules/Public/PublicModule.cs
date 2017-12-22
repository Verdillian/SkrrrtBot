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

namespace KnightBot.Modules.Public
{
    public class PublicModule : ModuleBase
    {
        Errors errors = new Errors();

        private ImageSharp.Image image = null;
        private string randomString = "";
        
        [Command("setgame")]
        [Remarks("Sets the game the bot is currently playing")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Setgame()
        {
            await (Context.Client as DiscordSocketClient).SetGameAsync("KnightBot.xyz");


            await Context.Message.DeleteAsync();
        }

        [Command("clear")]
        [Alias("c")]
        [Remarks("Clears all messages in a channel")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task Clear()
        {
            var items = await Context.Channel.GetMessagesAsync().Flatten();
            await Context.Channel.DeleteMessagesAsync(items);
        }

        private static IUser ThisIsMe;
        [Command("dm")]
        [Remarks("Dm's the owner.")]
        public async Task Dmowner([Remainder] string dm)
        {
            var myId = Context.User.Mention;
            if (ThisIsMe == null)
            {
                foreach (var user in Context.Guild.GetUsersAsync().Result)
                {
                    if (user.Id == 146377960360902656)
                    {
                        ThisIsMe = user;
                        myId = user.Mention;
                        break;
                    }
                }
            }

            var application = await Context.Client.GetApplicationInfoAsync();
            var message = await application.Owner.GetOrCreateDMChannelAsync();
 
            var embed = new EmbedBuilder()
            {
                Color = Colours.adminCol
            };

            embed.Description = $"{dm}";
            embed.WithFooter(new EmbedFooterBuilder().WithText($"Message From: {Context.User.Username} | Guild: {Context.Guild.Name}"));
            await message.SendMessageAsync("", false, embed);
            embed.Description = $"You have sent a message to {myId} he will read the message soon.";
            embed.WithFooter(new EmbedFooterBuilder().WithText($"Message Has Been Sent Succesfully!"));

            await Context.Channel.SendMessageAsync("", false, embed);

            await Context.Message.DeleteAsync();
        }

        //Start Music Bot
        private Process CreateStream(string url)
        {
            Process currentsong = new Process();

            currentsong.StartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/C youtube-dl.exe -o - {url} | ffmpeg -i pipe:0 -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            currentsong.Start();
            return currentsong;
        }



        public async Task SendLinkAsync(IGuild guild, string url)
        {

            IVoiceChannel channel = (Context.User as IVoiceState).VoiceChannel;
            IAudioClient client = await channel.ConnectAsync();

            var output = CreateStream(url).StandardOutput.BaseStream;
            var stream = client.CreatePCMStream(AudioApplication.Music, 128 * 1024);
            await output.CopyToAsync(stream);
            await stream.FlushAsync().ConfigureAwait(false);
        }




        public async Task StopAudio(IGuild guild)
        {
            IVoiceChannel channel = (Context.User as IVoiceState).VoiceChannel;
            IAudioClient client = await channel.ConnectAsync();

            await client.StopAsync();
            return;
        }



        [Command("play", RunMode = RunMode.Async)]
        public async Task play(string url)
        {
            var embed = new EmbedBuilder()
            {
                Color = Colours.musicCol
            };
            embed.Description = (Context.User.Mention + ", Has decided to listen to music!");

            await Context.Channel.SendMessageAsync("", false, embed);

            await SendLinkAsync(Context.Guild, url);

            await Context.Message.DeleteAsync();
        }

        [Command("stop", RunMode = RunMode.Async)]
        public async Task StopCmd()
        {
            await StopAudio(Context.Guild);

            await Context.Message.DeleteAsync();
        }
        // End Music Bot




        [Command("doggo")]
        [Summary("Posts random doggo pictures!")]
        public async Task Dog()
        {
            Console.WriteLine("Making API Call...");
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                string websiteurl = "https://random.dog/woof.json";
                client.BaseAddress = new Uri(websiteurl);
                HttpResponseMessage response = client.GetAsync("").Result;
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(result);


                Random randal;
                string[] doggers;
                randal = new Random();
                doggers = new string[]
                {
                        "doggers/dog1.jpeg",
                        "doggers/dog2.jpg",
                        "doggers/dog3.jpg",
                        "doggers/dog4.jpg",
                        "doggers/dog5.jpg",
                        "doggers/dog6.jpg",
                        "doggers/dog7.jpg",
                        "doggers/dog8.jpg",
                        "doggers/dog9.jpg",
                        "doggers/dog10.jpg",
                };
                string DogImage = json["url"].ToString();
                if (DogImage.Contains("mp4"))
                {
                    int RandomDoggoIndex = randal.Next(doggers.Length);
                    string DoggerToPost = doggers[RandomDoggoIndex];
                    await Context.Channel.SendFileAsync(DoggerToPost);
                }
                else
                {
                    await ReplyAsync(DogImage);
                }
            }
            await Context.Message.DeleteAsync();
        }

        [Command("cat")]
        [Summary("Posts random cat pictures!")]
        public async Task Cat()
        {
            Console.WriteLine("Making API Call...");
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                string websiteurl = "http://random.cat/meow";
                client.BaseAddress = new Uri(websiteurl);
                HttpResponseMessage response = client.GetAsync("").Result;
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(result);
                string CatImage = json["file"].ToString();
                await ReplyAsync(CatImage);
            }
            await Context.Message.DeleteAsync();
        }

        [Command("accept")]
        public async Task Accept()
        {
            var chan = Context.Channel;
            var userName = Context.User as SocketGuildUser;
            var newMemberRole = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == BotConfig.Load().NewMemberRank);

            if (newMemberRole != null)
            {
                if (userName.Roles.Contains(newMemberRole))
                {
                    var user = Context.User;

                    var config = new BotConfig();
                    var role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == BotConfig.Load().AcceptedMemberRole);

                    var remrole = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == BotConfig.Load().NewMemberRank);

                    await (user as IGuildUser).AddRoleAsync(role);
                    await (user as IGuildUser).RemoveRoleAsync(remrole);

                    var message = await ReplyAsync("You Have Accepted The Rules! Enjoy Being A Full Member!");
                    await Delete.DelayDeleteMessage(message, 10);

                    await Context.Message.DeleteAsync();
                }
            }
            else await errors.sendError(chan, "The new members role is not set up correctly in the config!", Colours.generalCol);
        }
    }
}