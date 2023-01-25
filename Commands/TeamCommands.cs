


namespace big
{
    public partial class Commands : BaseCommandModule
    {
        
        private static readonly string FilePath = "Commands.cs";

        [Command("DeleteTeam")]
        public async Task DeleteTeam(CommandContext ctx)
        {
            
            StandardLogging.LogInfo(FilePath, "DeleteTeam command was used by " + ctx.User.ToString());
            UserHandler.CheckIfRegistred(ctx);
            if (!CheckIfValid(ctx))
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
            Team team = await ChooseTeamAsync(ctx, teams);

            if(team == null)
            {
                
                StandardLogging.LogInfo(FilePath, "User did not choose a team. Canceling DeleteTeam");
                await ctx.Channel.SendMessageAsync("Canceled!").ConfigureAwait(false);
                return;
            } 


            //Confirming deletion
            await ctx.Channel.SendMessageAsync("Are you sure you want to delete " + team.TeamName + "? \n To confirm write \"CONFIRM\"").ConfigureAwait(false);
            var message = await ctx.Client.GetInteractivity().WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.Channel.Id == ctx.Channel.Id);

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
            UserHandler.CheckIfRegistred(ctx);
            if (!CheckIfValid(ctx))
            {  
                return;
            }

            if(TeamHandler.IsTeamNameTaken(TeamName))
            {
                
                StandardLogging.LogInfo(FilePath, "Team name is taken. Canceling CreateTeam");
                await ctx.Channel.SendMessageAsync("Team name is taken!").ConfigureAwait(false);
                return;
            }


            Game g = await StandardUserInteraction.ChooseGameAsync(ctx, GameHandler.Games);
            if(g == null)
            {

                StandardLogging.LogInfo(FilePath, "User did not choose a game. Canceling CreateTeam");
                await ctx.Channel.SendMessageAsync("Canceled!").ConfigureAwait(false);
                return;
            }              
            var t = new Team(g, TeamName, ctx.User);
            StandardLogging.LogInfo(FilePath, "Team " + t.TeamName + " created by " + ctx.User.ToString() + " playing " + t.game.GameName);
            TeamHandler.Teams.Add(t);
            await ctx.Client.SendMessageAsync(ctx.Channel, "Team named " + TeamName + " playing " + g.GameName + " created!").ConfigureAwait(false);
            
        }
       
        [Command("TransferCaptain")]
        public async Task TransferCaptain(CommandContext ctx)
        {
            
            StandardLogging.LogInfo(FilePath, "TransferCaptain command was used by " + ctx.User.ToString());
            UserHandler.CheckIfRegistred(ctx);
            if (!CheckIfValid(ctx))
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " is a bot Canceling TransferCaptain");
                return;
            }

            //Getting all teams that user is captain of

            List<Team> teams = ctx.User.GetOwnedTeams();
            if (teams == null)
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " has no teams. Canceling TransferCaptain");
                await ctx.Channel.SendMessageAsync("You have no teams!").ConfigureAwait(false);
                return;
            }

            Team TeamToTransfer = await StandardUserInteraction.ChooseTeamAsync(ctx, teams);

            if (TeamToTransfer == null)
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " did not choose a team. Canceling TransferCaptain");
                await ctx.Channel.SendMessageAsync("Canceled!").ConfigureAwait(false);
                return;
            }

            //Getting all users that are not captain of the team
            var otherMembers = TeamToTransfer.GetNonCaptainMembers().Select(x => x.User).ToList();
            

            if(otherMembers.Count() == 0)
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " is the only member of the team. Canceling TransferCaptain");
                
                await ctx.Channel.SendMessageAsync("You are the only member of the team!").ConfigureAwait(false);
                return;
            }


            DiscordUser newCaptain = await ChooseUserAsync(ctx, otherMembers);

            StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " chose " + newCaptain.ToString() + " as new captain of " + TeamToTransfer.TeamName);

            #pragma warning disable CS8625
            if(newCaptain == null)
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " did not choose a user. Canceling TransferCaptain");
                
                await ctx.Channel.SendMessageAsync("Canceled!").ConfigureAwait(false);
                return;
            }

            TeamToTransfer.TeamCaptain = newCaptain;
            StandardLogging.LogInfo(FilePath, $"User {ctx.User.ToString()} transfered captainship of {TeamToTransfer} to {TeamToTransfer.TeamCaptain.ToString()}");
            var t = newCaptain.SendDMAsync("You are now the captain of " + TeamToTransfer);    
            await ctx.Channel.SendMessageAsync($"Captain was transfered!  {TeamToTransfer.TeamCaptain.ToString() } is now the new captain of {TeamToTransfer.TeamName}").ConfigureAwait(false);
            await t;
            return;
        }

        [Command("JoinTeam")]
        public async Task JoinTeam(CommandContext ctx)
        {
            
            StandardLogging.LogInfo(FilePath, "JoinTeam command was used by " + ctx.User.ToString());
            UserHandler.CheckIfRegistred(ctx);
            if (!CheckIfValid(ctx))
            {
                
                return;
            }
            
            StandardLogging.LogInfo(FilePath, "JoinTeam command was used by " + ctx.User.ToString());
            HashAlgorithm sha = SHA256.Create();

            string now = DateTime.Now.ToString("HH::mm::ss:ffffff");
            
            
            string hash = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(now)));


            StandardLogging.LogInfo(FilePath, "Hash generated for " + ctx.User.ToString() + " is " + hash);

            Random r = new Random();
            int i = r.Next(0, hash.Length - 10);
            hash = hash.Substring(i, 10);

            StandardLogging.LogInfo(FilePath, "Final hash generated for " + ctx.User.ToString() + " is " + hash);
          


            UserHandler.AddUserHash(hash, ctx.User, ctx.Channel);
            
            await ctx.RespondAsync("A captain can enter the following code to add you to a team: \n " + hash);
        }

        [Command("AddToTeam")]
        public async Task AddToTeam(CommandContext ctx, string hash)
        {
            StandardLogging.LogInfo(FilePath, "AddToTeam command was used by " + ctx.User.ToString());
            
            UserHandler.CheckIfRegistred(ctx);
            if(!CheckIfValid(ctx))
            {          

                return;
            }
            
            StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " is trying to add a user with hash " + hash);
            DiscordUser userToAdd = UserHandler.GetUserFromHashAsync(hash);
            
            

            if(userToAdd == null)
            {
                StandardLogging.LogInfo(FilePath, "Could not find a user with hash " + hash);
                await ctx.RespondAsync("Could not find user with that hash or it has already been used. \n Please have the user try again");
                return;
            }

            


            //Check if user is trying to add himself/herself to a team
            if(userToAdd.Id == ctx.User.Id)
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " is trying to add himself/herself to a team");
                await ctx.RespondAsync("You cannot add yourself to a team");
                return;
            }

            
            StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " is trying to add " + userToAdd.ToString() + " to a team");
            List<Team> UsersTeams = ctx.User.GetOwnedTeams();

            
            if(UsersTeams.Count == 0)
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " is not a captain of any teams");
                await ctx.RespondAsync("You are not a captain of any teams");
                return;
            }
            
            StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " is a captain of " + UsersTeams.Count + " teams");
            
            
            Team Team = await ChooseTeamAsync(ctx, UsersTeams);
            if (Team == null)
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " did not choose a team. Canceling AddToTeam");
                return;
            }
                

            //Check if user is already in team
            if (userToAdd.IsInTeam(Team))
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " is already in team " + Team);
                await ctx.RespondAsync("User is already in team");
                return;
            }
            
           
            StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " is adding " + userToAdd.ToString() + " to team " + Team);

            Team.TeamMembers.Add(new TeamUser(userToAdd, Team.teamID, 0, "Member"));
            var t = userToAdd.SendDMAsync("You were added to the team: " + Team.TeamName + " By " + ctx.User.Username + "#" + ctx.User.Discriminator);
            await ctx.RespondAsync("User was added to team: " + Team.TeamName);
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
            
            StandardLogging.LogInfo(FilePath, "LeaveTeam command was used by " + ctx.User.ToString());
            UserHandler.CheckIfRegistred(ctx);
            if (!CheckIfValid(ctx))
            {
                return;
            }
            
            //Getting all the teams the user is in
            var teams = ctx.User.GetTeams();
            if(teams.Count == 0)
            {
                StandardLogging.LogInfo(FilePath, "User " + ctx.User.ToString() + " is not in any teams");
                await ctx.RespondAsync("You are not in any teams");
                return;
            }


            //Getting what team the user wants to leave
            Team team = await StandardUserInteraction.ChooseTeamAsync(ctx, teams);
            if (team == null)
                return;

            if(team.TeamCaptain.Id == ctx.User.Id)
            {
                await ctx.RespondAsync("You are the captain of this team. You cannot leave the team. \n If you want to leave the team you must transfer the captain role to another member of the team or delete the team");
                return;
            }

            //Getting confirmation from user
            if(!await StandardInteractivityHandler.GetConfirmation(ctx, StandardStringBuilder.BuildTeamConfirmationString(team, "Leave")))           
            return;
            
            #pragma warning disable CS8604
            //Removing user from team and notifying Captain
            team.TeamMembers.Remove(team.TeamMembers.Find(x => x.User.Id == ctx.User.Id));
            
            var t = team.TeamCaptain.SendDMAsync(ctx.User.Username + "#" + ctx.User.Discriminator + " has left the team: " + team.TeamName);
            await ctx.RespondAsync("You have left the team: " + team.TeamName);
            await t;
            return;
        }

        

        

        
        //Helper Methods
        

    }
}