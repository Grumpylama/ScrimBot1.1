namespace big
{
    public static class UserHandler
    {
        public static List<DiscordUser> Users = new List<DiscordUser>();

        public static void AddUser(DiscordUser u)
        {
            if(!Users.Contains(u))
                Users.Add(u);
            
        }

        public static bool CheckIfRegistred(CommandContext ctx)
        {
            if(CheckIfRegistred(ctx.User))
            {
                return true;
            }
            else
            {
                UserRegistration.RegisterUser(ctx.User, ctx);
                return false;
            }
        }

        private static bool CheckIfRegistred(DiscordUser user)
        {
            if (Users.Contains(user))
            {
                return true;
            }
            else
            {
                return false;
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

        public static async Task<DiscordUser> GetDiscordUserFromIDAsync(ulong id)
        {
            return await DiscordInterface.Client.GetUserAsync(id);
        }

        

        public static Dictionary<String, DiscordUser> hashes = new Dictionary<String, DiscordUser>();

        public static DiscordUser GetUserFromHashAsync(string hash)
        {
            
            if(hashes.ContainsKey(hash))
            {
                var ReturnUser = hashes[hash];
                hashes.Remove(hash);
                return ReturnUser;
            }
            

            return null;
        }

        public static void AddUserHash(string hash , DiscordUser user, DiscordChannel channel)
        {       
            
            if(!hashes.ContainsKey(hash))
            {
                hashes.Remove(hash);
            }
            hashes.Add(hash, user);            
        }

    }

    public static class UserRegistration
    {
        public static void RegisterUser(DiscordUser user, CommandContext ctx)
        {
            if(!UserHandler.Users.Contains(user))
            {
                UserHandler.AddUser(user);
                DiscordInterface.AddDmChannel(user, ctx.Channel);
                Console.WriteLine(user + " Registered");
                ctx.Client.SendMessageAsync(ctx.Channel, "Since you were not part of our system before you have been registered. In order for the system to work correctly you will need to stay in the discord server");            
            }
        
        }

        public static void RegisterUser(CommandContext ctx)
        {
            if(!UserHandler.Users.Contains(ctx.User))
            {
                UserHandler.AddUser(ctx.User);
                DiscordInterface.AddDmChannel(ctx.User, ctx.Channel);
                Console.WriteLine(ctx.User + " Registered");
                ctx.Client.SendMessageAsync(ctx.Channel, "Since you were not part of our system before you have been registered. In order for the system to work correctly you will need to stay in the discord server");            
            }
        
        }
    }
    
}