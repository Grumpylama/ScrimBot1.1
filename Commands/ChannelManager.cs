namespace big  
{
    public static class ChannelManager
    {
        public static Dictionary<DiscordUser, DiscordChannel> DMChannels = new Dictionary<DiscordUser, DiscordChannel>();
        
        
        public static async Task<DiscordChannel> GetDMChannelAsync(DiscordUser user)
        {
            if (DMChannels.ContainsKey(user))
            {
                return DMChannels[user];
            }
            else
            {
                return null;
            }
        }
    }
}