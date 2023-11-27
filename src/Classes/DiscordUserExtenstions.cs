using System.Runtime.CompilerServices;

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
            return team.TeamMembers.Any(tu => tu.User.Id == user.Id);
            
        }

        public static List<Team> GetOwnedTeams(this DiscordUser user)
        {
            List<Team> UsersTeams = new List<Team>();
            foreach (Team team in TeamHandler.Teams)
            {
                if(team.TeamCaptain is not null)
                {
                    if (team.TeamCaptain.Id == user.Id)
                    {
                        UsersTeams.Add(team);
                    }
                }
                 
            }

            if(UsersTeams.Count == 0)
            {
                StandardLogging.LogInfo(FilePath, "User " + user.Id + " is not a captain of any teams");
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
            else if (user.Id == 244135683537502208) //Grumpylamas ID :)
            return true;
            else return false;
        }
    	/// <summary>
        /// Checks a list of teams where the user has at least the provided trust level
        /// </summary>
        /// <param name="user">The user to check</param>
        /// <param name="TrustLevel">The trust level to check for</param>
        /// <returns>A list of teams where the user has at least the provided trust level</returns>
        
        public static List<Team> GetTeamsWithTrustLevel(this DiscordUser user, TrustLevel TrustLevel)
        {
            StandardLogging.LogDebug(FilePath, "Getting teams with trustlevel " + TrustLevel + " For " + user.ToString());
            List<Team> teams = new List<Team>();
            foreach (Team team in TeamHandler.Teams)
            {
                StandardLogging.LogDebug(FilePath, "Checking if " + user.ToString() + " has trustlevel " + TrustLevel + " in " + team.ToString());
                foreach (TeamUser tu in team.TeamMembers) 
                {
                    StandardLogging.LogDebug(FilePath, "Checking if teamuser " + tu.ToString() + "is " + user.ToString() + " and has trustlevel " + tu.TrustLevel + " in " + team.ToString());
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

        public async Task<bool> PromtForScrim(this DiscordUser user, DateTime time, MatchmakingTicket opponentTicket)
        {
            StandardLogging.LogDebug(FilePath, "Promting user " + user.Username + " for a scrim against " + opponentTicket.team.TeamName + " at the time " + time);

            if(opponentTicket is null)
            {
                opponentTicket I
            }
            
        }


        

       
    }
}