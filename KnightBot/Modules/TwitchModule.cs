using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib;
using TwitchLib.Client.Models;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client;
using TwitchLib.Api;
using TwitchLib.Api.Models.v5.Users;
using System.IO;
using TwitchLib.Client.Events.Services.MessageThrottler;
using TwitchLib.Client.Interfaces;
using Discord;
using System.Threading;
using Discord.Commands;
using KnightBot.Config;

namespace KnightBot.Modules
{
    public class TwitchModule : ModuleBase
    {

        readonly ConnectionCredentials credentials = new ConnectionCredentials(TwitchInfo.BotUsername, TwitchInfo.BotToken);
        TwitchClient client;

        //private static TwitchAPI api;

        TwitchAPI api = new TwitchAPI();


        public void Connect()
        {
            Console.WriteLine("Connecting...");

            //client = new TwitchClient(credentials, TwitchInfo.ChannelName, logging: false);

            client = new TwitchClient();
            client.Initialize(credentials, TwitchInfo.ChannelName);

            //client.OnLog += Client_OnLog;
            //client.OnConnectionError += Client_OnConnectionError;
            //client.OnMessageReceived += Client_MessageReceived;
            //client.OnUserTimedout += Client_OnUserTimeOut;
            //client.OnChatCommandReceived += Client_OnCommandReceived;
            //client.OnModeratorJoined += ModJoined;
            //client.OnModeratorLeft += ModLeft;

            client.Connect();

            //api.Settings.ClientId.Equals(TwitchInfo.ClientId);

            api.Settings.SetClientIdAsync(TwitchInfo.ClientId);

        }
        /*
        public async Task CheckLiveStream()
        {
            BotConfig config = new BotConfig();

            for (int i = 0; i < BotConfig.Load().streamers.Length; i++)
            {
                User userList = api.Users.v5.GetUserByNameAsync(BotConfig.Load().streamers.FirstOrDefault()).Result.Matches;
                if (userList == null || userList.Length == 0)
                {
                    return;
                }

                return userList[0].Id;

            }
        }
        */
        TimeSpan? GetUptime()
        {
            string userId = GetUserId(TwitchInfo.ChannelName);
            if (userId == null)
            {
                return null;
            }

            return api.Streams.v5.GetUptimeAsync(userId).Result;
        }

        string GetUserId(string username)
        {
            //User[] userList = api.Users.v5.GetUserByNameAsync(username).Result.Matches;
            User[] userList = api.Users.v5.GetUserByNameAsync(username).Result.Matches;
            if (userList == null || userList.Length == 0)
            {
                return null;
            }

            return userList[0].Id;
        }

    }
}
