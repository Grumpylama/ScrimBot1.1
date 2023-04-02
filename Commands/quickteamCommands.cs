namespace big
{
    public static class QuickCommands
    {
        private static readonly string FilePath = "QuickCommands.cs";

        public static bool QuickMatchMake(CommandContext ctx, string TeamName, DateTime Time)
        {

            return false;

        }

        public static bool QuickAddToTeam(CommandContext ctx, string hash, string TeamName)
        {
            var UserToAdd = StandardUserHandling.GetUserFromHashAsync(hash);
            var TeamToAddTo = TeamHandler.GetTeamFromName(TeamName);

            if (UserToAdd is null || TeamToAddTo is null)
            {
                return false;
                
            }

            if(!ctx.User.GetTeamsWithTrustLevel(TrustLevel.CanAddMembers).Contains(TeamToAddTo))
            {
                ctx.RespondAsync("Found no team with that name where you have the permission to add members");
                return false;
            }
            
            if(TeamToAddTo.GetMembers().Exists(x => x.User == UserToAdd))
            {
                ctx.RespondAsync("User is already in that team");
                return false;
            }

            TeamToAddTo.AddMember(UserToAdd);
            return false;  

        }

        public static async Task<bool> QuickTransferCaptain(CommandContext ctx, string TeamName, string NewCaptainID)
        {
            
            
            Team? teamToTransfer = ctx.User.GetOwnedTeams().Find(x => x.TeamName == TeamName);
            if(teamToTransfer is null)
            {
                await ctx.RespondAsync("Found no team with that name where you are captain");
                return false;
            }

            if(!ulong.TryParse(NewCaptainID, out ulong NewCaptainIDParsed))
            {
                await ctx.RespondAsync("Could not parse ID");
                return false;
            }

            var newCaptain = await StandardUserHandling.GetDiscordUserFromIDAsync(NewCaptainIDParsed);

            if(newCaptain is null || teamToTransfer.GetMembers().Find(x => x.User == newCaptain) is null)
            {
                await ctx.RespondAsync("Found no user with that ID in your team");
                return false;
            }

            if(teamToTransfer.TeamCaptain == newCaptain)
            {
                await ctx.RespondAsync("You can't transfer the captain role to yourself");
                return false;
            }

            teamToTransfer.TeamCaptain = newCaptain;
            var oldCaptainTeamUser = teamToTransfer.GetNonCaptainMembers().Find(x => x.User == ctx.User);
            if(oldCaptainTeamUser is not null)
            {
                oldCaptainTeamUser.TrustLevel = TrustLevel.Member;
                oldCaptainTeamUser.Position = "Member";
            }
            else if(oldCaptainTeamUser is null)
            {
                StandardLogging.LogError(FilePath, "Could not find old captain in team members");
            }
            
            var newCaptainTU = teamToTransfer.GetMembers().Find(x => x.User == newCaptain);
            if(newCaptainTU is not null)
            {
                newCaptainTU.TrustLevel = TrustLevel.TeamCaptain;
                newCaptainTU.Position = "Team Captain";
            }
            else if(newCaptainTU is null)
            {
                StandardLogging.LogError(FilePath, "Could not find new captain in team members");
            }


            StandardLogging.LogInfo(FilePath, $"User {ctx.User.ToString()} transfered captainship of {teamToTransfer} to {teamToTransfer.TeamCaptain.ToString()}");
            var t = newCaptain.SendDMAsync("You are now the captain of " + teamToTransfer);    
            await ctx.Channel.SendMessageAsync($"Captain was transfered!  {teamToTransfer.TeamCaptain.ToString() } is now the new captain of {teamToTransfer.TeamName}").ConfigureAwait(false);
            await t;

            return true;

        }

        public static async Task<bool> quickViewTeam(CommandContext ctx, string teamName)
        {
            var team = TeamHandler.GetTeamFromName(teamName);
            if(team is null || !team.GetMembers().Exists(x => x.User == ctx.User))
            {
                StandardLogging.LogInfo(FilePath, $"User {ctx.User.ToString()} tried to view team {teamName} but was not in it or it did not exist");
                await ctx.RespondAsync("Found no team with that name");
                return false;
            }

            await ctx.Client.SendMessageAsync(ctx.Channel, team.ToDiscordString());
            return true;

        }


        
    }
}