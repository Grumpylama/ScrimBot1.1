


namespace big
{
    public partial class Commands : BaseCommandModule
    {
        
        private static readonly string FilePath = "Commands.cs";

        [Command("DeleteTeam")]
        public async Task DeleteTeam(CommandContext ctx)
        {
            
            StandardLogging.LogInfo(FilePath, "DeleteTeam command was used by " + ctx.User.ToString());
            StandardUserHandling.CheckIfRegistred(ctx);
            if (!ctx.User.CheckIfValid())
            {
                return;
            }

            
            //Getting all teams that user is captain of
            List<Team> teams = ctx.User.GetOwnedTeams();
            if(teams == null)
            {
                
                StandardLogging.LogInfo(FilePath, "User has no teams. Canceling DeleteTeam");
                await ctx.Channel.SendMessageAsync("You have no teams!").ConfigureAwait(false);
                return;
            }


            //Chosing what team to delete
            var Response = await StandardUserInteraction.ChooseTeamAsync(ctx, teams);

            if(Response.Success == false)
            {
                
                StandardLogging.LogInfo(FilePath, "User canceled DeleteTeam");
                await ctx.Channel.SendMessageAsync("Canceled!").ConfigureAwait(false);
                return;
            }

            Team team = Response.ResponseItem;
            if(team == null)
            {
                
                StandardLogging.LogInfo(FilePath, "User did not choose a team. Canceling DeleteTeam");
                await ctx.Channel.SendMessageAsync("Canceled!").ConfigureAwait(false);
                return;
            } 


            //Confirming deletion
            

            if (await StandardInteractivityHandler.GetConfirmation(ctx, "Are you sure you want to delete your team: " + team.TeamName + " playing: " + team.game.GameName + "? \n To confirm write \"CONFIRM\"" ))
            {
                
                StandardLogging.LogInfo(FilePath, "Team " + team.TeamName + " deleted by " + ctx.User.ToString());
                TeamHandler.Teams.Remove(team);
                await ctx.Channel.SendMessageAsync("Team deleted!").ConfigureAwait(false);
                return;
            }            


            //Canceling deletion        
            
            StandardLogging.LogInfo(FilePath, "User did not confirm deletion of team");
            await ctx.Channel.SendMessageAsync("Canceled!").ConfigureAwait(false);   
            return;         
            
        }

        [Command("CreateTeam")]
        public async Task CreateTeam(CommandContext ctx, string TeamName)
        {

            
            StandardLogging.LogInfo(FilePath, "CreateTeam command was used by " + ctx.User.ToString());
            StandardUserHandling.CheckIfRegistred(ctx);
            if (!ctx.User.CheckIfValid())
            {  
                return;
            }

            if(TeamHandler.IsTeamNameTaken(TeamName))
            {
                
                StandardLogging.LogInfo(FilePath, "Team name is taken. Canceling CreateTeam");
                await ctx.Channel.SendMessageAsync("Team name is taken!").ConfigureAwait(false);
                return;
            }


            var response = await StandardUserInteraction.ChooseGameAsync(ctx, GameHandler.Games);
            if(response.Success == false)
            {
                
                StandardLogging.LogInfo(FilePath, "User canceled CreateTeam");
                await ctx.Channel.SendMessageAsync("Canceled!").ConfigureAwait(false);
                return;
            }

            Game game = response.ResponseItem;

            if(game == null)
            {

                StandardLogging.LogInfo(FilePath, "User did not choose a game. Canceling CreateTeam");
                await ctx.Channel.SendMessageAsync("Canceled!").ConfigureAwait(false);
                return;
            }              
            var t = new Team(game, TeamName, ctx.User);
            StandardLogging.LogInfo(FilePath, "Team " + t.TeamName + " created by " + ctx.User.ToString() + " playing " + t.game.GameName);
            TeamHandler.Teams.Add(t);
            await ctx.Client.SendMessageAsync(ctx.Channel, "Team named " + TeamName + " playing " + game.GameName + " created!").ConfigureAwait(false);
            
        }
       
        [Command("TransferCaptain")]
        public async Task TransferCaptain(CommandContext ctx, string TeamName = "", string NewCaptainID = "")
        {
            
            StandardLogging.LogInfo(FilePath, "TransferCaptain command was used by " + ctx.User.ToString());
            StandardUserHandling.CheckIfRegistred(ctx);
            if (!ctx.User.CheckIfValid())
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " is a bot Canceling TransferCaptain");
                return;
            }

            if(TeamName != "" && NewCaptainID != "")
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " used TransferCaptain with parameters. Using Quick version");
                await QuickCommands.QuickTransferCaptain(ctx, TeamName, NewCaptainID);
                return;
            }


            //Getting all teams that user is captain of

            List<Team> teams = ctx.User.GetOwnedTeams();
            if (teams is null)
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " has no teams. Canceling TransferCaptain");
                await ctx.Channel.SendMessageAsync("You have no teams!").ConfigureAwait(false);
                return;
            }

            var response = await StandardUserInteraction.ChooseTeamAsync(ctx, teams);

            if (response.Success == false)
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " did not choose a team. Canceling TransferCaptain");
                await ctx.Channel.SendMessageAsync("Canceled!").ConfigureAwait(false);
                return;
            }

            Team teamToTransfer = response.ResponseItem;

            if (teamToTransfer is null)
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " did not choose a team. Canceling TransferCaptain");
                await ctx.Channel.SendMessageAsync("Canceled!").ConfigureAwait(false);
                return;
            }

            //Getting all users that are not captain of the team
            var otherMembers = teamToTransfer.GetNonCaptainMembers().Select(x => x.User).ToList();
            

            if(otherMembers.Count() == 0)
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " is the only member of the team. Canceling TransferCaptain");
                
                await ctx.Channel.SendMessageAsync("You are the only member of the team!").ConfigureAwait(false);
                return;
            }


            var response2 = await StandardUserInteraction.ChooseUserAsync(ctx, otherMembers);

            if (response2.Success == false)
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " did not choose a user. Canceling TransferCaptain");
                await ctx.Channel.SendMessageAsync("Canceled!").ConfigureAwait(false);
                return;
            }

            DiscordUser newCaptain = response2.ResponseItem;

            StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " chose " + newCaptain.ToString() + " as new captain of " + teamToTransfer.TeamName);

            #pragma warning disable CS8625
            if(newCaptain == null)
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " did not choose a user. Canceling TransferCaptain");
                
                await ctx.Channel.SendMessageAsync("Canceled!").ConfigureAwait(false);
                return;
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
            return;
        }

        [Command("JoinTeam")]
        public async Task JoinTeam(CommandContext ctx)
        {
            
            StandardLogging.LogInfo(FilePath, "JoinTeam command was used by " + ctx.User.ToString());
            StandardUserHandling.CheckIfRegistred(ctx);
            if (!ctx.User.CheckIfValid())
            {
                return;
            }
            
            StandardLogging.LogInfo(FilePath, "JoinTeam command was used by " + ctx.User.ToString());
            HashAlgorithm sha = SHA256.Create();

            string now = DateTime.Now.ToString("HH::mm::ss:ffffff");
            
            
            string hash = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(now + ctx.User.ToString())));


            StandardLogging.LogInfo(FilePath, "Hash generated for " + ctx.User.ToString() + " is " + hash);

            Random r = new Random();
            int i = r.Next(0, hash.Length - 10);
            hash = hash.Substring(i, 10);

            StandardLogging.LogInfo(FilePath, "Final hash generated for " + ctx.User.ToString() + " is " + hash);
          


            StandardUserHandling.AddUserHash(hash, ctx.User, ctx.Channel);
            
            await ctx.Channel.SendMessageAsync("A captain can enter the following code to add you to a team: \n " + hash);
        }

        [Command("AddToTeam")]
        public async Task AddToTeam(CommandContext ctx, string hash, string TeamName = "")
        {
            StandardLogging.LogInfo(FilePath, "AddToTeam command was used by " + ctx.User.ToString());
            
            StandardUserHandling.CheckIfRegistred(ctx);
            if(!ctx.User.CheckIfValid())
            {          
                return;
            }

            if(TeamName != "")
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " is trying to add a user to a specific team");
                QuickCommands.QuickAddToTeam(ctx, hash, TeamName);
                return;
            }

            
            StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " is trying to add a user with hash " + hash);
            DiscordUser? userToAdd = StandardUserHandling.GetUserFromHashAsync(hash);
            
            

            if(userToAdd is null)
            {
                StandardLogging.LogInfo(FilePath, "Could not find a user with hash " + hash);
                await ctx.Channel.SendMessageAsync("Could not find user with that hash. Note that each hash can only be used once");
                return;
            }


            //Check if user is trying to add himself/herself to a team
            if(userToAdd.Id == ctx.User.Id)
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " is trying to add himself/herself to a team");
                await ctx.Channel.SendMessageAsync("You cannot add yourself to a team");
                return;
            }

            
            StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " is trying to add " + userToAdd.ToString() + " to a team");
            List<Team> UsersTeams = ctx.User.GetOwnedTeams();

            
            if(UsersTeams.Count == 0)
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " is not a captain of any teams");
                await ctx.Channel.SendMessageAsync("You are not a captain of any teams");
                return;
            }
            
            StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " is a captain of " + UsersTeams.Count + " teams");
            
            
            var response = await StandardUserInteraction.ChooseTeamAsync(ctx, UsersTeams);

            if(response.Success == false)
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " did not choose a team. Canceling AddToTeam");
                await ctx.Channel.SendMessageAsync("Canceled!");
                return;
            }

            Team Team = response.ResponseItem;

            if (Team is null)
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " did not choose a team. Canceling AddToTeam");
                return;
            }
                

            //Check if user is already in team
            if (userToAdd.IsInTeam(Team))
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " is already in team " + Team);
                await ctx.Channel.SendMessageAsync("User is already in team");
                return;
            }
            
           
            StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " is adding " + userToAdd.ToString() + " to team " + Team);

            Team.TeamMembers.Add(new TeamUser(userToAdd, Team.teamID, "Member"));
            var t = userToAdd.SendDMAsync("You were added to the team: " + Team.TeamName + " By " + ctx.User.Username + "#" + ctx.User.Discriminator);
            await ctx.Channel.SendMessageAsync("User was added to team: " + Team.TeamName);
            if(!await t)
            {
                StandardLogging.LogInfo(FilePath, "Could not send DM to " + userToAdd.ToString());
                await ctx.Channel.SendMessageAsync("Could not send DM to, " + userToAdd.Username + "#" + userToAdd.Discriminator + " please make sure they have DMs enabled, and is a member of the server. They were still added to your team");
                return;
            }
            return;
        }
        
        [Command("LeaveTeam")]
        public async Task LeaveTeam(CommandContext ctx)
        {
            try{
                StandardLogging.LogInfo(FilePath, "LeaveTeam command was used by " + ctx.User.ToString());
                StandardUserHandling.CheckIfRegistred(ctx);
                if (!ctx.User.CheckIfValid())
                {
                    return;
                }
                
                //Getting all the teams the user is in
                var teams = ctx.User.GetTeams();
                if(teams.Count == 0)
                {
                    StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " is not in any teams");
                    await ctx.Channel.SendMessageAsync("You are not in any teams");
                    return;
                }


                //Getting what team the user wants to leave
                var response = await StandardUserInteraction.ChooseTeamAsync(ctx, teams);

                if(response.Success == false)
                {
                    StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " did not choose a team. Canceling LeaveTeam");
                    await ctx.Channel.SendMessageAsync("Canceled!");
                    return;
                }

                Team team = response.ResponseItem;

                if (team is null)
                    return;

                if(team.TeamCaptain is not null)
                {
                    if(team.TeamCaptain.Id == ctx.User.Id)
                    {
                        await ctx.Channel.SendMessageAsync("You are the captain of this team. You cannot leave the team. \n If you want to leave the team you must transfer the captain role to another member of the team or delete the team");
                        return;
                    }
                }
                

                //Getting confirmation from user
                if(!await StandardInteractivityHandler.GetConfirmation(ctx, StandardStringBuilder.BuildTeamConfirmationString(team, "Leave")))           
                return;
                
                #pragma warning disable CS8604
                //Removing user from team and notifying Captain
                team.TeamMembers.Remove(team.TeamMembers.Find(x => x.User.Id == ctx.User.Id));
                
                var t = team.TeamCaptain.SendDMAsync(ctx.User.Username + "#" + ctx.User.Discriminator + " has left the team: " + team.TeamName);
                await ctx.Channel.SendMessageAsync("You have left the team: " + team.TeamName);
                await t;
                return;
            }
            catch (Exception e)
            {
                StandardLogging.LogError(FilePath, e.Message);
                try {
                    await ctx.Channel.SendMessageAsync("An error has occured. Please try again later");
                }
                catch (Exception e2)
                {
                    StandardLogging.LogError(FilePath, "Failed to send error message: " +  e2.Message);
                }
            }
            
        }

        

        

        [Command("ManageMembers")]
        [Description("Manage the members of a team")]
        [Aliases("ManageTeam", "ManageTeamMembers", "ManageTrust", "ManageTrustLevels")]
        public async Task ManageMembers(CommandContext ctx)
        {
            StandardLogging.LogInfo(FilePath, "ManageTeam command was used by " + ctx.User.ToString());
            StandardUserHandling.CheckIfRegistred(ctx);
            if (!ctx.User.CheckIfValid())
            {
                return;
            }

            //Getting all the teams where the user has permission to manage
            var teams = ctx.User.GetTeamsWithTrustLevel(TrustLevel.CanEditTrustLevels);

            if (teams.Count == 0)
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " is not allowed to manage any teams");
                await ctx.Channel.SendMessageAsync("You do not have permission to manage any teams");
                return;
            }

            
            //Getting what team the user wants to manage
            var response = await StandardUserInteraction.ChooseTeamAsync(ctx, teams);

            if(response.Success == false)
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " did not choose a team. Canceling ManageTeam");
                await ctx.Channel.SendMessageAsync("Canceled!");
                return;
            }

            Team team = response.ResponseItem;

            if (team is null)
                return;

            //Getting what user the user wants to manage
            TeamUser? callUser;
            
            callUser = team.TeamMembers.Find(x => x.User.Id == ctx.User.Id);
            List<TeamUser> teamUsers = new List<TeamUser>();

            if(callUser is null)
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " is not a member of team " + team.TeamName);
                await ctx.Channel.SendMessageAsync("Something went wrong please try again");
            }



            if(callUser is not null)
            {
                if(callUser.TrustLevel is TrustLevel.TeamCaptain)
                {
                    teamUsers = team.GetNonCaptainMembers();
                }
                else
                {
                    teamUsers = team.TeamMembers.FindAll(x => x.User.Id != ctx.User.Id && x.User.Id != team.TeamCaptain!.Id && x.TrustLevel < TrustLevel.CanEditTrustLevels);
                }
            }
            
            
            
            
            StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " is managing " + teamUsers.Count + " users in team " + team.TeamName);

            if (teamUsers.Count is 0)
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " is not allowed to manage any users in team " + team.TeamName);
                await ctx.Channel.SendMessageAsync("You do not have permission to manage any users in this team");
                return;
            }

            //Getting what user the user wants to manage
            var response2 = await StandardUserInteraction.ChooseTeamUserAsync(ctx, teamUsers);

            if(response2.Success == false)
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " did not choose a user. Canceling ManageTeam");
                await ctx.Channel.SendMessageAsync("Canceled!");
                return;
            }

            TeamUser userToManage = response2.ResponseItem;


            if (userToManage == null)
            {
                return;
            }
   

            //Getting what trust level the user wants to give the user
    

            

            if(callUser is null)
            {
                StandardLogging.LogError(FilePath, "Could not find user in team members");
                return;
            }
            
            var response3 = await StandardUserInteraction.ChooseTrustLevelAsync(ctx, callUser!.TrustLevel);

            if(response3.Success == false)
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " did not choose a trustlevel. Canceling ManageTeam");
                await ctx.Channel.SendMessageAsync("Canceled!");
                return;
            }

            TrustLevel trustLevelToGive = response3.ResponseItem;
            
            if(trustLevelToGive is not TrustLevel.None)
                userToManage.TrustLevel = trustLevelToGive;
            else return;


            await ctx.Channel.SendMessageAsync($" { userToManage } trustlevel was set to {trustLevelToGive}");
            
            return;

        }  


        [Command("ViewTeam")]
        public async Task ViewTeam(CommandContext ctx, string teamName = "")
        {


            StandardLogging.LogInfo(FilePath, "ViewTeam command was used by " + ctx.User.ToString());
            StandardUserHandling.CheckIfRegistred(ctx);
            if (!ctx.User.CheckIfValid())
            {
                return;
            }

            if(teamName != "")
            {
                StandardLogging.LogInfo(FilePath, "ViewTeam command was used by " + ctx.User.ToString() + " with parameter:  " + teamName);
                await QuickCommands.quickViewTeam(ctx, teamName);
                return;
            }

            //Getting all the teams the user is in
            var teams = ctx.User.GetTeams();
            if (teams.Count == 0)
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " is not in any teams");
                await ctx.Channel.SendMessageAsync("You are not in any teams");
                return;
            }

            //Getting what team the user wants to view
            var response = await StandardUserInteraction.ChooseTeamAsync(ctx, teams);

            if(response.Success == false)
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " did not choose a team. Canceling ViewTeam");
                await ctx.Channel.SendMessageAsync("Canceled!");
                return;
            }

            Team team = response.ResponseItem;
            
            if (team is null)
                return;

            

            await ctx.Client.SendMessageAsync(ctx.Channel, team.ToDiscordString());
            return;
          
            
        }

        [Command("AvoidTeam")]
        public async Task AvoidTeam(CommandContext ctx)
        {
            StandardLogging.LogInfo(FilePath, "AvoidTeam command was used by " + ctx.User.ToString());
            StandardUserHandling.CheckIfRegistred(ctx);

            if (!ctx.User.CheckIfValid())
            {
                return;
            }

            //Getting all the teams the user is in where they can avoid teams
            var teams = ctx.User.GetTeamsWithTrustLevel(TrustLevel.CanEditTeam);

            if(teams is null)
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " is not allowed to avoid any teams");
                await ctx.Channel.SendMessageAsync("You do not have permission to avoid any teams in any of the teams you are in");
                return;
            }

            if(teams.Count < 1)
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " is not allowed to avoid any teams");
                await ctx.Channel.SendMessageAsync("You do not have permission to avoid any teams in any of the teams you are in");
                return;
            }

            //Getting what team the user wants to avoid FROM
            await ctx.Channel.SendMessageAsync("Which of your teams do you want to avoid a team with");
            var response = await StandardUserInteraction.ChooseTeamAsync(ctx, teams);

            if(response.Success == false)
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " did not choose a team. Canceling AvoidTeam");
                await ctx.Channel.SendMessageAsync("Canceled!");
                return;
            }

            Team team = response.ResponseItem;

            if (team is null)
            {
                StandardLogging.LogError(FilePath, "Team is null");
                return;
            }
                
            StandardLogging.LogDebug(FilePath, "Chosen team is " + team.TeamName + " with ID " + team.teamID);
            

            //Getting what team the user wants to avoid
            var response2  = await StandardUserInteraction.PromtStringAsync(ctx, "What team do you want to avoid?");

            if(response2.Success == false )
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " did not choose a team. Canceling AvoidTeam");
                await ctx.Channel.SendMessageAsync("Canceled!");
                return;
            }

            Team teamToAvoid = TeamHandler.GetTeamFromName(response2.ResponseItem);

            if(teamToAvoid is null)
            {
                StandardLogging.LogInfo(FilePath, "Could not find team with name " + response2.ResponseItem);
                await ctx.Channel.SendMessageAsync("Could not find team with name " + response2.ResponseItem);
                return;
            }

            StandardLogging.LogDebug(FilePath, "Chosen team to avoid is " + teamToAvoid.TeamName + " with ID " + teamToAvoid.teamID);
            team.AddAvoidedTeam(teamToAvoid);

            await ctx.Channel.SendMessageAsync("Team " + teamToAvoid.TeamName + " was added to the avoid list of team " + team.TeamName);
            return;

        }

        [Command("UnavoidTeam")]
        public async Task UnAvoidTeam(CommandContext ctx)
        {
            StandardLogging.LogInfo(FilePath, "UnAvoidTeam command was used by " + ctx.User.ToString());
            StandardUserHandling.CheckIfRegistred(ctx);

            if(!ctx.User.CheckIfValid())
            {
                return;
            }

            //Getting all the teams the user is in where they can avoid teams
            var teams = ctx.User.GetTeamsWithTrustLevel(TrustLevel.CanEditTeam);

            if(teams.Count < 1)
            {
                await ctx.Channel.SendMessageAsync("You do not have permission to avoid any teams in any of the teams you are in");
                return;
            }

            //Getting what team the user wants to avoid FROM
            var response = await StandardUserInteraction.ChooseTeamAsync(ctx, teams);

            if(response.Success == false)
            {
                await ctx.Channel.SendMessageAsync("Canceled!");
                return;
            }

            Team team = response.ResponseItem;

            if (team is null)
                return;

            //Getting what team the user wants to unAvoid

            var response2  = await StandardUserInteraction.ChooseTeamAsync(ctx, team.AvoidedTeams);

            if(response2.Success == false )
            {
                await ctx.Channel.SendMessageAsync("Canceled!");
                return;
            }

            Team teamToUnAvoid = response2.ResponseItem;


            if(teamToUnAvoid is null)
            {
                await ctx.Channel.SendMessageAsync("Canceled!");
                return;
            }

            team.RemoveAvoidedTeam(teamToUnAvoid);

            await ctx.Channel.SendMessageAsync("Team " + teamToUnAvoid.TeamName + " was removed from the avoid list of team " + team.TeamName);
            return;
        }

        [Command("ViewAvoidedTeams")]
        public async Task ViewAvoidedTeams(CommandContext ctx)
        {
            StandardLogging.LogInfo(FilePath, "ViewAvoidedTeams command was used by " + ctx.User.ToString());
            StandardUserHandling.CheckIfRegistred(ctx);

            if(!ctx.User.CheckIfValid())
            {
                return;
            }

            //Getting all the teams the user is in where they can avoid teams
            var teams = ctx.User.GetTeams();

            if(teams.Count < 1)
            {
                await ctx.Channel.SendMessageAsync("You are not in any teams");
                return;
            }

            //Getting what team the user wants to avoid FROM
            var response = await StandardUserInteraction.ChooseTeamAsync(ctx, teams);

            if(response.Success == false)
            {
                await ctx.Channel.SendMessageAsync("Canceled!");
                return;
            }

            Team team = response.ResponseItem;

            if (team is null)
                return;

            if(team.AvoidedTeams.Count < 1)
            {
                await ctx.Channel.SendMessageAsync("Team " + team.TeamName + " is not avoiding any teams");
                return;
            }

            string s = StandardStringBuilder.BuildTeamListString(team.AvoidedTeams);

            await ctx.Channel.SendMessageAsync("Team " + team.TeamName + " is avoiding the following teams: \n" + s);
            return;
        }

    }
}