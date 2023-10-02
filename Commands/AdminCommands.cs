using DSharpPlus.CommandsNext.Converters;

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
        [Hidden]
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
        [Aliases("saveall", "ForceSave")]
        [Hidden]
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
        [Hidden]
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
        [Hidden]
        public async Task VarDump(CommandContext ctx)
        {
            StandardLogging.LogInfo(FilePath, "VarDump used by " + ctx.User);
            if( ctx.User.IsAdmin() == false)
            {
                await ctx.RespondAsync("You are not an admin");
                return;
            }

            StandardLogging.VarDump();
            await ctx.RespondAsync("VarDump done");

        }

        [Command("Sudo")]
        [Description("Runs a command as another user")]
        [Hidden]
        public async Task Sudo(CommandContext ctx, [Description("Member to execute as.")] ulong memberID, [Description("Command text to execute.")] string command, [RemainingText] string args)
        {
            StandardLogging.LogInfo(FilePath, "Sudo used by " + ctx.User);

            if( ctx.User.IsAdmin() == false)
            {
                await ctx.RespondAsync("You are not an admin");
                StandardLogging.LogInfo(FilePath, "User " + ctx.User + " is not an admin caneling Sudo");
                return;
            }
            // note the [RemainingText] attribute on the argument.
            // it will capture all the text passed to the command
            
            // let's trigger a typing indicator to let
            // users know we're working
            await ctx.TriggerTypingAsync();

            var member = StandardUserHandling.GetUserFromID(memberID);

            if(member is null)
            {
                await ctx.RespondAsync("Member not found");
                StandardLogging.LogInfo(FilePath, "Sudo used by " + ctx.User + " on " + member + " not found");
                return;
            }
            
            StandardLogging.LogInfo(FilePath, "Sudo used by " + ctx.User + " on " + member);

            // get the command service, we need this for
            // sudo purposes
            var cmds = ctx.CommandsNext;

            // retrieve the command and its arguments from the given string
            var cmd = cmds.FindCommand(command, out var customArgs);

            StandardLogging.LogInfo(FilePath, "Customargs are " + customArgs);
            if(cmd == null)
            {
                await ctx.RespondAsync("Command not found");
                StandardLogging.LogInfo(FilePath, "Sudo used by " + ctx.User + " on " + member + " with command " + cmd + " not found");
                return;
            }

            StandardLogging.LogInfo(FilePath, "Sudo used by " + ctx.User + " on " + member + " with command " + cmd);

            // create a fake CommandContext
            var fakeContext = cmds.CreateFakeContext(member, ctx.Channel, command, ctx.Prefix, cmd, args);

            StandardLogging.LogInfo(FilePath, "Sudo used by " + ctx.User + " on " + member + " with command " + cmd + " with fakeContext " + fakeContext);

            // and perform the sudo
            await cmds.ExecuteCommandAsync(fakeContext);


        }

        [Command("TestScrim")]
        [Description("Tests the scrim command")]
        [Hidden]
        public async Task TestScrim(CommandContext ctx, string team1name, string team2name)
        {
            StandardLogging.LogInfo(FilePath, "TestScrim used by " + ctx.User);
            if( ctx.User.IsAdmin() == false)
            {
                await ctx.RespondAsync("You are not an admin");
                return;
            }

           


            var team1 = TeamHandler.GetTeamFromName(team1name);
            var team2 = TeamHandler.GetTeamFromName(team2name);

            if(team1 == null)
            {
                await ctx.RespondAsync("Team " + team1name + " does not exist");
                return;
            }
            if(team2 == null)
            {
                await ctx.RespondAsync("Team " + team2name + " does not exist");
                return;
            }

            await ctx.RespondAsync("Starting scrim between " + team1 + " and " + team2);

            ScrimHandler.RegisterScrim(new Scrim(team1, team2, team1.game, DateTime.Now));
            ScrimHandler.ForceScrimsToDisk();

        }
        

    }
}

