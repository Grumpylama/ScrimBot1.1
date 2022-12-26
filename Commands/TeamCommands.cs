using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Interactivity.EventHandling;
using DSharpPlus.Interactivity.Enums;   
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Linq;




namespace big
{
    public class TeamCommands : BaseCommandModule
    {

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
            foreach (var user in Dependecies.Users)
            {
                if (user.Id == ctx.User.Id)
                {
                    Console.WriteLine("User is already registered. Canceling Register");
                    await ctx.Channel.SendMessageAsync("You are already registered!").ConfigureAwait(false);
                    return;
                }
            }

            Console.WriteLine("Registering user");
            Dependecies.Users.Add(ctx.User);           
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
            Team team = await ChooseTeam(ctx, teams);

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
                Team.Teams.Remove(team);
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
            foreach(Game game in Game.Games)
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
                    if(i > 0 && i <= Game.Games.Count)
                    {
                        Team.Teams.Add(new Team(Game.Games[i - 1], TeamName, ctx.User));
                        await ctx.RespondAsync("Team named " + TeamName + " was created for game " + Game.Games[i - 1].GameName);
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
           


            Dependecies.AddUserHash(hash, ctx.User, ctx.Channel);
            
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
            var ToAdd = await Dependecies.GetUserFromHash(hash);
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
            Team Team = await ChooseTeam(ctx, UsersTeams);
            if (ChooseTeam == null)
                return;

            //Check if user is already in team
            if (isInTeam(userToAdd, Team))
            {
                await ctx.RespondAsync("User is already in team");
                return;
            }
            
            Console.WriteLine("Adding user to team");


            Team.TeamMembers.Add(new TeamUser(userToAdd, Team.teamID, 0, "Member"));
            ctx.Client.SendMessageAsync(channel, "You were added to the team: " + Team.TeamName + " By " + ctx.User.Username + "#" + ctx.User.Discriminator).ConfigureAwait(false);
            await ctx.RespondAsync("User was added to team: " + Team.TeamName);
      
        }
        


        
        //Helper Methods
        private bool CheckIfValid(CommandContext ctx)
        {
            //Check if user is a bot
            if (ctx.User.IsBot)
            {
                Console.WriteLine("User is a bot. Canceling command");
                return false;
            }

            //Check if user is registered
            foreach (var user in Dependecies.Users)
            {
                if (user.Id == ctx.User.Id)
                {
                    Console.WriteLine("User is registered. Continuing command");
                    return true;
                }
            }

            Console.WriteLine("User is not registered. Canceling command");
            return false;
        }

        private List<Team> GetUsersTeams(DiscordUser user)
        {
            List<Team> UsersTeams = new List<Team>();
            foreach (Team team in Team.Teams)
            {
                if (team.TeamCaptain.Id == user.Id)
                {
                    UsersTeams.Add(team);
                }
            }
            
            if(UsersTeams.Count == 0)
            {
                Console.WriteLine("User is not a captain of any teams");
                return null;
            }

            return UsersTeams;
        }

        private async Task<Team> ChooseTeam(CommandContext ctx, List<Team> Teams)
        {
            string s = "Which team?";
            int i = 1;
            foreach (Team team in Teams)
            {
                s += "\n" + i + ": " + team.TeamName;
                i++;
            }

            while (true)
            {
                await ctx.RespondAsync(s);
                var message = await ctx.Client.GetInteractivity().WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.Channel.Id == ctx.Channel.Id);
                if (message.Result.Content.ToLower().Contains("cancel"))
                {
                    await ctx.RespondAsync("Canceled");
                    return null;
                }
                if (Int32.TryParse(message.Result.Content, out i))
                {
                    if (i > 0 && i <= Teams.Count)
                    {
                        return Teams[i - 1];
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

        private bool isInTeam(DiscordUser user, Team team)
        {
            foreach (TeamUser tu in team.TeamMembers)
            {
                if (tu.User.Id == user.Id)
                {
                    return true;
                }
            }

            return false;
        }
        
    }
        
}