using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;



namespace KnightBot.Modules.NewServer
{
    class ServerConfig
    {
        [JsonIgnore]
        public static readonly string appdir = AppContext.BaseDirectory;

        public string serverID { get; set; }
        public string serverPrefix { get; set; }
        public string newMemRole { get; set; }
        public string accMemRole { get; set; }
        public string monRole { get; set; }
        public string NSFWRole { get; set; }

        public ServerConfig()
        {
            serverID = "";
            serverPrefix = "";
            newMemRole = "";
            accMemRole = "";
            monRole = "";
            NSFWRole = "";
        }

        public void Save(string path)
        {
            string file = Path.Combine(appdir, path);
            File.WriteAllText(file, ToJson());
        }
        public static ServerConfig Load(string path)
        {
            string file = Path.Combine(appdir, path);
            return JsonConvert.DeserializeObject<ServerConfig>(File.ReadAllText(file));
        }
        public string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);


    }
}
