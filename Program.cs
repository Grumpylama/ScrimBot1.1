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

            FileManager.StartUp();

            var bot = new Bot();

            


            List<Team> teams = new List<Team>();
            Dependecies.Games.Add(new Game("Game1", null));
            Dependecies.Games.Add(new Game("Game2", null));
            Dependecies.Games.Add(new Game("Game3", null));
            Dependecies.Games.Add(new Game("Game4", null));

            SaveableUser user = new SaveableUser(123);
            SaveableUser user2 = new SaveableUser(124);
            SaveableUser user3 = new SaveableUser(125);
            SaveableUser user4 = new SaveableUser(126);
            SaveableUser user5 = new SaveableUser(127);

            var cols = user.GetType().GetProperties();




            bot.runAsync().GetAwaiter().GetResult();

        }
    }
   
}


