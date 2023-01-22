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

        public static List<Team> GetOwnedTeams(this DiscordUser user)
        {
            List<Team> UsersTeams = new List<Team>();
            foreach (Team team in TeamHandler.Teams)
            {
                if (team.TeamCaptain.Id == user.Id)
                {
                    UsersTeams.Add(team);
                }
            }

            if(UsersTeams.Count == 0)
            {
                Console.WriteLine("User is not a captain of any teams");
                return null;
            }
            return UsersTeams;
        }

        public static async Task<bool> SendMessageAsync(this DiscordUser user, string message)
        {
            if (DiscordInterface.DMChannel.ContainsKey(user))
            {
                await DiscordInterface.DMChannel[user].SendMessageAsync(message);
                return true;
            }
            else return false;
        }

        public static async Task<bool> SendMessageAsync(this DiscordUser user, string message, int timeout)
        {
            if (DiscordInterface.DMChannel.ContainsKey(user))
            {
                await DiscordInterface.DMChannel[user].SendMessageAsync(message);
                
                return true;
            }
            else return false;
        }

        public static SaveableUser ToSavableUser(this DiscordUser user)
        {
            return new SaveableUser(user.Id, user.GetDMChannel().Id);
        }
    }
}