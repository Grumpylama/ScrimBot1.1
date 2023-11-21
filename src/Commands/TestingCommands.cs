namespace big
{
    
    public class TestingCommands : BaseCommandModule
    {
        [Command("test")]
        [Description("Test")]
        [Hidden]
        public async Task Test(CommandContext ctx)
        {
            if(ctx.User.IsAdmin() == false)
            {
                await ctx.Channel.SendMessageAsync("You are not an admin");
                return;
            }

            
            await ctx.Channel.SendMessageAsync("Test");
        }


    }
}