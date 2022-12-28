using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Interactivity.EventHandling;
using DSharpPlus.Interactivity.Enums;   



namespace big
{
    public partial class Commands : BaseCommandModule
    {        

        [Command("matchmake")]
        public async Task Matchmake(CommandContext ctx)
        {
            
            
            Team teamTomatchMake = await ChooseTeam(ctx, GetMemberTeams(ctx.User));
            

        }
    }
}