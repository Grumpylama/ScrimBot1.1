namespace big
{
    public static class StandardUserInteraction
    {

        private static readonly string FilePath = "StandardUserInteraction.cs";
        
        
        public static async Task RespondAsync(CommandContext ctx, string message)
        {
            await ctx.RespondAsync(message);
        }

        public static async Task<Nullable<EDate>> PromtDateAsync(CommandContext ctx)
        {
            
            
            while(true)
            {
                await ctx.RespondAsync("What day will you be playing? \n 1:Tonight \n 2:Tomorrow \n 3:Other");
                var message = await ctx.Client.GetInteractivity().WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.Channel.Id == ctx.Channel.Id);
                if (message.Result.Content.ToLower().Contains("cancel"))
                {
                    return null;
                }
                if(message.Result.Content != "1" && message.Result.Content != "2" && message.Result.Content != "3")
                {
                    await ctx.RespondAsync("Please enter a valid number");
                    continue;
                }
                break;
            }
            while(true)
            {
                await ctx.RespondAsync("What time will you be playing? \n Please Enter in the format HH:MM \n Please matchmake at either xx:00 or xx:30 \n Enter \"ASAP\" if you are looking for a game ASAP");
                var message = await ctx.Client.GetInteractivity().WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.Channel.Id == ctx.Channel.Id);
                switch (message.Result.Content.ToLower())
                {
                    case "cancel":
                        return null;
                    case "asap":
                        return new EDate(GetNearestHour(DateTime.Now), true);
                    default:
                        if (DateTime.TryParse(message.Result.Content, out DateTime date))
                        {                   
                            return new EDate(GetNearestHour(date), false);
                        }
                        else
                        {
                            await ctx.RespondAsync("Please enter a valid time");
                            continue;
                        }
                        
                }
            }
            
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

        public static async Task<DiscordUser> ChooseUserAsync(CommandContext ctx, List<DiscordUser> users)
        {
            string s = StandardStringBuilder.BuildUserListString(users);
            await ctx.Client.SendMessageAsync(ctx.Channel, s);
            return await StandardInteractivityHandler.ChooseByNumber<DiscordUser>(ctx, users);
        }

        public static async Task<TeamUser> ChooseTeamUserAsync(CommandContext ctx, List<TeamUser> users)
        {
            string s = StandardStringBuilder.BuildTeamUserListString(users);
            await ctx.Client.SendMessageAsync(ctx.Channel, s);
            return await StandardInteractivityHandler.ChooseByNumber<TeamUser>(ctx, users);
        }

        public static async Task<TrustLevel> ChooseTrustLevelAsync(CommandContext ctx, TrustLevel maxTrustLevel)
        {
            List<TrustLevel> trustLevels = new List<TrustLevel>();
            foreach (TrustLevel trustLevel in Enum.GetValues(typeof(TrustLevel)))
            {
                if (trustLevel <= maxTrustLevel)
                {
                    if(trustLevel != TrustLevel.TeamCaptain)
                    trustLevels.Add(trustLevel);
                }
            }
            string s = StandardStringBuilder.BuildTrustLevelListString(trustLevels);
            await ctx.Client.SendMessageAsync(ctx.Channel, s);
            return await StandardInteractivityHandler.ChooseByNumber<TrustLevel>(ctx, trustLevels);
        }


        private static DateTime GetNearestHour(DateTime dt)
        {
            if(dt.Minute >= 30)           
                return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour + 1, 0, 0);       
            else        
                return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0);
        }

        
    }
}