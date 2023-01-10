namespace big
{
    public static class DiscordInterface
    {
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
    }
}