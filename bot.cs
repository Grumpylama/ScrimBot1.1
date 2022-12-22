using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Net.Http;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Linq;

namespace big
{
    public partial class Bot
    {
        

        public DiscordClient Client { get; private set; }

        public CommandsNextExtension Commands { get; private set; }


        
        public async Task runAsync()
        {

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

            Client = new DiscordClient(config);
            Client.Ready += OnClientReady;


            var CommandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableMentionPrefix = true,
                EnableDms = true

            };
            Commands = Client.UseCommandsNext(CommandsConfig);

            Commands.RegisterCommands<TestCommands>();
            Commands.RegisterCommands<TeamCommands>();

            await Client.ConnectAsync();
            Console.WriteLine("Bot is set up and running");
            await Task.Delay(-1);

        }

        private Task OnClientReady(object sender, ReadyEventArgs e)
        {
            Console.WriteLine("Bot is ready");
            return Task.CompletedTask;

        }

        //private async Task SendHttpRequestAsync(string URL, )
        //{
        //    var response = await httpclient.GetAsync("https://google.com");
        //    Console.WriteLine(response.StatusCode);
        //}

    }
}