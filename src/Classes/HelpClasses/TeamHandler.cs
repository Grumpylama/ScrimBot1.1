namespace big
{
    public static class TeamHandler
    {

        private static readonly string FilePath = "TeamHandler.cs";
        public static List<Team> Teams = new List<Team>();

        
        


        public static Team GetTeamFromID(int id)
        {
            StandardLogging.LogDebug(FilePath, "Getting team " + id);
            Team? t = null;

            if((t = Teams.Find(x => x.teamID == id)) is not null)
            {
                StandardLogging.LogDebug(FilePath, "Team " + id + " found. Team is: " + t.TeamName);
                return t;
            }
            else
            {
                StandardLogging.LogError(FilePath, "Team " + id + " not found");
                throw new Exception("Team not found");
            }
        }

        public static bool IsTeamNameTaken(string name)
        {
            StandardLogging.LogDebug(FilePath, "Checking if team name " + name + " is taken");
            return Teams.Exists(team => team.TeamName == name);
            
        }

        public static void VarDump()
        {
            foreach (var team in Teams)
            {
                StandardLogging.LogInfo(FilePath, "Team: " + team.TeamName + " with ID " + team.teamID + " and game " + team.game.GameName + " with Captain " + team.TeamCaptain + " Created at " + team.CreationTime );
                foreach (var user in team.TeamMembers)
                {
                    StandardLogging.LogInfo(FilePath, "     User: " + user.User + " with position " + user.Position + " and trust " + user.TrustLevel + " and joined at " + user.JoinDate );
                }
            }
        }

        public static Team GetTeamFromName(string name)
        {
            StandardLogging.LogDebug(FilePath, "Getting team " + name);

            if(Teams.Exists(x => x.TeamName == name))
            {
                StandardLogging.LogDebug(FilePath, "Team " + name + " found");
                return Teams.Find(x => x.TeamName == name)!;
            }
            else
            {
                StandardLogging.LogError(FilePath, "Team " + name + " not found");
                throw new Exception("Team not found");
            }
        }

        public static bool ForceUpdateCaptainDMChannels()
        {
            bool success = true;
            foreach (var team in Teams)
            {
                if(team.updateCaptainChannel())
                {
                    StandardLogging.LogInfo(FilePath, "Updated Captain Channel for team " + team.TeamName);
                }
                else
                {
                    StandardLogging.LogError(FilePath, "Failed to update Captain Channel for team " + team.TeamName);
                    success = false;
                }
                
            }

            return success;
        }

        
    }
}