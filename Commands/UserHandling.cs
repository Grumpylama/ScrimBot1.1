namespace big
{
    public static class UserHandler
    {
        public static List<DiscordUser> Users = new List<DiscordUser>();



        public static void RegisterUser(DiscordUser user, CommandContext ctx)
        {
            if (!Users.Contains(user))
            {
                Users.Add(user);
                Console.WriteLine("User registered");

                

            }


        }

        public static DiscordUser GetUserFromID(ulong id)
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

        public static async Task LoadUsersAsync(List<SaveableUser> users)
        {
            List<Task<DiscordUser>> tasks = new List<Task<DiscordUser>>();
            foreach (var user in users)
            {
                tasks.Add(GetDiscordUserFromIDAsync(user.ID));
            }

            foreach(var task in tasks)
            {
                Users.Add(await task);
            }

            
        }

        public static async Task<DiscordUser> GetDiscordUserFromIDAsync(ulong id, Dependecies d)
        {
            return await d.Client.GetUserAsync(id);
        }

        public static List<UserHash> hashes = new List<UserHash>();

        public static async Task<Tuple<DiscordUser, DiscordChannel>> GetUserFromHashAsync(string hash)
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