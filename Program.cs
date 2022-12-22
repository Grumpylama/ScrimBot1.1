
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;


namespace big
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var bot = new Bot();


            List<Team> teams = new List<Team>();
            Game.Games.Add(new Game("Game1", null));
            Game.Games.Add(new Game("Game2", null));
            Game.Games.Add(new Game("Game3", null));
            Game.Games.Add(new Game("Game4", null));

            
            bot.runAsync().GetAwaiter().GetResult();

        }
    }
   
}


