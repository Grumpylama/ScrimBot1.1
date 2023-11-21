using System.Net.Security;
using Microsoft.VisualBasic;
using ScrimBot1._1.src.Classes.Types;

namespace big
{
    public static class StandardInteractivityHandler
    {

       

        public static async Task<bool> GetConfirmation(CommandContext ctx, string message)
        {
            await ctx.Channel.SendMessageAsync(message);
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
                    await ctx.Channel.SendMessageAsync("Please enter either \"confirm\" or \"no\"");
                }
            }
        }

        public static async Task<InteractionResponse<T>> ChooseByNumber<T>(CommandContext ctx, List<T> list)
        {
            try
            {
                while(true)
                {
                    var message = await ctx.Client.GetInteractivity().WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.Channel.Id == ctx.Channel.Id);
                    if (int.TryParse(message.Result.Content, out int i))
                    {
                        if (i > 0 && i <= list.Count)
                        {
                            return new InteractionResponse<T>(list[i - 1], InteractionOutcome.Success);
                        }
                        else
                        {
                            await ctx.Channel.SendMessageAsync("Please enter a valid number");
                        }
                    }
                    else if (message.Result.Content.ToLower() == "cancel")
                    {
                        return new InteractionResponse<T>(default(T), InteractionOutcome.Cancelled);                
                    }
                    else 
                    {
                        await ctx.Channel.SendMessageAsync("Please enter a valid number");
                    }

                }
            }
            catch
            {
                return new InteractionResponse<T>(default(T), InteractionOutcome.Error);
            }
            
        }


       
    }
}