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
        public DiscordUser? TeamCaptain { get; set; }
        
        public List<TeamUser> TeamMembers { get; set; }

        public int teamID { get; set; }
        public float MMR { get; set; }
        public Game game { get; private set; }

        public DateTime CreationTime { get; set; }
        public DiscordChannel? CaptainChannel { get; set; }

        public List<Team> AvoidedTeams {get; private set;} = new List<Team>();


        public SaveableTeam ToSavable()
        {
            SaveableTeam savableTeam = new SaveableTeam();
            savableTeam.ID = this.teamID;
            if(this.TeamCaptain is not null)
            savableTeam.CaptainID = this.TeamCaptain.Id;
            savableTeam.MMR = this.MMR;
            savableTeam.gameID = this.game.GameID;
            savableTeam.TeamName = this.TeamName;
            savableTeam.CreationTime = this.CreationTime;

            

            return savableTeam;
        }

        public bool ChangeCaptain(DiscordUser newCaptain)
        {
            if (this.TeamMembers.Exists(x => x.User.Id == newCaptain.Id))
            {
                if(this.TeamCaptain is null)
                {
                    StandardLogging.LogInfo(FilePath, "Team didn't have a captain. Setting new captain " + newCaptain.Id);
                    this.TeamMembers.Find(x => x.User.Id == newCaptain.Id)!.TrustLevel = TrustLevel.TeamCaptain;
                    this.TeamCaptain = newCaptain;
                    this.CaptainChannel = newCaptain.GetDMChannel();
                    return true;
                }
                var oldCaptain = this.TeamCaptain;
                this.TeamCaptain = newCaptain;
                this.CaptainChannel = newCaptain.GetDMChannel();

                if(this.TeamMembers.Exists(x => x.User.Id == oldCaptain.Id))
                {
                    this.TeamMembers.Find(x => x.User.Id == newCaptain.Id)!.TrustLevel = TrustLevel.TeamCaptain;
                    this.TeamMembers.Find(x => x.User.Id == oldCaptain.Id)!.TrustLevel = TrustLevel.Member;

                }
                
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> DMCaptainAsync(string message, int timeout = 300)
        {
            if(this.TeamCaptain is null)
            {
                StandardLogging.LogError(FilePath, "Can't DM captain. Captain is null");
                throw new Exception("Team captain is null");
            }
            return await this.TeamCaptain.SendMessageAsync(message, timeout);
        }

        public async Task<InteractivityResult<DSharpPlus.Entities.DiscordMessage>> ListenToCaptainAsync(double timeout = 0)
        {
            StandardLogging.LogDebug(FilePath, "Listening to captain " + this.TeamCaptain + " From the team " + this.TeamName + " with ID " + this.teamID + " for " + timeout + " seconds" + " in channel " + this.CaptainChannel);

            if(this.TeamCaptain is null)
            {
                StandardLogging.LogError(FilePath, "Can't listen to captain. Captain is null");
                throw new Exception("Team captain is null");
            }
            if(this.CaptainChannel is null)
            {
                StandardLogging.LogError(FilePath, "Can't listen to captain. CaptainChannel is null");
                throw new Exception("CaptainChannel is null");
            }
            DiscordClient client = DiscordInterface.Client!;
            InteractivityExtension interactivity = client.GetInteractivity();
            if(timeout != 0)
            {
                return await interactivity.WaitForMessageAsync(x => x.Author.Id == this.TeamCaptain.Id && x.ChannelId == this.CaptainChannel!.Id, TimeSpan.FromSeconds(timeout)); 
            }
            else
            {
                return await interactivity.WaitForMessageAsync(x => x.Author.Id == this.TeamCaptain.Id && x.ChannelId == this.CaptainChannel!.Id);
                
            }
            
        }


        public bool isAvoided(Team team)
        {
            return AvoidedTeams.Exists(x => x.teamID == team.teamID);
        }

        public void AddAvoidedTeam(Team team)
        {
            StandardLogging.LogInfo(FilePath, "Adding avoided team " + team.teamID + " to team " + this.teamID);
            AvoidedTeams.Add(team);
        }

        public void RemoveAvoidedTeam(Team team)
        {
            try
            {   
                StandardLogging.LogInfo(FilePath, "Removing avoided team " + team.teamID + " from team " + this.teamID);
                AvoidedTeams.Remove(team);
            }
            catch(Exception e)
            {
                StandardLogging.LogError(FilePath, "Error removing avoided team " + team.teamID + " from team " + this.teamID);
                StandardLogging.LogError(FilePath, e.Message);
            }
            
        }
        

        public bool changeTrustlevel(DiscordUser user, TrustLevel newTrustLevel)
        {
            if (this.TeamMembers.Exists(x => x.User.Id == user.Id))
            {
                try 
                {
                    this.TeamMembers.Find(x => x.User.Id == user.Id)!.TrustLevel = newTrustLevel;
                    return true;
                }
                catch
                {
                    StandardLogging.LogError(FilePath, "Error changing trust level of user " + user.Id + " to " + newTrustLevel.ToString());
                    return false;
                }
                
            }
            else
            {
                return false;
            }
        }

        public bool changeTrustlevel(TeamUser user, TrustLevel newTrustLevel)
        {
            StandardLogging.LogInfo(FilePath, "Changing trust level of user " + user.User.Id + " to " + newTrustLevel.ToString() + " in team " + this.teamID);

            if (this.TeamMembers.Exists(x => x.User.Id == user.User.Id))
            {
                this.TeamMembers.Find(x => x.User.Id == user.User.Id)!.TrustLevel = newTrustLevel;
                
                return true;
            }
            else
            {
                StandardLogging.LogError(FilePath, "Error changing trust level of user " + user.User.Id + " to " + newTrustLevel.ToString() + " in team " + this.teamID + " because user is not in team");
                return false;
            }
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
            CaptainChannel = TeamCaptain.GetDMChannel();   
            
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
            StandardLogging.LogDebug(FilePath, "Getting non captain members of team " + teamID);
            if(this.TeamCaptain is null)
            {
                StandardLogging.LogDebug(FilePath, "No captain is set. Returning all members");
                return TeamMembers;
            }
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

        public bool updateCaptainChannel()
        {
            StandardLogging.LogDebug(FilePath, "Updating captain channel for team " + teamID);
            if(this.TeamCaptain is null)
            {
                StandardLogging.LogError(FilePath, "Can't update captain channel. Captain is null");
                return false;
            }
            try
            {
                CaptainChannel = TeamCaptain.GetDMChannel();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        


        public override string ToString()
        {
            return $"{TeamName} playing {game.GameName}";
        }

        public string ToDiscordString()
        {
            string returnString = $" ```{TeamName} playing {game.GameName} Created { CreationTime.ToShortDateString() }\n \n";
            returnString += $"| Name            | Position | Trustlevel | ";
            foreach (var TeamMember in TeamMembers)
            {
                
                returnString += $" \n| {TeamMember.User.Username}#{TeamMember.User.Discriminator} | {TeamMember.Position} | {TeamMember.TrustLevel} |";
            }
            returnString += " ```";
            return returnString;
        }
    }

    
}

    