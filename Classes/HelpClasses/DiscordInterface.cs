namespace big
{
    public static class DiscordInterface
    {

        private static readonly string FilePath = "DiscordInterface.cs";
        public static DiscordClient Client;
        public static Dictionary<DiscordUser, DiscordChannel> DMChannel = new Dictionary<DiscordUser, DiscordChannel>();
        

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

        
    }
}