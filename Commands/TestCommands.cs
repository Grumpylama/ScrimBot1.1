

public class TestCommands : BaseCommandModule
{

    Random random = new Random();

    [Command("ping")]
    public async Task Ping(CommandContext ctx)
    {
        Console.WriteLine("Ping command was used by " + ctx.User.ToString());
        await ctx.Channel.SendMessageAsync("Pong").ConfigureAwait(false);
    }

    [Command("gaytest")]
    public async Task Gaytest(CommandContext ctx)
    {       
        Console.WriteLine("Gaytest command was used by " + ctx.User.ToString());
        if(random.Next(0,2) == 1) await ctx.Channel.SendMessageAsync("You are gay").ConfigureAwait(false);
        else await ctx.Channel.SendMessageAsync("You are not gay").ConfigureAwait(false);       
    }
    [Command("boobs")]
    public async Task Boobs(CommandContext ctx)
    {
        Console.WriteLine("boobs command was ued by " + ctx.User.ToString());
        await ctx.Channel.SendMessageAsync("https://google.com/search?q=boobs").ConfigureAwait(false);
    }
    [Command("add")]
    public async Task Add(CommandContext ctx, int n1, int n2) 
    {
        Console.WriteLine("add command was used by" + ctx.User.ToString());
        await ctx.Channel.SendMessageAsync((n1 + n2).ToString()).ConfigureAwait(false);       
    }
    [Command("Stress")]
    public async Task Stress(CommandContext ctx)
    {
        Console.WriteLine("Stressing Comemces");
        for(int i = 0; i < 100000; i++)
        {
            for(int j = 0; j < 10000; j++)
            {
                
                
                Console.WriteLine(i + j);
                
            }
            
        }

        Console.WriteLine("Stressing Complete");
    }
    
}