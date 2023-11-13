namespace big
{
    public static class StandardUserHandling
    {

        private static readonly string FilePath = "UserHandler.cs";
        public static List<DiscordUser> Users = new List<DiscordUser>();


        public static DiscordUser GetDiscordUserFromUsername(string username)
        {
            string name = username.Split('#')[0];
            string discriminator = username.Split('#')[1];

            StandardLogging.LogDebug(FilePath, "Getting user " + username + "From username");
            DiscordUser? user = null;

            if((user = Users.Find(x => x.Username == name && discriminator == x.Discriminator)) is not null)
            {
                StandardLogging.LogDebug(FilePath, "User " + username + " found");
                return user;
            }
            else
            {
                StandardLogging.LogError(FilePath, "User " + username + " not found");
                throw new Exception("User not found");
            }
            
        }
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
            
            StandardLogging.LogDebug(FilePath, "Getting user " + id);

            DiscordUser? user = null;
            if((user = Users.Find(x => x.Id == id)) is not null)
            {
                StandardLogging.LogDebug(FilePath, "User " + id + " found");
                return user;
            }
            else
            {
                StandardLogging.LogError(FilePath, "User " + id + " not found");
                throw new Exception("User not found");
            }
        }

        public static async Task LoadUsersAsync(List<SaveableUser> users)
        {
            List<Tuple<Task<DiscordUser>, Task<DiscordChannel>>> UserGetTasks = new List<Tuple<Task<DiscordUser>, Task<DiscordChannel>>>();
            
            foreach (var user in users)
            {
                var T = new Tuple<Task<DiscordUser>, Task<DiscordChannel>>(GetDiscordUserFromIDAsync(user.ID), GetDiscordChannelFromIDAsync(user.DMChannelID));
                UserGetTasks.Add(T);
            }

            foreach (var task in UserGetTasks)
            {
                StandardLogging.LogDebug(FilePath, "Getting DiscordUser and DiscordChannel from ID: " + task.Item1.Id);
                DiscordChannel? channel;
                DiscordUser? user;
                try
                {
                    user = await task.Item1;
                    
                }
                catch
                {
                    StandardLogging.LogError(FilePath, "User not found");
                    continue;
                }
                try
                {
                    channel = await task.Item2;
                }
                catch
                {
                    StandardLogging.LogError(FilePath, "Channel not found");
                    continue;
                }
                 
                StandardLogging.LogDebug(FilePath, "Got DiscordUser: " + user + " and ChannelID: " + channel.Id);
                 
                StandardLogging.LogDebug(FilePath, "Got DiscordChannel: " + channel);
                if(user is null || channel is null)
                {
                    StandardLogging.LogError(FilePath, "User or Channel was null");
                    continue;
                }
                AddUser(user);
                DiscordInterface.AddDmChannel(user, channel);
            }
            
        }

        public static async Task<DiscordUser> GetDiscordUserFromIDAsync(ulong id)
        {
            try
            {
                return await DiscordInterface.Client!.GetUserAsync(id);
            }
            catch(Exception e)
            {
                StandardLogging.LogError(FilePath, e.Message);
                throw new Exception("User not found");
            }
            
        }

        public static async Task<DiscordChannel> GetDiscordChannelFromIDAsync(ulong id)
        {
            return await DiscordInterface.Client!.GetChannelAsync(id);
        }

        

        public static Dictionary<String, DiscordUser> hashes = new Dictionary<String, DiscordUser>();

        public static DiscordUser? GetUserFromHashAsync(string hash)
        {

            StandardLogging.LogDebug(FilePath, "Getting user " + hash);
            
            if(hashes.ContainsKey(hash))
            {
                
                var ReturnUser = hashes[hash];
                hashes.Remove(hash);
                StandardLogging.LogDebug(FilePath, "User " + hash + " found and is " + ReturnUser.ToString());
                return ReturnUser;
            }
            

            StandardLogging.LogInfo(FilePath, "User " + hash + " not found");
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
        private static readonly string filepath = "UserRegistration.cs";
        public static void RegisterUser(DiscordUser user, CommandContext ctx)
        {
            if(!StandardUserHandling.Users.Contains(user))
            {
                StandardUserHandling.AddUser(user);
                DiscordInterface.AddDmChannel(user, ctx.Channel);
             
                StandardLogging.LogInfo(filepath,  user + " Registered");
                ctx.Client.SendMessageAsync(ctx.Channel, "Since you were not part of our system before you have been registered. In order for the system to work correctly you will need to stay in the discord server");            
            }
        
        }

        public static void RegisterUser(CommandContext ctx)
        {
            if(!StandardUserHandling.Users.Contains(ctx.User))
            {
                StandardUserHandling.AddUser(ctx.User);
                DiscordInterface.AddDmChannel(ctx.User, ctx.Channel);
                
                StandardLogging.LogInfo(filepath,  ctx.User + " Registered");
                ctx.Client.SendMessageAsync(ctx.Channel, "Since you were not part of our system before you have been registered. In order for the system to work correctly you will need to stay in the discord server");            
            }
        
        }

        public static bool RegisterUser(DiscordUser user, DiscordChannel channel)
        {
            if(!StandardUserHandling.Users.Contains(user))
            {
                StandardUserHandling.AddUser(user);
                DiscordInterface.AddDmChannel(user, channel);
                
                StandardLogging.LogInfo(filepath,  user + " Registered");
                return true;
            }
            else
            {
                return false;
            }
        
        }

        
    }
    
}