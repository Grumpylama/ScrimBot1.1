global using DSharpPlus;
global using DSharpPlus.CommandsNext;
global using DSharpPlus.CommandsNext.Attributes;
global using DSharpPlus.Entities;
global using DSharpPlus.Interactivity.Extensions;
global using DSharpPlus.Interactivity.EventHandling;
global using DSharpPlus.Interactivity.Enums;   
global using DSharpPlus.EventArgs;
global using DSharpPlus.Interactivity;
global using Newtonsoft.Json;
global using System;
global using System.IO;
global using System.Text;
global using System.Threading.Tasks;
global using System.Collections.Generic;
global using System.Text.RegularExpressions;
global using System.Security.Cryptography;
global using System.Linq;

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


