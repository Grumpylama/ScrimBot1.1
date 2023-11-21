using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Interactivity.EventHandling;
using DSharpPlus.Interactivity.Enums;   



namespace big
{
    

    public partial class MatchmakingCommands : BaseCommandModule
    {        

        private static readonly string FilePath = "MatchmakingCommands.cs";
        [Command("matchmake")]
        public async Task Matchmake(CommandContext ctx)
        {
            
            await ctx.RespondAsync("Matchmaking is currently disabled");
            
            
            var teams = ctx.User.GetTeamsWithTrustLevel(TrustLevel.CanMatchMake);

            if (teams.Count == 0)
            {
                await ctx.RespondAsync("You don't have the right to matchmake for any teams");
                return;
            }

            await ctx.RespondAsync("Which team would you like to matchmake for?");
            var team = await StandardUserInteraction.ChooseTeamAsync(ctx, teams);

            if(team.Success == false)
            {
                await ctx.RespondAsync("Matchmaking cancelled");
                return;
            }

            
            var time = await StandardUserInteraction.PromtDateAsync(ctx);

            if(time.Success == false)
            {
                await ctx.RespondAsync("Matchmaking cancelled");
                return;
            }

            
            if(MatchMakingSystem.AddTeamToMatchMaking(team.ResponseItem, (DateTime)time.ResponseItem, ctx.User))
            {
                await ctx.RespondAsync("Team added to matchmaking");
                return;
            }
            else
            {
                await ctx.RespondAsync("Error adding team to matchmaking");
                return;
            }
            


            //d.MatchMakerHandler.addMatchMakingTeam(new MatchMakingTeam());

        }

        [Command("viewTickets")]
        public async Task ViewTickets(CommandContext ctx)
        {
            StandardLogging.LogInfo(FilePath, ctx.User + " is viewing their matchmaking tickets");

            StandardUserHandling.CheckIfRegistred(ctx);

            if(!ctx.User.CheckIfValid())
            {
                return;
            }

            var teams = ctx.User.GetTeamsWithTrustLevel(TrustLevel.CanMatchMake);

            if (teams.Count == 0)
            {
                await ctx.RespondAsync("You don't have the right to matchmake for any teams");
                return;
            }

            await ctx.RespondAsync("Which team would you like to view tickets for?");
            var team = await StandardUserInteraction.ChooseTeamAsync(ctx, teams);

            if(team.Success == false)
            {
                await ctx.RespondAsync("Viewing tickets cancelled");
                return;
            }

            var dateTimes = MatchMakingSystem.GetDateTimes(team.ResponseItem);
            if(dateTimes.Count == 0)
            {
                await ctx.RespondAsync("No tickets found");
                return;
            }

            




        }
    }
}