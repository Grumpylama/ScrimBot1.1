using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Interactivity.EventHandling;
using DSharpPlus.Interactivity.Enums;   



namespace big
{
    [Group("matchmaking")]
    [Description("Matchmaking commands")]

    public partial class MatchmakingCommands : BaseCommandModule
    {        

        [Command("matchmake")]
        public async Task Matchmake(CommandContext ctx)
        {
            
            
            throw new NotImplementedException();
            

            //d.MatchMakerHandler.addMatchMakingTeam(new MatchMakingTeam());

        }
    }
}