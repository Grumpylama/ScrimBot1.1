

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
        
        public float MMR { get; set; }
        public int gameID { get; set; }
        public string TeamName { get; set;}

        public SaveableTeam()
        {

            
        }

        public Team ToTeam(Dependecies d)
        {
            Team team = new Team(d.GetGameFromID(this.gameID), this.TeamName, d.GetUserFromID(this.CaptainID));
            team.MMR = this.MMR;
            team.teamID = this.ID;

            return team;
        }


    }

    

    public class SavableTeamUser : ISavable 
    {
        public int ID { get; set; }
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
        }

        public SavableTeamUser()
        {
            
        }

        public TeamUser ToTeamUser(Dependecies d)
        {
            TeamUser teamUser = new TeamUser(d.GetUserFromID(this.UserID), this.TeamID, this.roleID, this.Position);
            teamUser.teamID = this.TeamID;
            return teamUser;
        }
    }

    


    
}