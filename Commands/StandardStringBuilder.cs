namespace big
{
    public static class StandardStringBuilder
    {
        public static string BuildTeamString(Team t)
        {
            string s = "Team: " + t.TeamName + "\nCaptain: " + t.TeamCaptain.Username + "\nMembers: ";
            foreach (TeamUser member in t.TeamMembers)
            {
                s += member.User.Username + ", ";
            }
            return s;
        }

        public static string BuildTeamListString(List<Team> teams)
        {
            string s = "";
            int i = 1;
            foreach (Team t in teams)
            {
                s += i + ". " + t.TeamName + "\n";
                i++;
            }
            return s;
        }

        public static string BuildUserListString(List<DiscordUser> users)
        {
            string s = "";
            int i = 1;
            foreach (DiscordUser user in users)
            {
                s += i + ". " + user.Username + "#" + user.Discriminator + "\n";
                i++;
            }
            return s;
        }
    }
}