namespace big
{
    public class TeamUser
    {
        //Refrence To a user Instance
        public User user { get; set; }

        //What team the user is in, Team will include a list of this class so usually won't be necessary
        public int teamID { get; set; }

        //Role ID, Roles are 0 : Roster, 1 : Bench, 2 : Coach, 3 : Manager
        public int roleID {get; set;}

        //Position of the user in the team (Top, Jungle, Mid, ADC, Support)
        public string Position {get; set;}


        public TeamUser(User user, int teamID, int roleID, string Position)
        {
            this.user = user;
            this.teamID = teamID;
            this.roleID = roleID;
            this.Position = Position;
        }
        public TeamUser()
        {
            this.user = new User(0);
            this.teamID = 0;
            this.roleID = 0;
            this.Position = "Default";
        }
    }
}