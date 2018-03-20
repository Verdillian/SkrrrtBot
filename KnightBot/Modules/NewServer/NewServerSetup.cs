using System;
using System.Threading.Tasks;
using Discord.Commands;
using System.IO;
using Newtonsoft.Json;
using Discord;
using KnightBot.util;

namespace KnightBot.Modules.NewServer
{
    [Group("setup")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class NewServerSetup : ModuleBase
    {

        private ServerConfig save = new ServerConfig();

        [Command("prefix")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task setPrefix([Remainder]string prefix)
        {
            if (prefix != null)
            {
                save.serverID = Context.Guild.Id.ToString();
                save.newMemRole = ServerConfig.Load("servers/" + Context.Guild.Id.ToString() + ".json").newMemRole;
                save.accMemRole = ServerConfig.Load("servers/" + Context.Guild.Id.ToString() + ".json").accMemRole;
                save.monRole = ServerConfig.Load("servers/" + Context.Guild.Id.ToString() + ".json").monRole;
                save.NSFWRole = ServerConfig.Load("servers/" + Context.Guild.Id.ToString() + ".json").NSFWRole;
                save.serverPrefix = prefix;
                save.Save("servers/" + save.serverID + ".json");


                var message = await ReplyAsync("Prefix Has Been Set! Continue With Setup by Doing --newmemrole (role you want)");
                await Delete.DelayDeleteMessage(message, 10);

            }

            await Context.Message.DeleteAsync();

        }

        [Command("newmemrole")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task setNewmemrole([Remainder]string newmem)
        {
            if (newmem != null)
            {
                save.serverID = ServerConfig.Load("servers/" + Context.Guild.Id.ToString() + ".json").serverID;
                save.serverPrefix = ServerConfig.Load("servers/" + Context.Guild.Id.ToString() + ".json").serverPrefix;
                save.newMemRole = newmem;
                save.accMemRole = ServerConfig.Load("servers/" + Context.Guild.Id.ToString() + ".json").accMemRole;
                save.monRole = ServerConfig.Load("servers/" + Context.Guild.Id.ToString() + ".json").monRole;
                save.NSFWRole = ServerConfig.Load("servers/" + Context.Guild.Id.ToString() + ".json").NSFWRole;
                
                save.Save("servers/" + Context.Guild.Id.ToString() + ".json");

                var message = await ReplyAsync("New Member Role Has Been Set! Continue With Setup by Setting the Accepted Member Role --accmemrole (role you want)");
                await Delete.DelayDeleteMessage(message, 10);


            }

            await Context.Message.DeleteAsync();


        }

        [Command("accmemrole")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task setAccmemrole([Remainder]string accmem)
        {
            if (accmem != null)
            {

                save.serverID = ServerConfig.Load("servers/" + Context.Guild.Id.ToString() + ".json").serverID;
                save.serverPrefix = ServerConfig.Load("servers/" + Context.Guild.Id.ToString() + ".json").serverPrefix;
                save.newMemRole = ServerConfig.Load("servers/" + Context.Guild.Id.ToString() + ".json").newMemRole;
                save.accMemRole = accmem;
                save.monRole = ServerConfig.Load("servers/" + Context.Guild.Id.ToString() + ".json").monRole;
                save.NSFWRole = ServerConfig.Load("servers/" + Context.Guild.Id.ToString() + ".json").NSFWRole;

                save.Save("servers/" + Context.Guild.Id.ToString() + ".json");


                var message = await ReplyAsync("Accepted Member Role Has Been Set! Continue With Setup by Setting the Money Role (admin role that can give money to people) --monrole (role you want)");
                await Delete.DelayDeleteMessage(message, 10);


            }

            await Context.Message.DeleteAsync();

        }

        [Command("monrole")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task setMonrole([Remainder]string monrole)
        {
            if (monrole != null)
            {
                save.serverID = ServerConfig.Load("servers/" + Context.Guild.Id.ToString() + ".json").serverID;
                save.serverPrefix = ServerConfig.Load("servers/" + Context.Guild.Id.ToString() + ".json").serverPrefix;
                save.newMemRole = ServerConfig.Load("servers/" + Context.Guild.Id.ToString() + ".json").newMemRole;
                save.accMemRole = ServerConfig.Load("servers/" + Context.Guild.Id.ToString() + ".json").accMemRole;
                save.monRole = monrole;
                save.NSFWRole = ServerConfig.Load("servers/" + Context.Guild.Id.ToString() + ".json").NSFWRole;

                save.Save("servers/" + Context.Guild.Id.ToString() + ".json");


                var message = await ReplyAsync("Money Role Has Been Set! Continue With Setup by Setting the NSFW Role --nsfwrole (role you want)");
                await Delete.DelayDeleteMessage(message, 10);



            }

            await Context.Message.DeleteAsync();

        }

        [Command("nsfwrole")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task setNsfwrole([Remainder]string nsfwrole)
        {
            if (nsfwrole != null)
            {
                save.serverID = ServerConfig.Load("servers/" + Context.Guild.Id.ToString() + ".json").serverID;
                save.serverPrefix = ServerConfig.Load("servers/" + Context.Guild.Id.ToString() + ".json").serverPrefix;
                save.newMemRole = ServerConfig.Load("servers/" + Context.Guild.Id.ToString() + ".json").newMemRole;
                save.accMemRole = ServerConfig.Load("servers/" + Context.Guild.Id.ToString() + ".json").accMemRole;
                save.monRole = ServerConfig.Load("servers/" + Context.Guild.Id.ToString() + ".json").monRole;
                save.NSFWRole = nsfwrole;

                save.Save("servers/" + Context.Guild.Id.ToString() + ".json");

                var message = await ReplyAsync("NSFW Role Has Been Set! You Have Completed Setup And Can Now Use The Prefix You Have Defined Previously. If You Somehow Forget The Prefix Feel Free To Use --dm and DM the creator of the bot!");
                await Delete.DelayDeleteMessage(message, 10);

            }

            await Context.Message.DeleteAsync();

        }

        public static readonly string appdir = AppContext.BaseDirectory;

        // attempted create thing -- WORKS!!!!!!

        [Command("create")]
        public async Task createServer()
        {
            if (!File.Exists(appdir + "servers/" + Context.Guild.Id.ToString() + ".json"))
            {
                var newServer = File.Create(Path.Combine(appdir, "servers/" + Context.Guild.Id.ToString() + ".json"));

                newServer.Close();

                save.serverID = "";
                save.serverPrefix = "";
                save.newMemRole = "";
                save.accMemRole = "";
                save.monRole = "";
                save.NSFWRole = "";
                save.Save("servers/" + Context.Guild.Id.ToString() + ".json");

            }

            save.serverID = ServerConfig.Load("servers/" + Context.Guild.Id.ToString() + ".json").serverID;
            save.serverPrefix = ServerConfig.Load("servers/" + Context.Guild.Id.ToString() + ".json").serverPrefix;
            save.newMemRole = ServerConfig.Load("servers/" + Context.Guild.Id.ToString() + ".json").newMemRole;
            save.accMemRole = ServerConfig.Load("servers/" + Context.Guild.Id.ToString() + ".json").accMemRole;
            save.monRole = ServerConfig.Load("servers/" + Context.Guild.Id.ToString() + ".json").monRole;
            save.NSFWRole = ServerConfig.Load("servers/" + Context.Guild.Id.ToString() + ".json").NSFWRole;

            save.Save("servers/" + Context.Guild.Id.ToString() + ".json");

            var message = await ReplyAsync("File Has Been Created! Continue With Setup by Doing --prefix (prefix you want)");
            await Delete.DelayDeleteMessage(message, 10);

            await Context.Message.DeleteAsync();

        }



    }
}
