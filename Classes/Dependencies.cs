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

        
        //public InteractivityModule Interactivity = new InteractivityModule();


        public static List<UserHash> hashes = new List<UserHash>();

        

        public static async Task<DiscordUser> GetUserFromHash(string hash)
        {
            for (int i = 0; i < hashes.Count; i++)
            {
                if (hashes[i].hash == hash)
                {
                    DiscordUser returnUser = hashes[i].user;
                    hashes.Remove(hashes[i]);
                    return returnUser;
                }
            }

            return null;
        }

        
        public static void AddUserHash(string hash , DiscordUser user)
        {
            UserHash userHash = new UserHash(user, hash);

            foreach (var v in hashes)
            {
                if(v.user.Id == user.Id)
                {
                    hashes.Remove(v);
                }
                
            }

            hashes.Add(userHash);
            
        }
        
    }
}