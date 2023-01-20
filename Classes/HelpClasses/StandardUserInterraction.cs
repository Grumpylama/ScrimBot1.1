namespace big
{
    public static class StandardUserInteraction
    {
        
        
        public static async Task RespondAsync(CommandContext ctx, string message)
        {
            await ctx.RespondAsync(message);
        }

        public static async Task SendDMAsync(DiscordUser user, string message)
        {
            var Channnel = await DiscordInterface.GetDMChannelAsync(user);
            await DiscordInterface.Client.SendMessageAsync(Channnel, message);
        }

        public static async Task<Team> ChooseTeamAsync(CommandContext ctx, List<Team> teams)
        {
            string s = StandardStringBuilder.BuildTeamListString(teams);
            await ctx.RespondAsync(s);
            return await StandardInteractivityHandler.ChooseByNumber<Team>(ctx, teams);            
        }

        public static async Task<Game> ChooseGameAsync(CommandContext ctx, List<Game> games)
        {
            string s = StandardStringBuilder.BuildGamePromtString(games);
            await ctx.Client.SendMessageAsync(ctx.Channel, s);
            return await StandardInteractivityHandler.ChooseByNumber<Game>(ctx, games);
        }

        
    }
}