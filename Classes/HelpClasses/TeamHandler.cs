namespace big
{
    public static class TeamHandler
    {

        private static readonly string FilePath = "TeamHandler.cs";
        public static List<Team> Teams = new List<Team>();



        public static Team GetTeamFromID(int id)
        {
            foreach (var team in Teams)
            {
                if (team.teamID == id)
                {
                    return team;
                }
            }

            return null;
        }

        public static bool IsTeamNameTaken(string name)
        {
            foreach (var team in Teams)
            {
                if (team.TeamName == name)
                {
                    return true;
                }
            }

            return false;
        }

        public static void VarDump()
        {
            foreach (var team in Teams)
            {
                StandardLogging.LogInfo(FilePath, "Team: " + team.TeamName + " with ID " + team.teamID + " and game " + team.game.GameName + " with Captain " + team.TeamCaptain);
                foreach (var user in team.TeamMembers)
                {
                    StandardLogging.LogInfo(FilePath, "     User: " + user.User + " with role " + user.roleID + " and position " + user.Position );
                }
            }
        }
    }
}