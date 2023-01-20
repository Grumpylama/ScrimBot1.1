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

        public static string BuildGamePromtString(List<Game> games)
        {
            return "Which game will you be playing? \n" +  BuildGameListString(games);
        }
        public static string BuildGameListString(List<Game> games)
        {
            string s = "";
            int i = 1;
            foreach (Game game in games)
            {
                s += i + ". " + game.GameName + "\n";
                i++;
            }
            return s;
        }

        public static string BuildTeamConfirmationString(Team t, string verb)
        {
            string s = "Are you sure you want to " + verb + " " + t.TeamName + "?" + "\n Type \"CONFIRM\" to confirm or \"CANCEL\" to cancel";
            return s;
        }

        
    }
}