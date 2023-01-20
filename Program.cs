﻿global using DSharpPlus;
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
global using System.Timers;
global using System.Threading;

namespace big
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            //Registering the unhandled exception handler
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            //Registering the save timer
            var saveTimer = new System.Timers.Timer(900000); // 900000 milliseconds = 15 minutes
            saveTimer.Elapsed += TimerSave;
            saveTimer.Start();


            //Create a new bot
            var bot = new Bot();            
            

            //Running the bot, won't ever resolve
            bot.runAsync().GetAwaiter().GetResult();

        }


        private static void TimerSave(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Timersave, Saving data");
            FileManager.SaveAll();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // Code to run when an unhandled exception occurs
            try{
                UserHandler.GetUserFromID(244135683537502208).SendDMAsync(e.ToString()).GetAwaiter().GetResult();
            }
            catch{
                Console.WriteLine("Failed to send notification message");
            }

            
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            try{
                FileManager.SaveAll();
            }
            catch{
                Console.WriteLine("Failed to save data");
                Console.WriteLine("Last Save: " + FileManager.LastSave);
            }
            
            // Code to run when the application exits
        }

    }
   
}


