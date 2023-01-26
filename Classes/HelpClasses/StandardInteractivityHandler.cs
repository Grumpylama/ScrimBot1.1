namespace big
{
    public static class StandardInteractivityHandler
    {

        private static readonly string FilePath = "StandardInteractivityHandler.cs";
       

        public static async Task<bool> GetConfirmation(CommandContext ctx, string message)
        {
            await ctx.RespondAsync(message);
            while (true)
            {
                var m = await ctx.Client.GetInteractivity().WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.Channel.Id == ctx.Channel.Id);
                if (m.Result.Content.ToLower() == "CONFIRM")
                {
                    return true;
                }
                else if (m.Result.Content.ToLower() == "no")
                {
                    return false;
                }
                else
                {
                    await ctx.RespondAsync("Please enter yes or no");
                }
            }
        }

        public static async Task<T> ChooseByNumber<T>(CommandContext ctx, List<T> l)
        {
            while(true)
            {
                var message = await ctx.Client.GetInteractivity().WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.Channel.Id == ctx.Channel.Id);
                if (int.TryParse(message.Result.Content, out int i))
                {
                    if (i > 0 && i <= l.Count)
                    {
                        return l[i - 1];
                    }
                    else
                    {
                        await ctx.RespondAsync("Please enter a valid number");
                    }
                }
                else if (message.Result.Content.ToLower() == "cancel")
                {
                    return default(T);
                }
                else 
                {
                    await ctx.RespondAsync("Please enter a valid number");
                }

            }
        }


       
    }
}