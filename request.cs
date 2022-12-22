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
using System.Collections.Generic;

//probably won't be used  (entire file)
public interface IRequestData
{
    public void print();
}
public class ServerRequest
{
    public string Authkey;

    public List<Request> Requests { get; set; }

    public string ToJSON()
    {
        return JsonConvert.SerializeObject(this);
    }

    public ServerRequest(string authkey)
    {
        this.Authkey = authkey;
        Requests = new List<Request>();
    }
}

public class Request
{
    public string requestType { get; set; }
    public string? TeamID = null;
    public string? GameID = null;
    public string? DiscordID = null;
    public string? TeamName = null;
    public List<string>? TeamMembers = null;
    public string? MMR = null;
    public string? Team1Score = null;
    public string? Team2Score = null;
    public string? PlayedAt = null;
    public string? UserID = null;
    public string? RequesterID = null;
    public string? TeamCaptainID = null;
    public string? Team1ID = null;
    public string? Team2ID = null;

}

/*


    public class DefaultData : IRequestData
    {
        public string data { get; set; }
        public void print()
        {
            Console.WriteLine("Default data");
        }
    }

    public class CreateUserData : IRequestData
    {
        int DiscordID { get; set; }
        public CreateUserData(string DiscordID)
        {
            try
            {
                this.DiscordID = Convert.ToInt32(DiscordID);
                Console.WriteLine( "DiscordID was converted to " + this.DiscordID);
            }
            catch
            {
                Console.WriteLine("DiscordID is not a number");
            }
        }

        public void print()
        {
            Console.WriteLine(DiscordID);
        }
    }

    public class AddTeamToDBData : IRequestData
    {
        string TeamName { get; set; }    
        int TeamCaptain { get; set; }
        int RequesterID { get; set; }
        string TeamMembers { get; set; } //Internal ID's of team members seperatad by a comma
        public void print()
        {
            Console.WriteLine("Default data");
        }
    }

    public class UpdateMMRData : IRequestData
    {
        string TeamID { get; set; }
        float MMR { get; set; }
        int GameID { get; set; }
        public void print()
        {
            Console.WriteLine("Default data");
        }
    }

    public class UpdateTeamNameData : IRequestData
    {
        int RequesterID { get; set; } // For logging purposes, also used to check if the requester is the team captain
        string TeamID { get; set; }
        string TeamName { get; set; }
        public void print()
        {
            Console.WriteLine("Default data");
        }
    }

    public class AddScrimToDBData : IRequestData
    {
        int GameID { get; set; }
        int Team1ID { get; set; }
        int Team2ID { get; set; }
        float Team1Score { get; set; } // Optional (Default 0)
        float Team2Score { get; set; } // Optional (Default 0)
        DateTime PlayedAt { get; set; }
        public void print()
        {
            Console.WriteLine("Default data");
        }
    }

    public class DeleteUserData : IRequestData
    {
        int UserID { get; set; }
        public void print()
        {
            Console.WriteLine("Default data");
        }
    }

*/



