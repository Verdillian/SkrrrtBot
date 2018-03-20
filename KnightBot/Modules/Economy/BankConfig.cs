using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KnightBot.Modules.Economy
{
    class BankConfig
    {

        [JsonIgnore]
        public static readonly string appdir = AppContext.BaseDirectory;

        public string userID { get; set; }
        public int currentMoney { get; set; }
        public int currentPoints { get; set; }

        public BankConfig()
        {
            userID = "";
            currentMoney = 100;
            currentPoints = 0;
        }

        public void Save(string path)
        {
            string file = Path.Combine(appdir, path);
            File.WriteAllText(file, ToJson());
        }
        public static BankConfig Load(string path)
        {
            string file = Path.Combine(appdir, path);
            return JsonConvert.DeserializeObject<BankConfig>(File.ReadAllText(file));
        }
        public string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);

    }
}
