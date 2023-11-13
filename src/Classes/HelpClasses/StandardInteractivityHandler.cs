using System.Net.Security;

namespace big
{
    public static class StandardInteractivityHandler
    {

       

        public static async Task<bool> GetConfirmation(CommandContext ctx, string message)
        {
            await ctx.RespondAsync(message);
            while (true)
            {
                var m = await ctx.Client.GetInteractivity().WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.Channel.Id == ctx.Channel.Id);
                if (m.Result.Content.ToLower() == "confirm")
                {
                    return true;
                }
                else if (m.Result.Content.ToLower() == "no")
                {
                    return false;
                }
                else
                {
                    await ctx.RespondAsync("Please enter either \"confirm\" or \"no\"");
                }
            }
        }

        public static async Task<Tuple<bool, T>> ChooseByNumber<T>(CommandContext ctx, List<T> list)
        {
            while(true)
            {
                var message = await ctx.Client.GetInteractivity().WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.Channel.Id == ctx.Channel.Id);
                if (int.TryParse(message.Result.Content, out int i))
                {
                    if (i > 0 && i <= list.Count)
                    {
                        return new Tuple<bool, T>(false, list[i - 1]);
                    }
                    else
                    {
                        await ctx.RespondAsync("Please enter a valid number");
                    }
                }
                else if (message.Result.Content.ToLower() == "cancel")
                {
                    return new Tuple<bool, T>(true, list[0]);
                }
                else 
                {
                    await ctx.RespondAsync("Please enter a valid number");
                }

            }
        }


       
    }
}