

namespace big
{
    public partial class Bot
    {
        
        #pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        //This is a false positive, since it is initialized when it runs
        public DiscordClient Client { get; private set; }

        public IAdminInterface AdminInterface = new AdminInterface();

        public CommandsNextExtension Commands { get; private set; }

        private static readonly string FilePath = "Bot.cs";
        
        public async Task runAsync()
        {
            
            StandardLogging.LogInfo(FilePath, "Starting Bot");
            
            StandardLogging.LogInfo(FilePath, "Loading Config");
            BotConfigExtractor extractor = new BotConfigExtractor();

            
            string token = AdminInterface.PromtKey();

            
            
            
            var config = new DiscordConfiguration
            {
                Token = token,
                TokenType = TokenType.Bot,
                
            };
            StandardLogging.LogInfo(FilePath, "Config Loaded");
            
            try{
                Client = new DiscordClient(config);
            }
            catch(Exception e)
            {
                StandardLogging.LogFatal(FilePath, $"Error while starting bot with config: {config} {e}");
            }
            StandardLogging.LogInfo(FilePath, "Bot Started");

            Client.Ready += OnClientReady;
            

            Client.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromMinutes(2)
            });
            
            StandardLogging.LogInfo(FilePath, "Interactivity is ready");

            StandardLogging.LogInfo(FilePath, "Starting Commands");

            var CommandsConfig = new CommandsNextConfiguration
            {    
                StringPrefixes = new string[] { "!" },
                EnableMentionPrefix = true,
                EnableDms = true

            };

            

            try
            {
                Commands = Client.UseCommandsNext(CommandsConfig);
            }
            catch(Exception e)
            {
                StandardLogging.LogFatal(FilePath, $"Error while starting commands: {e}");
                
            }

            StandardLogging.LogInfo(FilePath, "Commands are ready");


            StandardLogging.LogInfo(FilePath, "Registering Commands");
            try
            {
                //Commands.RegisterCommands<AdminCommands>();
                Commands.RegisterCommands<AdminCommands>();
                Commands.RegisterCommands<Commands>();
                Commands.RegisterCommands<TestingCommands>();
                //Commands.RegisterCommands<QuickCommands>();
                
            }
            catch(Exception e)
            {
                StandardLogging.LogError(FilePath, $"Error while registering commands: {e}");
            }
            

            StandardLogging.LogInfo(FilePath, "Commands are registered");
            
            

            await Client.ConnectAsync();
            
            // Set up the discord interface
            big.DiscordInterface.Client = Client;
            
            StandardLogging.LogInfo(FilePath, "Attempting to start FileManager");
            await FileManager.StartUpAsync();
            FileManager.SetUpTimerSave(900000); // 15 minutes
            
            Client.GuildMemberAdded += EventListners.OnGuildMemberAdded;

            //Wait infinitely so the bot stays connected
            await Task.Delay(-1);

        }

        private Task OnClientReady(object sender, ReadyEventArgs e)
        {
            StandardLogging.LogInfo(FilePath, "Bot is ready");
            return Task.CompletedTask;

        }

       


         

    }
}