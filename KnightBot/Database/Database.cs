using Discord;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnightBot
{
    public class Database
    {
        private string Table { get; set; }
        private const string server = "ip";
        private const string database = "db";
        private const string username = "username";
        private const string password = "password";
        private MySqlConnection dbConnection;

        public Database(string table)
        {
            this.Table = table;
            MySqlConnectionStringBuilder stringBuilder = new MySqlConnectionStringBuilder();
            stringBuilder.Server = server;
            stringBuilder.UserID = username;
            stringBuilder.Password = password;
            stringBuilder.Database = database;
            stringBuilder.SslMode = MySqlSslMode.None;

            var connectionString = stringBuilder.ToString();
            dbConnection = new MySqlConnection(connectionString);
            dbConnection.Open();
        }

        public MySqlDataReader FireCommand(string query)
        {
            if (dbConnection == null)
            {
                return null;
            }

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            var mySqlReader = command.ExecuteReader();
            return mySqlReader;
        }

        public static List<String> CheckExistingUser(IUser user)
        {
            var result = new List<String>();
            var database = new Database("db");
            var str = string.Format("SELECT * FROM " + "econ" + " WHERE user_id = '{0}'", user.Id);
            var tableName = database.FireCommand(str);

            while (tableName.Read())
            {
                var userId = (string)tableName["user_id"];

                result.Add(userId);
            }
            return result;
        }

        public static string EnterUser(IUser user)
        {
            var database = new Database("db");
            var str = string.Format("INSERT INTO tokens (user_id, username, tokens) VALUES ('{0}', '{1}', '100')", user.Id, user.Username);
            var table = database.FireCommand(str);
            database.CloseConnection();
            return null;
        }

        public static List<tableName> GetUserStatus(IUser user)
        {
            var result = new List<tableName>();

            var database = new Database("db");

            var str = string.Format("SELECT * FROM tokens WHERE user_id = '{0}'", user.Id);
            var tableName = database.FireCommand(str);

            while (tableName.Read())
            {
                var userId = (string)tableName["user_id"];
                var userName = (string)tableName["username"];
                var currentTokens = (int)tableName["tokens"];

                result.Add(new tableName
                {
                    UserId = userId,
                    Username = userName,
                    Tokens = currentTokens
                });
            }
            database.CloseConnection();

            return result;
        }

        public static void ChangeTokens(IUser user, int tokens)
        {
            var database = new Database("db");

            try
            {
                var strings = string.Format("UPDATE tokens SET tokens = tokens + '{1}' WHERE user_id = {0}", user.Id, tokens);
                var reader = database.FireCommand(strings);
                reader.Close();
                database.CloseConnection();
                return;
            }
            catch (Exception)
            {
                database.CloseConnection();
                return;
            }
        }

        public static string Cbank(IUser user)
        {
            var database = new Database("db");

            var str = string.Format("INSERT INTO econ(user_id, username, money ) VALUES ('{0}', '{1}' ,'100')", user.Id, user.Username);
            var table = database.FireCommand(str);

            database.CloseConnection();

            return null;
        }

        public static List<Econ> GetUserMoney(IUser user)
        {
            var result = new List<Econ>();
            var database = new Database("db");

            var str = string.Format("SELECT * FROM econ WHERE user_id = '{0}'", user.Id);
            var econ = database.FireCommand(str);

            while (econ.Read())
            {
                var userId = (string)econ["user_id"];
                var money = (int)econ["money"];

                result.Add(new Econ
                {
                    UserId = userId,
                    Money = money
                });
            }
            database.CloseConnection();
            return result;

        }

        public static void updMoney(IUser user, int money)
        {
            var database = new Database("db");

            try
            {
                var strings = string.Format("UPDATE econ SET money = money + '{1}' WHERE user_id = {0}", user.Id, money);
                var reader = database.FireCommand(strings);
                reader.Close();
                database.CloseConnection();
                return;
            }
            catch (Exception e)
            {
                database.CloseConnection();
                return;
            }
        }

        



        public void CloseConnection()
        {
            if (dbConnection.Equals(null))
            {
                dbConnection.Close();
            }
        }


    }
}
