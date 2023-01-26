using DSharpPlus.Entities;

namespace big
{
    public class TeamUser
    {

        private static readonly string FilePath = "TeamUser.cs";

        //Refrence To a user Instance
        public DiscordUser User { get; set; }

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
            return "TeamUser: " + this.User.Username + " " + this.teamID + " " + this.roleID + " " + this.Position;
        }

        
        public SavableTeamUser ToSavable()
        {
            SavableTeamUser savableTeamUser = new SavableTeamUser();
            savableTeamUser.UserID = this.User.Id;
            savableTeamUser.TeamID = this.teamID;
            savableTeamUser.roleID = this.roleID;
            savableTeamUser.Position = this.Position;
            savableTeamUser.TrustLevel = getIntFromTrustLevel(this.TrustLevel);
            return savableTeamUser;
        }



        public TeamUser(DiscordUser user, int teamID, int roleID, string Position, TrustLevel TrustLevel = 0)
        {
            this.User = user;
            this.teamID = teamID;
            this.roleID = roleID;
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
                case TrustLevel.CanEditTrustLevel:
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
        CanEditTrustLevel = 5,
        TeamCaptain = 6

        

    }
}