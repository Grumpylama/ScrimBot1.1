namespace big
{
    public static class StandardUserInteraction
    {
        
        
        public static async Task RespondAsync(CommandContext ctx, string message)
        {
            await ctx.RespondAsync(message);
        }

        public static async Task SendDMAsync(Dependecies d, DiscordUser user, string message)
        {
            var Channnel = await d.GetDMChannelAsync(user);
            await d.Client.SendMessageAsync(Channnel, message);
        }

        public static async Task<Team> ChooseTeamAsync(CommandContext ctx, List<Team> teams)
        {
            string s = StandardStringBuilder.BuildTeamListString(teams);
            await ctx.RespondAsync(s);
            return await StandardInteractivityHandler.ChooseByNumber<Team>(ctx, teams);            
        }

        


        
    }
}