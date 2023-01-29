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
global using System.Timers;
global using System.Threading;
global using Serilog.Sinks.SystemConsole;
global using Serilog;
global using Serilog.Sinks.File;
global using System.Reflection;





namespace big
{
    public class Program
    {
        
        private static readonly string FilePath = "Program.cs";
        public static void Main(string[] args)
        {
            //Registering the unhandled exception handler
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);

            
            

            


            //Registering the save timer
            var saveTimer = new System.Timers.Timer(900000); // 900000 milliseconds = 15 minutes
            saveTimer.Elapsed += TimerSave;
            saveTimer.Start();
            StandardLogging.LogInfo(FilePath, "Save timer started");


            //Create a new bot
            var bot = new Bot(); 
            StandardLogging.LogInfo(FilePath, "Bot created");           
            

            //Running the bot, won't ever resolve
            StandardLogging.LogInfo(FilePath, "Attempting to run bot");
            bot.runAsync().GetAwaiter().GetResult();

        }


        private static void TimerSave(object sender, ElapsedEventArgs e)
        {
            
            StandardLogging.LogInfo(FilePath, "Timersave, Saving data");
            FileManager.SaveAll();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // Code to run when an unhandled exception occurs
            try
            {
                UserHandler.GetUserFromID(244135683537502208).SendDMAsync(e.ToString()).GetAwaiter().GetResult();
            }
            catch{
                
                StandardLogging.LogError(FilePath, "Failed to send notification message");
            }

            
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            StandardLogging.LogInfo(FilePath, "Process exit, saving data");
            try{
                FileManager.SaveAll();
            }
            catch(Exception ex)
            {
                StandardLogging.LogError(FilePath, "Failed to save data");
                StandardLogging.LogError(FilePath, "Last Save: " + FileManager.LastSave);
                StandardLogging.LogError(FilePath, ex.ToString());
            }
            
            // Code to run when the application exits
        }

    }
   
}


