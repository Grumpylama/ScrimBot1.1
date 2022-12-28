using DSharpPlus.Interactivity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;


namespace big
{
    
    public class Dependecies
    {
        public DiscordClient Client;


        public MatchMakerHandler MatchMakerHandler = new MatchMakerHandler();
        public List<Team> Teams = new List<Team>();
        public List<Game> Games = new List<Game>();
        public List<DiscordUser> Users = new List<DiscordUser>();

        public List<SaveableUser> UserIDs = new List<SaveableUser>();

        public Dictionary<DiscordUser, DiscordChannel> DMChannel = new Dictionary<DiscordUser, DiscordChannel>();
        
        
        
        
        public DiscordUser GetUserFromID(ulong id)
        {
            foreach (var user in Users)
            {
                if (user.Id == id)
                {
                    return user;
                }
            }

            return null;
        }


        public Team GetTeamFromID(int id)
        {
            foreach (var team in Teams)
            {
                if (team.teamID == id)
                {
                    return team;
                }
            }

            return null;
        }

        public Game GetGameFromID(int id)
        {
            foreach (var game in Games)
            {
                if (game.GameID == id)
                {
                    return game;
                }
            }

            return null;
        }
        
        

        public async Task<DiscordChannel> GetDMChannel(DiscordUser user)
        {
            if (DMChannel.ContainsKey(user))
            {
                return DMChannel[user];
            }
            else
            {
                return null;
            }
        }

        public async Task LoadGames(List<Game> games)
        {
            Games = games;
        }

        public async Task<DiscordUser> GetDiscordUserFromID(ulong id)
        {
            return await Client.GetUserAsync(id);
        }

        public async Task LoadUsers(List<SaveableUser> users)
        {
            List<Task<DiscordUser>> tasks = new List<Task<DiscordUser>>();
            foreach (var user in users)
            {
                tasks.Add(GetDiscordUserFromID(user.ID));
            }

            foreach(var task in tasks)
            {
                Users.Add(await task);
            }

            
        }
        

        

        
        //public InteractivityModule Interactivity = new InteractivityModule();


        public List<UserHash> hashes = new List<UserHash>();

        

        public async Task<Tuple<DiscordUser, DiscordChannel>> GetUserFromHash(string hash)
        {
            
            for (int i = 0; i < hashes.Count; i++)
            {
                if (hashes[i].hash == hash)
                {
                    DiscordUser returnUser = hashes[i].user;
                    DiscordChannel returnChannel = hashes[i].channel;
                    hashes.Remove(hashes[i]);
                    return new Tuple<DiscordUser, DiscordChannel>(returnUser, returnChannel);
                }
            }

            return null;
        }

        
        public void AddUserHash(string hash , DiscordUser user, DiscordChannel channel)
        {
            UserHash userHash = new UserHash(user, hash, channel);
            UserHash r = new UserHash();
            bool found = false;
            
            foreach (var v in hashes)
            {
                if(v.user.Id == user.Id)
                {
                    found = true;
                    r = v;
                }
                
            }
            if(found)
            {
                hashes.Remove(r);
            }
            
            hashes.Add(userHash);
            
        }
        
    }
}