

namespace big
{
    public class SaveableUser : ISavable
    {
        public ulong ID { get; set; }
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
        public int ID { get; set; }
        public ulong CaptainID { get; set; }
        public List<ulong> TeamMembers { get; set; }
        public float MMR { get; set; }
        public int gameID { get; set; }
        public string TeamName { get; set;}

        public SaveableTeam()
        {

            TeamMembers = new List<ulong>();
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