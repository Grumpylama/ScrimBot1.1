

namespace big
{
    public class SaveableUser : ISavable
    {

        public static string FilePath = "SaveableUser.cs.SaveableUser";
        public ulong ID { get; set; }

        public ulong DMChannelID { get; set; }
        
        public SaveableUser(ulong ID, ulong DMChannelID)
        {
            this.ID = ID;
            this.DMChannelID = DMChannelID;
            StandardLogging.LogInfo(FilePath, "User saved: " + ID + " " + DMChannelID);
        }
        

        public SaveableUser()
        {
            
        }
    }


    #pragma warning disable CS8618
    

    public class SaveableTeam : ISavable
    {

        public static string FilePath = "SaveableUser.cs.SavableTeam";
        public int ID { get; set; }
        public ulong CaptainID { get; set; }
        
        public float MMR { get; set; }
        public int gameID { get; set; }
        public string TeamName { get; set;}

        public SaveableTeam()
        {

            
        }

        public Team ToTeam()
        {
            Team team = new Team(GameHandler.GetGameFromID(this.gameID), this.TeamName, UserHandler.GetUserFromID(this.CaptainID));
            team.MMR = this.MMR;
            team.teamID = this.ID;
            StandardLogging.LogInfo(FilePath, "Team loaded: " + team.ToString());
            return team;
        }


    }

    

    public class SavableTeamUser : ISavable 
    {

        public static string FilePath = "SaveableUser.cs.SavableTeamUser";
        
        public ulong UserID { get; set; }
        public int TeamID { get; set; }
        public int roleID { get; set; }
        public string Position { get; set; }

        public SavableTeamUser(ulong UserID, int TeamID, int roleID, string Position)
        {
            this.UserID = UserID;
            this.TeamID = TeamID;
            this.roleID = roleID;
            this.Position = Position;
            StandardLogging.LogInfo(FilePath, "TeamUser saved: " + UserID + " " + TeamID + " " + roleID + " " + Position);

        }

        public SavableTeamUser()
        {
            
        }

        public TeamUser ToTeamUser()
        {
            TeamUser teamUser = new TeamUser(UserHandler.GetUserFromID(this.UserID), this.TeamID, this.roleID, this.Position);
            teamUser.teamID = this.TeamID;
            StandardLogging.LogInfo(FilePath, "TeamUser loaded: " + teamUser.ToString());
            return teamUser;
        }
    }

    


    
}