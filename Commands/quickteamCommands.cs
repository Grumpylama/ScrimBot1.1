namespace big
{
    public static class QuickCommands
    {

        public static bool QuickMatchMake(CommandContext ctx, string TeamName, DateTime Time)
        {

            return false;

        }

        public static bool QuickAddToTeam(CommandContext ctx, string hash, string TeamName)
        {
            var UserToAdd = UserHandler.GetUserFromHashAsync(hash);
            var TeamToAddTo = TeamHandler.GetTeamFromName(TeamName);

            if (UserToAdd == null || TeamToAddTo == null)
            {
                return false;
            }

            if(!ctx.User.GetTeamsWithTrustLevel(TrustLevel.CanAddMembers).Contains(TeamToAddTo))
            {
                return false;
            }
            
            TeamToAddTo.AddMember(UserToAdd);
            return true;

            

        }
        
    }
}