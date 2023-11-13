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
                await ctx.RespondAsync("You are not an admin");
                return;
            }

            
            await ctx.RespondAsync("Test");
        }


    }
}