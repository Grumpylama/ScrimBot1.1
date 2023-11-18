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
    }
}