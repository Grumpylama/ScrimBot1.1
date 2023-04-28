namespace big
{
    public static class DiscordInterface
    {

        private static readonly string FilePath = "DiscordInterface.cs";
        public static DiscordClient? Client;
        public static Dictionary<DiscordUser, DiscordChannel> DMChannel = new Dictionary<DiscordUser, DiscordChannel>();

        public static List<DiscordUser> AdminList = new List<DiscordUser>();
        

        public static async Task<DiscordChannel> GetDMChannelAsync(DiscordUser user)
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

        public static void AddDmChannel(DiscordUser user, DiscordChannel channel)
        {
            if (!DMChannel.ContainsKey(user))
            {
                DMChannel.Add(user, channel);
            }
        }

        public static void VarDump()
        {
            foreach (var user in DMChannel)
            {
                StandardLogging.LogInfo(FilePath, "User: " + user.Key.Username + " with channel " + user.Value.Id);
            }
        }

        
    }
}