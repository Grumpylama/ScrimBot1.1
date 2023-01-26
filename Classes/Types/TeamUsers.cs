using DSharpPlus.Entities;

namespace big
{
    public class TeamUser
    {

        private static readonly string FilePath = "TeamUser.cs";

        //Refrence To a user Instance
        public DiscordUser User { get; set; }

        public static List<PropertyInfo> GetProperties()
        {
            List<PropertyInfo> properties = new List<PropertyInfo>();
            properties.Add(typeof(TeamUser).GetProperty("User"));
            properties.Add(typeof(TeamUser).GetProperty("teamID"));
            properties.Add(typeof(TeamUser).GetProperty("roleID"));
            properties.Add(typeof(TeamUser).GetProperty("Position"));
            properties.Add(typeof(TeamUser).GetProperty("TrustLevel"));
            return properties;
        }

        //What team the user is in, Team will include a list of this class so usually won't be necessary
        public int teamID { get; set; }

        //Role ID, Roles are 0 : Roster, 1 : Bench, 2 : Coach, 3 : Manager
        public int roleID {get; set;}

        //Position of the user in the team (Top, Jungle, Mid, ADC, Support)
        public string Position {get; set;}

        //Trust level of the user, 0 = Normal member 1 = Can matchmake 2 = Can add members 3 = Can remove members 4 = Can edit team 
        // 5 = Can edit Trust Level 6 = TeamCaptain
        public TrustLevel TrustLevel {get; set;}

        public override string ToString()
        {
            return "TeamUser: " + this.User.Username + " " + TeamHandler.GetTeamFromID(this.teamID) + " " + this.Position;
        }

        
        public SavableTeamUser ToSavable()
        {
            SavableTeamUser savableTeamUser = new SavableTeamUser();
            savableTeamUser.UserID = this.User.Id;
            savableTeamUser.TeamID = this.teamID;
            savableTeamUser.Position = this.Position;
            savableTeamUser.TrustLevel = getIntFromTrustLevel(this.TrustLevel);
            return savableTeamUser;
        }



        public TeamUser(DiscordUser user, int teamID, string Position, TrustLevel TrustLevel = 0)
        {
            this.User = user;
            this.teamID = teamID;
            this.Position = Position;
            this.TrustLevel = TrustLevel;

        }
        

        public DiscordUser GetUser()
        {
            return this.User;
        }

        private int getIntFromTrustLevel(TrustLevel tl)
        {
            switch (tl)
            {
                case TrustLevel.Member:
                    return 0;
                case TrustLevel.CanMatchMake:
                    return 1;
                case TrustLevel.CanAddMembers:
                    return 2;
                case TrustLevel.CanRemoveMembers:
                    return 3;
                case TrustLevel.CanEditTeam:
                    return 4;
                case TrustLevel.CanEditTrustLevels:
                    return 5;
                case TrustLevel.TeamCaptain:
                    return 6;
                default:
                    return 0;
            }
        }

        
        
    }


    public enum TrustLevel
    {
        Member = 0,
        CanMatchMake = 1,
        CanAddMembers = 2,
        CanRemoveMembers = 3,
        CanEditTeam = 4,
        CanEditTrustLevels = 5,
        TeamCaptain = 6

        

    }
}