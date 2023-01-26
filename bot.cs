

namespace big
{
    public partial class Bot
    {
        

        public DiscordClient Client { get; private set; }

        public CommandsNextExtension Commands { get; private set; }

        private static readonly string FilePath = "Bot.cs";
        
        public async Task runAsync()
        {
            
            
            StandardLogging.LogInfo(FilePath, "Starting Bot");
            
            StandardLogging.LogInfo(FilePath, "Loading Config");
            var json = String.Empty;
            using (var fs = File.OpenRead("BotConfig.json"))            
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);

            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);
            


            

            
            var config = new DiscordConfiguration
            {
                Token = configJson.Token,
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
                StringPrefixes = new string[] { configJson.Prefix },
                EnableMentionPrefix = true,
                EnableDms = true

            };

            StandardLogging.LogInfo(FilePath, $"Command Config is as follows: {CommandsConfig}" );

            try
            {
                Commands = Client.UseCommandsNext(CommandsConfig);
            }
            catch(Exception e)
            {
                StandardLogging.LogFatal(FilePath, $"Error while starting commands: {e}");
                StandardLogging.LogFatal(FilePath, $"Command Config is as follows: {CommandsConfig}" );
            }

            StandardLogging.LogInfo(FilePath, "Commands are ready");


            StandardLogging.LogInfo(FilePath, "Registering Commands");
            try
            {
                //Commands.RegisterCommands<AdminCommands>();
                Commands.RegisterCommands<AdminCommands>();
                Commands.RegisterCommands<Commands>();
                //Commands.RegisterCommands<QuickCommands>();
                
            }
            catch(Exception e)
            {
                StandardLogging.LogError(FilePath, $"Error while registering commands: {e}");
            }
            

            StandardLogging.LogInfo(FilePath, "Commands are registered");
            
            

            await Client.ConnectAsync();
            
            big.DiscordInterface.Client = Client;
            
            StandardLogging.LogInfo(FilePath, "Attempting to start FileManager");
            await FileManager.StartUpAsync();
            
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