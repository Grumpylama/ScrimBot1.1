namespace big
{
    public static class DiscordUserExtensions
    {
        public static DiscordChannel GetDMChannel(this DiscordUser user)
        {
            if (DiscordInterface.DMChannel.ContainsKey(user))
            {
                return DiscordInterface.DMChannel[user];
            }
            else
            {
                return null;
            }
        }

        public static async Task<bool> SendDMAsync(this DiscordUser user, string message)
        {
            if (DiscordInterface.DMChannel.ContainsKey(user))
            {
                await DiscordInterface.DMChannel[user].SendMessageAsync(message);
                return true;
            }
            else
            {
                return false;
                
            }
        }

        public static List<Team> GetTeams(this DiscordUser user)
        {
            List<Team> teams = new List<Team>();
            foreach (Team team in TeamHandler.Teams)
            {
                foreach (TeamUser tu in team.TeamMembers)
                {
                    if (tu.User.Id == user.Id)
                    {
                        teams.Add(team);
                    }
                }
            }

            return teams;
        }

        public static bool IsInTeam(this DiscordUser user, Team team)
        {
            foreach (TeamUser tu in team.TeamMembers)
            {
                if (tu.User.Id == user.Id)
                {
                    return true;
                }
            }

            return false;
        }
    }
}