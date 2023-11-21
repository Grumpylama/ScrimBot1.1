using System.Threading.Channels;
using Microsoft.VisualBasic;
using ScrimBot1._1.src.Classes.Types;

namespace big
{
    public static class StandardUserInteraction
    {

        private static readonly string FilePath = "StandardUserInteraction.cs";
        
        
        

        

        public static async Task<InteractionResponse<string>> PromtStringAsync(CommandContext ctx, string promt)
        {
            try
            {
                await ctx.Channel.SendMessageAsync(promt);
                var message = await ctx.Client.GetInteractivity().WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.Channel.Id == ctx.Channel.Id);
                if (message.Result.Content.ToLower().Contains("cancel"))
                {
                    return new InteractionResponse<string>("", InteractionOutcome.Cancelled);
                }
                return new InteractionResponse<string>(message.Result.Content, InteractionOutcome.Success); 
            }
            catch
            {
                StandardLogging.LogError(FilePath, "Error getting string from user");
                return new InteractionResponse<string>("", InteractionOutcome.Error);
            }
        }

        
        public static async Task<InteractionResponse<DateTime>> PromtDateAsync(CommandContext ctx)
        {
            
            DateTime timeToPlay;
            while(true)
            {
                await ctx.Channel.SendMessageAsync("What day will you be playing? \n 1:Tonight \n 2:Tomorrow");
                var message = await ctx.Client.GetInteractivity().WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.Channel.Id == ctx.Channel.Id);
                if (message.Result.Content.ToLower().Contains("cancel"))
                {
                    return new InteractionResponse<DateTime>(DateTime.MinValue, InteractionOutcome.Cancelled);
                }
                if(message.Result.Content != "1" && message.Result.Content != "2")
                {
                    await ctx.Channel.SendMessageAsync("Please enter a valid number");
                    continue;
                }
                else
                {
                    switch (message.Result.Content)
                    {
                        case "1":
                            timeToPlay = DateTime.Today;
                            break;
                        case "2":
                            timeToPlay = DateTime.Today.AddDays(1);
                            break;
                        default:
                            await ctx.Channel.SendMessageAsync("Please enter a valid number");
                            continue;
                    }
                    break;
                }

                
            }

            while(true)
            {
                await ctx.Channel.SendMessageAsync("What time will you be playing? \n Please Enter in the format HH:MM \n Please matchmake at either xx:00 or xx:30");
                var message = await ctx.Client.GetInteractivity().WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.Channel.Id == ctx.Channel.Id);
                switch (message.Result.Content.ToLower())
                {
                    case "cancel":
                        return new InteractionResponse<DateTime>(DateTime.MinValue, InteractionOutcome.Cancelled);
                    default:
                        if (DateTime.TryParse(message.Result.Content, out DateTime date))
                        {                   
                            timeToPlay = new DateTime(timeToPlay.Year, timeToPlay.Month, timeToPlay.Day, date.Hour, date.Minute, 0);
                            return new InteractionResponse<DateTime>(timeToPlay, InteractionOutcome.Success);
                        }
                        else
                        {
                            await ctx.Channel.SendMessageAsync("Please enter a valid time");
                            continue;
                        }
                        
                }
            }
            
        }

        public static async Task SendDMAsync(DiscordUser user, string message)
        {
            DiscordChannel? channel = null;
            try
            {
                channel = DiscordInterface.GetDMChannelAsync(user);
            }
            catch
            {
                StandardLogging.LogError(FilePath, "Failed to get DM channel for user " + user.Username);
                throw new Exception("Failed to get DM channel for user " + user.Username);
            }
            
            if(channel is not null)
            await DiscordInterface.Client!.SendMessageAsync(channel, message);
            return;
        }

        public static async Task<InteractionResponse<Team>> ChooseTeamAsync(CommandContext ctx, List<Team> teams)
        {
            string s = StandardStringBuilder.BuildTeamListString(teams);
            await ctx.Channel.SendMessageAsync(s);

            return await StandardInteractivityHandler.ChooseByNumber<Team>(ctx, teams);
                     
        }

        public static async Task<InteractionResponse<Game>> ChooseGameAsync(CommandContext ctx, List<Game> games)
        {
            string s = StandardStringBuilder.BuildGamePromtString(games);
            await ctx.Client.SendMessageAsync(ctx.Channel, s);
            return await StandardInteractivityHandler.ChooseByNumber<Game>(ctx, games);
        }

        public static async Task<InteractionResponse<DiscordUser>> ChooseUserAsync(CommandContext ctx, List<DiscordUser> users)
        {
            string s = StandardStringBuilder.BuildUserListString(users);
            await ctx.Client.SendMessageAsync(ctx.Channel, s);
            return await StandardInteractivityHandler.ChooseByNumber<DiscordUser>(ctx, users);
        }

        public static async Task<InteractionResponse<TeamUser>> ChooseTeamUserAsync(CommandContext ctx, List<TeamUser> users)
        {
            string s = StandardStringBuilder.BuildTeamUserListString(users);
            await ctx.Client.SendMessageAsync(ctx.Channel, s);
            return await StandardInteractivityHandler.ChooseByNumber<TeamUser>(ctx, users);
        }

        public static async Task<InteractionResponse<TrustLevel>> ChooseTrustLevelAsync(CommandContext ctx, TrustLevel maxTrustLevel)
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