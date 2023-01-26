namespace big
{
    public static class DiscordUserExtensions
    {


        private static readonly string FilePath = "DiscordUserExtensions.cs";
        #pragma warning disable CS8603
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
                StandardLogging.LogInfo(FilePath, "User is not a captain of any teams");
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

        public static bool IsAdmin(this DiscordUser user)
        {
            if (DiscordInterface.AdminList.Contains(user))
            {
                return true;
            }
            else return false;
        }

        public static List<Team> GetTeamsWithTrustLevel(this DiscordUser user, TrustLevel TrustLevel)
        {
            List<Team> teams = new List<Team>();
            foreach (Team team in TeamHandler.Teams)
            {
                foreach (TeamUser tu in team.TeamMembers)
                {
                    if (tu.User.Id == user.Id && tu.TrustLevel >= TrustLevel)
                    {
                        teams.Add(team);
                    }
                }
            }

            return teams;

        } 

        public static bool CheckIfValid(this DiscordUser user)
        {
            //Check if user is a bot
            if (user.IsBot)
            {
                StandardLogging.LogInfo(FilePath, "User is a bot. Canceling command");
                return false;
            }

            return true;
        }

       
    }
}