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
            foreach (var user in User.Users)
            {
                if (user.DiscordID == ctx.User.Id)
                {
                    Console.WriteLine("User is already registered. Canceling Register");
                    await ctx.Channel.SendMessageAsync("You are already registered!").ConfigureAwait(false);
                    return;
                }
            }

            Console.WriteLine("Registering user");
            User.Users.Add(new User(ctx.User.Id));           
            await ctx.Channel.SendMessageAsync("You are now registered!").ConfigureAwait(false);           
            
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
            
            
            
            Bot.Conversations.Add(new Conversation(ctx.User.Id, ctx.Channel));
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
                        Team.Teams.Add(new Team(Game.Games[i - 1], TeamName, ctx.User.Id));
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

        



        private bool CheckIfValid(CommandContext ctx)
        {
            //Check if user is a bot
            if (ctx.User.IsBot)
            {
                Console.WriteLine("User is a bot. Canceling command");
                return false;
            }

            //Check if user is registered
            foreach (var user in User.Users)
            {
                if (user.DiscordID == ctx.User.Id)
                {
                    Console.WriteLine("User is registered. Continuing command");
                    return true;
                }
            }

            Console.WriteLine("User is not registered. Canceling command");
            return false;
        }
    }
}