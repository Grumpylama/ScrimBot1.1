


namespace big
{
    public partial class Commands : BaseCommandModule
    {
        public static Dependecies d;

        

        [Command("Register")]
        public async Task Register(CommandContext ctx)
        {
            Console.WriteLine("Register command was used by " + ctx.User.ToString());


            //Check if user is a bot
            if(ctx.User.IsBot)
            {
                Console.WriteLine("Regestering user is a bot. Canceling Register");
                await ctx.Channel.SendMessageAsync("You are a bot!").ConfigureAwait(false);
                return;
            }

            //Check if user is already registered
            foreach (var user in d.Users)
            {
                if (user.Id == ctx.User.Id)
                {
                    Console.WriteLine("User is already registered. Canceling Register");
                    await ctx.Channel.SendMessageAsync("You are already registered!").ConfigureAwait(false);
                    return;
                }
            }

            Console.WriteLine("Registering user");
            
            d.Users.Add(ctx.User);   
            d.DMChannel.Add(ctx.User, ctx.Channel);       
            await ctx.Channel.SendMessageAsync("You are now registered!").ConfigureAwait(false);           
            
        }


        [Command("DeleteTeam")]
        public async Task DeleteTeam(CommandContext ctx)
        {
            Console.WriteLine("DeleteTeam command was used by " + ctx.User.ToString());
            if (!CheckIfValid(ctx))
            {
                Console.WriteLine("User is not valid. Canceling DeleteTeam");
                await ctx.Channel.SendMessageAsync("Could not Delete Team! Are you registred? \n Try again or register using !register").ConfigureAwait(false);
                return;
            }


            //Getting all teams that user is captain of
            List<Team> teams = GetUsersTeams(ctx.User);
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

            if (message.Result.Content == "CONFIRM")
            {
                Console.WriteLine("User confirmed deletion of team");
                d.Teams.Remove(team);
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
            if (!CheckIfValid(ctx))
            {
                Console.WriteLine("User is not valid. Canceling CreateTeam");
                await ctx.Channel.SendMessageAsync("Could not Create Team! Are you registred? \n Try again or register using !register").ConfigureAwait(false);
                return;
            }
                                      
            string s = "What game will you be playing?";
            int i = 1;
            foreach(Game game in d.Games)
            {
                s += "\n" + i + ": " + game.GameName;
                i++;
            }
            await ctx.RespondAsync(s);

            
            while(true)
            {
                var message = await ctx.Client.GetInteractivity().WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.Channel.Id == ctx.Channel.Id);            
                if(Int32.TryParse(message.Result.Content, out i))
                {
                    if(i > 0 && i <= d.Games.Count)
                    {
                        d.Teams.Add(new Team(d.Games[i - 1], TeamName, ctx.User));
                        await ctx.RespondAsync("Team named " + TeamName + " was created for game " + d.Games[i - 1].GameName);
                        return;
                    }
                    else
                    {
                        await ctx.RespondAsync("Please enter a valid number");
                    }
                }
                else
                {
                    await ctx.RespondAsync("Please enter a valid number");
                }
            
            }
            
        }
       
        [Command("TransferCaptain")]
        public async Task TransferCaptain(CommandContext ctx)
        {
            
            Console.WriteLine("TransferCaptain command was used by " + ctx.User.ToString());
            if (!CheckIfValid(ctx))
            {
                Console.WriteLine("User is not valid. Canceling TransferCaptain");
                await ctx.Channel.SendMessageAsync("Could not Transfer Captain! Are you registred? \n Try again or register using !register").ConfigureAwait(false);
                return;
            }

            //Getting all teams that user is captain of
            List<Team> teams = GetUsersTeams(ctx.User);
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
            List<DiscordUser> otherMembers = GetUsersNotCaptain(TeamToTransfer);

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
            var t = ctx.Client.SendMessageAsync(d.DMChannel[newCaptain], "You are now the captain of " + TeamToTransfer.TeamName);
            await ctx.Channel.SendMessageAsync("Captain was transfered!").ConfigureAwait(false);
            await t;
            return;
        }

        [Command("JoinTeam")]
        public async Task JoinTeam(CommandContext ctx)
        {

            if (!CheckIfValid(ctx))
            {
                Console.WriteLine("User is not valid. Canceling JoinTeam");
                await ctx.Channel.SendMessageAsync("Could not Join Team! Are you registred? \n Try again or register using !register").ConfigureAwait(false);
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
           


            d.AddUserHash(hash, ctx.User, ctx.Channel);
            
            await ctx.RespondAsync("A captain can enter the following code to add you to a team: \n " + hash);
        }

        [Command("AddToTeam")]
        public async Task AddToTeam(CommandContext ctx, string hash)
        {
            Console.WriteLine("AddToTeam command was used by " + ctx.User.ToString());
            
            if(!CheckIfValid(ctx))
            {
                Console.WriteLine("User is not valid. Canceling AddToTeam");
                await ctx.Channel.SendMessageAsync("Could not Add to Team! Are you registred? \n Try again or register using !register")
                .ConfigureAwait(false);
                return;
            }
            
            Console.WriteLine("User is valid trying to get user from provided hash");
            var ToAdd = await d.GetUserFromHashAsync(hash);
            DiscordUser userToAdd = ToAdd.Item1;
            DiscordChannel channel = ToAdd.Item2;

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

            List<Team> UsersTeams = GetUsersTeams(ctx.User);

            Console.WriteLine("User has " + UsersTeams.Count + " teams");
            if(UsersTeams.Count == 0)
            {
                await ctx.RespondAsync("You are not a captain of any teams");
                return;
            }
            
            Console.WriteLine("Asking user which team to add to");
            string s = "Which team would you like to add " + userToAdd.Username + " to?";
            int i = 1;
            foreach(Team team in UsersTeams)
            {
                s += "\n" + i + ": " + team.TeamName;
                i++;
            }

            Console.WriteLine("Getting team from user");
            Team Team = await ChooseTeamAsync(ctx, UsersTeams);
            if (ChooseTeamAsync == null)
                return;

            //Check if user is already in team
            if (isInTeam(userToAdd, Team))
            {
                await ctx.RespondAsync("User is already in team");
                return;
            }
            
            Console.WriteLine("Adding user to team");


            Team.TeamMembers.Add(new TeamUser(userToAdd, Team.teamID, 0, "Member"));
            var t = ctx.Client.SendMessageAsync(channel, "You were added to the team: " + Team.TeamName + " By " + ctx.User.Username + "#" + ctx.User.Discriminator).ConfigureAwait(false);
            await ctx.RespondAsync("User was added to team: " + Team.TeamName);
            await t;
      
        }
        
        [Command("LeaveTeam")]
        public async Task LeaveTeam(CommandContext ctx)
        {
            Console.WriteLine("LeaveTeam command was used by " + ctx.User.ToString());
            if (!CheckIfValid(ctx))
            {
                Console.WriteLine("User is not valid. Canceling LeaveTeam");
                await ctx.Channel.SendMessageAsync("Could not Leave Team! Are you registred? \n Try again or register using !register").ConfigureAwait(false);
                return;
            }

            //Getting all the teams the user is in
            var teams = GetMemberTeams(ctx.User);
            if(teams.Count == 0)
            {
                await ctx.RespondAsync("You are not in any teams");
                return;
            }

            //Getting what team the user wants to leave
            Team team = await ChooseTeamAsync(ctx, teams);
            if (team == null)
                return;
            if(team.TeamCaptain.Id == ctx.User.Id)
            {
                await ctx.RespondAsync("You are the captain of this team. You cannot leave the team. \n If you want to leave the team you must transfer the captain role to another member of the team or delete the team");
                return;
            }

            //Getting confirmation from user
            await ctx.RespondAsync("Are you sure you want to leave the team: " + team.TeamName + "?" + "\n Type \"CONFIRM\" to leave the team");
            var message = await ctx.Client.GetInteractivity().WaitForMessageAsync(x => x.Author.Id == ctx.User.Id);
            if(message.Result.Content != "CONFIRM")
            {
                await ctx.RespondAsync("You did not type \"CONFIRM\". Cancelling LeaveTeam");
                return;
            }

            //Removing user from team and notifying Captain
            team.TeamMembers.Remove(team.TeamMembers.Find(x => x.User.Id == ctx.User.Id));
            var t = ctx.Client.SendMessageAsync(d.DMChannel[team.TeamCaptain], ctx.User.Username + "#" + ctx.User.Discriminator + " has left the team: " + team.TeamName);
            await ctx.RespondAsync("You have left the team: " + team.TeamName);
            await t;
            return;
        }

        [Command("Kill")]
        public async Task Kill(CommandContext ctx)
        {
            Console.WriteLine("Kill command recceived. Killing application");
            big.FileManager.SaveAll(d);
            Environment.Exit(1);
        }

        
        //Helper Methods
        

    }
}