namespace big
{
    public static class TeamHandler
    {
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
    }
}