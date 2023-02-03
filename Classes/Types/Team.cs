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
        private static int teamIDCounter = 0;   
        public string TeamName { get; set; }
        public DiscordUser TeamCaptain { get; set; }
        
        public List<TeamUser> TeamMembers { get; set; }

        public int teamID { get; set; }
        public float MMR { get; set; }
        public Game game { get; private set; }

        public DateTime CreationTime { get; set; }


        public SaveableTeam ToSavable()
        {
            SaveableTeam savableTeam = new SaveableTeam();
            savableTeam.ID = this.teamID;
            savableTeam.CaptainID = this.TeamCaptain.Id;
            savableTeam.MMR = this.MMR;
            savableTeam.gameID = this.game.GameID;
            savableTeam.TeamName = this.TeamName;
            savableTeam.CreationTime = this.CreationTime;

            

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


        
        
        
        //Constructor for creating a team with no members
        public Team(Game game, string TeamName, DiscordUser TeamCaptain, bool AddCaptain = true, DateTime CreationTime = default) 
        {
            this.TeamName = TeamName;
            this.game = game;
            this.TeamCaptain = TeamCaptain;    
            this.TeamMembers = new List<TeamUser>();  
            this.teamID = teamIDCounter;

            if(CreationTime == default)
                this.CreationTime = DateTime.Now;
            else
                this.CreationTime = CreationTime;
            teamIDCounter++;
            if (AddCaptain)
            {
                TeamMembers.Add(new TeamUser(TeamCaptain, teamID, "Captain", TrustLevel.TeamCaptain));  
            }
            
            //Change this later to actual starting MMR
            this.MMR = 0;   
            
        }

        

        public Team()
        {
            this.TeamName = "Default";
            this.CreationTime = DateTime.Now;
            this.TeamMembers = new List<TeamUser>();
            //Change this later to actual starting MMR
            this.MMR = 0;
            this.teamID = teamIDCounter;
            this.game = new Game();
            teamIDCounter++;
        }
        
        

        public static void SetStaticID(int id)
        {
            StandardLogging.LogInfo(FilePath, "Setting Static ID to " + id);
            teamIDCounter = id;
        }
        //Adds a member to the team
        public void AddMember(DiscordUser user, string Position)
        {
           
            StandardLogging.LogInfo(FilePath, "Adding Member" + user.Id + " to team " + teamID + " as " + Position);
            TeamMembers.Add(new TeamUser(user, teamID, Position));
        }

        // Overload for default position
        public void AddMember(DiscordUser user)
        {
            
            StandardLogging.LogInfo(FilePath, "Adding Member" + user.Id + " to team " + teamID + " as Default");
            TeamMembers.Add(new TeamUser(user, teamID, "Member"));
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

        public string ToDiscordString()
        {
            string returnString = $" ```{TeamName} playing {game.GameName} Created { CreationTime.ToShortDateString() }\n \n";
            returnString += $"| Name | Position | Trustlevel | ";
            foreach (var item in TeamMembers)
            {
                if(item.User.Id != TeamCaptain.Id)
                returnString += $" \n{item.User.Username}#{item.User.Discriminator}";
            }
            returnString += " ```";
            return returnString;
        }
    }

    
}

    