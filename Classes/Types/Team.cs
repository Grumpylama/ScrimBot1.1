using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Entities;

namespace big
{
    public class Team
    {
        
        private static readonly string FilePath = "Team.cs";
        public static int teamIDCounter = 0;   
        public string TeamName { get; set; }
        public DiscordUser TeamCaptain { get; set; }
        
        public List<TeamUser> TeamMembers { get; set; }

        public int teamID { get; set; }
        public float MMR { get; set; }
        public Game game { get; private set; }


        public SaveableTeam ToSavable()
        {
            SaveableTeam savableTeam = new SaveableTeam();
            savableTeam.ID = this.teamID;
            savableTeam.CaptainID = this.TeamCaptain.Id;
            savableTeam.MMR = this.MMR;
            savableTeam.gameID = this.game.GameID;
            savableTeam.TeamName = this.TeamName;

            

            return savableTeam;
        }

        public async Task<bool> DMCaptainAsync(string message)
        {
            return await this.TeamCaptain.SendMessageAsync(message);
        }

        public async Task<bool> DMCaptainAsync(string message, int timeout)
        {
            return await this.TeamCaptain.SendMessageAsync(message, timeout);
        }


        //Constructor for creating a team with members
        public Team(Game game, string TeamName, DiscordUser TeamCaptain, List<TeamUser> members)
        {
            this.game = game;
            this.TeamName = TeamName;
            this.TeamCaptain = TeamCaptain;
            this.TeamMembers = members;

            //Change this later to actual starting MMR
            this.MMR = 0;
            this.teamID = teamIDCounter;           
            teamIDCounter++;
        }
        public Team(string TeamName, DiscordUser TeamCaptain)
        {
            this.TeamName = TeamName;
            this.TeamCaptain = TeamCaptain;
            this.TeamMembers = new List<TeamUser>();
            //Change this later to actual starting MMR
            this.MMR = 0;
            this.teamID = teamIDCounter;

            //Dummy game
            this.game = new Game();
            teamIDCounter++;    
        }
        //Constructor for creating a team with no members
        public Team(Game game, string TeamName, DiscordUser TeamCaptain, bool AddCaptain = true) 
        {
            this.TeamName = TeamName;
            this.game = game;
            this.TeamCaptain = TeamCaptain;    
            this.TeamMembers = new List<TeamUser>();  
            if (AddCaptain)
            {
                TeamMembers.Add(new TeamUser(TeamCaptain, teamID, 0, "Captain", TrustLevel.TeamCaptain));  
            }
            
            //Change this later to actual starting MMR
            this.MMR = 0;   
            this.teamID = teamIDCounter;        
            teamIDCounter++;
        }

        public Team()
        {
            this.TeamName = "Default";
            
            this.TeamMembers = new List<TeamUser>();
            //Change this later to actual starting MMR
            this.MMR = 0;
            this.teamID = teamIDCounter;
            this.game = new Game();
            teamIDCounter++;
        }
        
        

        //Adds a member to the team
        public void AddMember(DiscordUser user, int roleID, string Position)
        {
           
            StandardLogging.LogInfo(FilePath, "Adding Member" + user.Id + " to team " + teamID + " as " + Position);
            TeamMembers.Add(new TeamUser(user, teamID, roleID, Position));
        }

        // Overload for default position
        public void AddMember(DiscordUser user, int roleID = 0)
        {
            
            StandardLogging.LogInfo(FilePath, "Adding Member" + user.Id + " to team " + teamID + " as Default");
            TeamMembers.Add(new TeamUser(user, teamID, roleID, "Default"));
        }

        public void RemoveMember(DiscordUser user)
        {
        
            StandardLogging.LogInfo(FilePath, "Removing Member" + user.Id + " from team " + teamID);
            foreach (var item in TeamMembers)
            {
                if (item.User.Id == user.Id)
                {
                    TeamMembers.Remove(item);
                    break;
                }
            }
        }

        public List<TeamUser> GetMembers()
        {
            return TeamMembers;
        }

        public List<TeamUser> GetNonCaptainMembers()
        {
            List<TeamUser> nonCaptainMembers = new List<TeamUser>();
            foreach (var item in TeamMembers)
            {
                if (item.User.Id != TeamCaptain.Id)
                {
                    nonCaptainMembers.Add(item);
                }
            }
            return nonCaptainMembers;
        }
        


        public override string ToString()
        {
            return $"{TeamName} playing {game.GameName}";
        }
    }

    
}

    