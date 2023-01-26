

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
            Team team = new Team(GameHandler.GetGameFromID(this.gameID), this.TeamName, UserHandler.GetUserFromID(this.CaptainID), false);
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

        public int TrustLevel { get; set; }

        public SavableTeamUser(ulong UserID, int TeamID, int roleID, string Position, int TrustLevel = 0)
        {
            this.UserID = UserID;
            this.TeamID = TeamID;
            this.roleID = roleID;
            this.Position = Position;
            this.TrustLevel = TrustLevel;
            StandardLogging.LogInfo(FilePath, "TeamUser saved: " + UserID + " " + TeamID + " " + roleID + " " + Position);

        }

        public SavableTeamUser()
        {
            
        }

        public TeamUser ToTeamUser()
        {
            TeamUser teamUser = new TeamUser(UserHandler.GetUserFromID(this.UserID), this.TeamID, this.roleID, this.Position, GetTrustLevel(this.TrustLevel));
            teamUser.teamID = this.TeamID;
            StandardLogging.LogInfo(FilePath, "TeamUser loaded: " + teamUser.ToString());
            return teamUser;
        }

        private TrustLevel GetTrustLevel(int trustLevel)
        {
            switch (trustLevel)
            {
                case 0:
                    return big.TrustLevel.Member;
                case 1:
                    return big.TrustLevel.CanMatchMake;
                case 2:
                    return big.TrustLevel.CanAddMembers;
                case 3:
                    return big.TrustLevel.CanRemoveMembers;
                case 4:
                    return big.TrustLevel.CanEditTeam;
                case 5:
                    return big.TrustLevel.CanEditTrustLevel;
                case 6:
                    return big.TrustLevel.TeamCaptain;
                default:
                    return big.TrustLevel.Member;

            
            }
        }
    }

    


    
}