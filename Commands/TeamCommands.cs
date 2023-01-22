


namespace big
{
    public partial class Commands : BaseCommandModule
    {
        
        

        [Command("DeleteTeam")]
        public async Task DeleteTeam(CommandContext ctx)
        {
            Console.WriteLine("DeleteTeam command was used by " + ctx.User.ToString());
            UserHandler.CheckIfRegistred(ctx);
            if (!CheckIfValid(ctx))
            {
                return;
            }


            //Getting all teams that user is captain of
            List<Team> teams = ctx.User.GetOwnedTeams();
            if(teams == null)
            {
                Console.WriteLine("User has no teams. Canceling DeleteTeam");
                await ctx.Channel.SendMessageAsync("You have no teams!").ConfigureAwait(false);
                return;
            }


            //Chosing what team to delete
            Team team = await ChooseTeamAsync(ctx, teams);

            if(team == null)
            {
                Console.WriteLine("User did not choose a team. Canceling DeleteTeam");
                await ctx.Channel.SendMessageAsync("Canceled!").ConfigureAwait(false);
                return;
            } 


            //Confirming deletion
            await ctx.Channel.SendMessageAsync("Are you sure you want to delete " + team.TeamName + "? \n To confirm write \"CONFIRM\"").ConfigureAwait(false);
            var message = await ctx.Client.GetInteractivity().WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.Channel.Id == ctx.Channel.Id);

            if (await StandardInteractivityHandler.GetConfirmation(ctx, "Are you sure you want to delete your team: " + team.TeamName + " playing: " + team.game.GameName + "? \n To confirm write \"CONFIRM\"" ))
            {
                Console.WriteLine("User confirmed deletion of team");
                TeamHandler.Teams.Remove(team);
                await ctx.Channel.SendMessageAsync("Team deleted!").ConfigureAwait(false);
                return;
            }            


            //Canceling deletion        
            Console.WriteLine("User did not confirm deletion of team");
            await ctx.Channel.SendMessageAsync("Canceled!").ConfigureAwait(false);   
            return;         
            
        }

        [Command("CreateTeam")]
        public async Task CreateTeam(CommandContext ctx, string TeamName)
        {

            Console.WriteLine("CreateTeam command was used by " + ctx.User.ToString());
            UserHandler.CheckIfRegistred(ctx);
            if (!CheckIfValid(ctx))
            {  
                return;
            }

            if(TeamHandler.IsTeamNameTaken(TeamName))
            {
                Console.WriteLine("Team name is taken. Canceling CreateTeam");
                await ctx.Channel.SendMessageAsync("Team name is taken!").ConfigureAwait(false);
                return;
            }


            Game g = await StandardUserInteraction.ChooseGameAsync(ctx, GameHandler.Games);
            if(g == null)
            {
                Console.WriteLine("User did not choose a game. Canceling CreateTeam");
                await ctx.Channel.SendMessageAsync("Canceled!").ConfigureAwait(false);
                return;
            }              
            TeamHandler.Teams.Add(new Team(g , TeamName, ctx.User));
            await ctx.Client.SendMessageAsync(ctx.Channel, "Team named " + TeamName + " playing " + g.GameName + " created!").ConfigureAwait(false);
            
        }
       
        [Command("TransferCaptain")]
        public async Task TransferCaptain(CommandContext ctx)
        {
            
            Console.WriteLine("TransferCaptain command was used by " + ctx.User.ToString());
            UserHandler.CheckIfRegistred(ctx);
            if (!CheckIfValid(ctx))
            {
                
                return;
            }

            //Getting all teams that user is captain of
            List<Team> teams = ctx.User.GetOwnedTeams();
            if (teams == null)
            {
                Console.WriteLine("User has no teams. Canceling TransferCaptain");
                await ctx.Channel.SendMessageAsync("You have no teams!").ConfigureAwait(false);
                return;
            }

            Team TeamToTransfer = await ChooseTeamAsync(ctx, teams);

            if (TeamToTransfer == null)
            {
                Console.WriteLine("User did not choose a team. Canceling TransferCaptain");
                await ctx.Channel.SendMessageAsync("Canceled!").ConfigureAwait(false);
                return;
            }

            //Getting all users that are not captain of the team
            List<DiscordUser> otherMembers = TeamToTransfer.GetNonCaptainMembers().ForEach(item => item.GetUser());

            if(otherMembers.Count() == 0)
            {
                Console.WriteLine("User is the only member of the team. Canceling TransferCaptain");
                await ctx.Channel.SendMessageAsync("You are the only member of the team!").ConfigureAwait(false);
                return;
            }

            DiscordUser newCaptain = await ChooseUserAsync(ctx, otherMembers);

            if(newCaptain == null)
            {
                Console.WriteLine("User did not choose a user. Canceling TransferCaptain");
                await ctx.Channel.SendMessageAsync("Canceled!").ConfigureAwait(false);
                return;
            }

            TeamToTransfer.TeamCaptain = newCaptain;
            var t = ctx.Client.SendMessageAsync(DiscordInterface.DMChannel[newCaptain], "You are now the captain of " + TeamToTransfer.TeamName);
            await ctx.Channel.SendMessageAsync("Captain was transfered!").ConfigureAwait(false);
            await t;
            return;
        }

        [Command("JoinTeam")]
        public async Task JoinTeam(CommandContext ctx)
        {
            
            UserHandler.CheckIfRegistred(ctx);
            if (!CheckIfValid(ctx))
            {
                
                return;
            }
            Console.WriteLine("JoinTeam command was used by " + ctx.User.ToString());
            HashAlgorithm sha = SHA256.Create();

            string now = DateTime.Now.ToString("HH::mm::ss:ffffff");
            Console.WriteLine(now);
            
            string hash = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(now)));

            Console.WriteLine(hash);

            Random r = new Random();
            int i = r.Next(0, hash.Length - 10);
            hash = hash.Substring(i, 10);
          


            UserHandler.AddUserHash(hash, ctx.User, ctx.Channel);
            
            await ctx.RespondAsync("A captain can enter the following code to add you to a team: \n " + hash);
        }

        [Command("AddToTeam")]
        public async Task AddToTeam(CommandContext ctx, string hash)
        {
            Console.WriteLine("AddToTeam command was used by " + ctx.User.ToString());
            
            UserHandler.CheckIfRegistred(ctx);
            if(!CheckIfValid(ctx))
            {          

                return;
            }
            
            Console.WriteLine("User is valid trying to get user from provided hash");
            DiscordUser userToAdd = UserHandler.GetUserFromHashAsync(hash);
            
            

            if(userToAdd == null)
            {
                await ctx.RespondAsync("Could not find user with that hash or it has already been used. \n Please have the user try again");
                return;
            }

            Console.WriteLine("User to add is " + userToAdd.ToString());

            //Check if user is trying to add himself/herself to a team
            if(userToAdd.Id == ctx.User.Id)
            {
                await ctx.RespondAsync("You cannot add yourself to a team");
                return;
            }

            Console.WriteLine("User is not trying to add himself/herself to a team");

            List<Team> UsersTeams = ctx.User.GetOwnedTeams();

            Console.WriteLine("User has " + UsersTeams.Count + " teams");
            if(UsersTeams.Count == 0)
            {
                await ctx.RespondAsync("You are not a captain of any teams");
                return;
            }
            
            Console.WriteLine("Asking user which team to add to");
            
            Console.WriteLine("Getting team from user");
            Team Team = await ChooseTeamAsync(ctx, UsersTeams);
            if (Team == null)
                return;

            //Check if user is already in team
            if (userToAdd.IsInTeam(Team))
            {
                await ctx.RespondAsync("User is already in team");
                return;
            }
            
            Console.WriteLine("Adding user to team");


            Team.TeamMembers.Add(new TeamUser(userToAdd, Team.teamID, 0, "Member"));
            var t = userToAdd.SendDMAsync("You were added to the team: " + Team.TeamName + " By " + ctx.User.Username + "#" + ctx.User.Discriminator);
            await ctx.RespondAsync("User was added to team: " + Team.TeamName);
            if(!await t)
            {
                Console.WriteLine("Could not send DM to user");
                await ctx.Channel.SendMessageAsync("Could not send DM to, " + userToAdd.Username + "#" + userToAdd.Discriminator + " please make sure they have DMs enabled, and is a member of the server. They were still added to your team");
                return;

            }
            return;
        }
        
        [Command("LeaveTeam")]
        public async Task LeaveTeam(CommandContext ctx)
        {
            Console.WriteLine("LeaveTeam command was used by " + ctx.User.ToString());
            UserHandler.CheckIfRegistred(ctx);
            if (!CheckIfValid(ctx))
            {
                return;
            }
            
            //Getting all the teams the user is in
            var teams = ctx.User.GetTeams();
            if(teams.Count == 0)
            {
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
            

            //Removing user from team and notifying Captain
            team.TeamMembers.Remove(team.TeamMembers.Find(x => x.User.Id == ctx.User.Id));
            
            var t = team.TeamCaptain.SendDMAsync(ctx.User.Username + "#" + ctx.User.Discriminator + " has left the team: " + team.TeamName);
            await ctx.RespondAsync("You have left the team: " + team.TeamName);
            await t;
            return;
        }

        [Command("Kill")]
        public async Task Kill(CommandContext ctx, string password)
        {
            if(password != "SuperIdol")
            {
                await ctx.RespondAsync("Incorrect password");
                return;
            }
            Console.WriteLine("Kill command recceived. Killing application");
            big.FileManager.SaveAll();
            Environment.Exit(1);
        }

        [Command("Die")]
        public async Task Die(CommandContext ctx, string password)
        {
            if(password != "SuperIdol")
            {
                await ctx.RespondAsync("Incorrect password");
                return;
            }
            Console.WriteLine("Kill command recceived. Killing application");
            
            Environment.Exit(1);
        }

        
        //Helper Methods
        

    }
}