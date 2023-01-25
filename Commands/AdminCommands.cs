

namespace big
{

    //[Group("admin")]
    //[Description("Admin commands")]
    //[Hidden]

    public class AdminCommands : BaseCommandModule
    {
        private static readonly string FilePath = "AdminCommands.cs";


        [Command("shutdown")]
        [Description("Shuts down the bot")]
        [Aliases("exit" , "stop")]
        public async Task Shutdown(CommandContext ctx)
        {
            StandardLogging.LogInfo(FilePath, "Shutdown used by " + ctx.User);
            if( ctx.User.IsAdmin() == false)
            {
                await ctx.RespondAsync("You are not an admin");
                return;
            }
            StandardLogging.LogInfo(FilePath, "Shutting down bot");
            await ctx.RespondAsync("Shutting down bot");
            await ctx.Client.DisconnectAsync();
            Environment.Exit(0);
        }

        [Command("save")]
        [Description("Saves all data")]
        public async Task Save(CommandContext ctx)
        {
            StandardLogging.LogInfo(FilePath, "Save used by " + ctx.User);
            if( ctx.User.IsAdmin() == false)
            {
                await ctx.RespondAsync("You are not an admin");
                return;
            }
            StandardLogging.LogInfo(FilePath, "Saving all data");
            await ctx.RespondAsync("Saving all data");
            FileManager.SaveAll();
        }

        [Command("CheckIfMatchmaking")]
        [Description("Checks if the user is in matchmaking")]
        public async Task CheckIfMatchmaking(CommandContext ctx, DiscordUser user)
        {
            StandardLogging.LogInfo(FilePath, "CheckIfMatchmaking used by " + ctx.User);
            if( ctx.User.IsAdmin() == false)
            {
                await ctx.RespondAsync("You are not an admin");
                return;
            }

            throw new NotImplementedException();
            
        }

        [Command("VarDump")]
        [Description("Dumps all variables")]
        public async Task VarDump(CommandContext ctx)
        {
            StandardLogging.LogInfo(FilePath, "VarDump used by " + ctx.User);
            if( ctx.User.IsAdmin() == false)
            {
                await ctx.RespondAsync("You are not an admin");
                return;
            }

            TeamHandler.VarDump();
            DiscordInterface.VarDump();
            GameHandler.VarDump();
            StandardLogging.LogInfo(FilePath, "VarDump done");
            await ctx.RespondAsync("VarDump done");

            
        }
    }
}

