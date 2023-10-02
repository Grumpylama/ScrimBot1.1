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
global using Interfaces;
global using System.Text.Json.Serialization;






namespace big
{
    public class Program
    {
        
        private static readonly string FilePath = "Program.cs";
        public static void Main(string[] args)
        {
            //Registering the unhandled exception handler
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            

            
            


            //Create a new bot
            var bot = new Bot(); 
            StandardLogging.LogInfo(FilePath, "Bot created");           
            

            //Running the bot, won't ever resolve
            StandardLogging.LogInfo(FilePath, "Attempting to run bot");
            bot.runAsync().GetAwaiter().GetResult();

        }


        

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // Code to run when an unhandled exception occurs
            try
            {
                StandardUserHandling.GetUserFromID(244135683537502208).SendDMAsync(e.ExceptionObject.ToString()!).GetAwaiter().GetResult();
            }
            catch
            {
                
                StandardLogging.LogError(FilePath, "Failed to send notification message");
            }

            
        }

        

    }
   
}


