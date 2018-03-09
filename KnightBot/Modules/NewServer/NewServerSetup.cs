using System;
using System.Threading.Tasks;
using Discord.Commands;
using System.IO;
using Newtonsoft.Json;
using Discord;

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
            }
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
            }
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

            }
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
            }
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
            }
        }

        public static readonly string appdir = AppContext.BaseDirectory;

        // attempted create thing

        [Command("create")]
        [RequireUserPermission(GuildPermission.Administrator)]
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

        }



    }
}
