using DSharpPlus.Entities;

namespace big
{
    public class TeamUser
    {
        //Refrence To a user Instance
        public DiscordUser User { get; set; }

        //What team the user is in, Team will include a list of this class so usually won't be necessary
        public int teamID { get; set; }

        //Role ID, Roles are 0 : Roster, 1 : Bench, 2 : Coach, 3 : Manager
        public int roleID {get; set;}

        //Position of the user in the team (Top, Jungle, Mid, ADC, Support)
        public string Position {get; set;}

        
        public SavableTeamUser ToSavable()
        {
            SavableTeamUser savableTeamUser = new SavableTeamUser();
            savableTeamUser.ID = this.teamID;
            savableTeamUser.UserID = this.User.Id;
            savableTeamUser.TeamID = this.teamID;
            savableTeamUser.roleID = this.roleID;
            savableTeamUser.Position = this.Position;
            return savableTeamUser;
        }

        public TeamUser(DiscordUser user, int teamID, int roleID, string Position)
        {
            this.User = user;
            this.teamID = teamID;
            this.roleID = roleID;
            this.Position = Position;
        }

        
        
    }
}