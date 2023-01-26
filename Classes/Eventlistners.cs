namespace big
{

    public static class EventListners
    {

        public static async Task OnGuildMemberAdded(DiscordClient c ,GuildMemberAddEventArgs e)
        {
            StandardLogging.LogInfo("EventListners.cs", "User joined + " + e.Member.Username + " Joined");
            UserRegistration.RegisterUser(e.Member, await e.Member.CreateDmChannelAsync() );
        }

    }
}