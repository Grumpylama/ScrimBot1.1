namespace big
{
    public static class StandardStringBuilder
    {
        private static readonly string FilePath = "StandardStringBuilder.cs";

        public static string BuildTeamString(Team t)
        {
            string s = "Team: " + t.TeamName + "\nCaptain: " + t.TeamCaptain.Username + "\nMembers: ";
            foreach (TeamUser member in t.TeamMembers)
            {
                s += member.User.Username + ", ";
            }
            StandardLogging.LogInfo(FilePath, "BuildTeamString: Built team string: \n" + s);
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
            StandardLogging.LogInfo(FilePath, "BuildTeamListString: Built team list string: \n" + s);
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
            StandardLogging.LogInfo(FilePath, "BuildUserListString: Built user list string: \n" + s);
            return s;
        }

        public static string BuildGamePromtString(List<Game> games)
        {
            string s = "Which game will you be playing? \n" +  BuildGameListString(games);
            StandardLogging.LogInfo(FilePath, "BuildGamePromtString: Built game promt string: \n" + s);
            return s;
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
            StandardLogging.LogInfo(FilePath, "BuildGameListString: Built game list string: \n" + s);
            return s;
        }

        public static string BuildTeamConfirmationString(Team t, string verb)
        {
            string s = "Are you sure you want to " + verb + " " + t.TeamName + "?" + "\n Type \"CONFIRM\" to confirm or \"CANCEL\" to cancel";
            StandardLogging.LogInfo(FilePath, "BuildTeamConfirmationStrng: Built confirmation string: " + s);
            return s;
        }

        
    }
}