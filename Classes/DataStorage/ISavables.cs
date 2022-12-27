

namespace big
{
    public class SaveableUser : ISavable
    {
        public ulong ID;
        public SaveableUser(ulong ID)
        {
            this.ID = ID;
        }

        public SaveableUser()
        {
            
        }
    }

    public class SaveableTeam : ISavable
    {
        public int ID;
        public ulong CaptainID;
        public List<ulong> TeamMembers = new List<ulong>();
        public float MMR;
        public int gameID;
        public string TeamName;

        public SaveableTeam(){}


    }

    public class SavableGame : ISavable 
    {
        string gameName;

        public int ID;

        public string GameAPI;

        public SavableGame()
        {
            
        }

    }

    public class SavableTeamUser : ISavable 
    {
        public int ID;
        public ulong UserID;
        public int TeamID;
        public int roleID;
        public string Position;

        public SavableTeamUser(ulong UserID, int TeamID, int roleID, string Position)
        {
            this.UserID = UserID;
            this.TeamID = TeamID;
            this.roleID = roleID;
            this.Position = Position;
        }

        public SavableTeamUser()
        {
            
        }
    }

    


    
}