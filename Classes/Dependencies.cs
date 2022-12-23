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
    
    public static class Dependecies
    {
        
        public static List<Team> teams = new List<Team>();
        public static List<Game> games = new List<Game>();
        public static List<DiscordUser> Users = new List<DiscordUser>();

        public static async Task<DiscordUser> GetUserFromID(ulong id)
        {
            foreach (var v in Users)
            {
                if (v.Id == id)
                {
                    return v;
                }
            }
            return null;
        }

        
        //public InteractivityModule Interactivity = new InteractivityModule();


        public static List<UserHash> hashes = new List<UserHash>();

        

        public static async Task<Tuple<DiscordUser, DiscordChannel>> GetUserFromHash(string hash)
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

        
        public static void AddUserHash(string hash , DiscordUser user, DiscordChannel channel)
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