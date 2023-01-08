namespace big
{
    public static class StandardInteractivityHandler
    {
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
                else
                {
                    await ctx.RespondAsync("Please enter a valid number");
                }

            }
        }
    }
}