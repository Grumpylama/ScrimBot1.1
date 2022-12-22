namespace big
{
    public class Team
    {
        public static List<Team> Teams = new List<Team>();

        public static int teamIDCounter = 0;   
        public string TeamName { get; set; }
        public User TeamCaptain { get; set; }
        
        public List<TeamUser> TeamMembers { get; set; }

        public int teamID { get; set; }
        public float MMR { get; set; }
        public Game game { get; private set; }

        //Constructor for creating a team with members
        public Team(Game game, string TeamName, ulong TeamCaptain, List<TeamUser> members)
        {
            this.game = game;
            this.TeamName = TeamName;
            this.TeamCaptain = SearchForUser(TeamCaptain);
            this.TeamMembers = members;
            //Change this later to actual starting MMR
            this.MMR = 0;
            this.teamID = teamIDCounter;           
            teamIDCounter++;
        }
        public Team(string TeamName, ulong TeamCaptain)
        {
            this.TeamName = TeamName;
            this.TeamCaptain = SearchForUser(TeamCaptain);
            this.TeamMembers = new List<TeamUser>();
            //Change this later to actual starting MMR
            this.MMR = 0;
            this.teamID = teamIDCounter;

            //Dummy game
            this.game = new Game();
            teamIDCounter++;
        }
        //Constructor for creating a team with no members
        public Team(Game game, string TeamName, ulong TeamCaptain)
        {
            this.TeamName = TeamName;
            this.game = game;
            this.TeamCaptain = SearchForUser(TeamCaptain);    
            this.TeamMembers = new List<TeamUser>();    
            //Change this later to actual starting MMR
            this.MMR = 0;   
            this.teamID = teamIDCounter;        
            teamIDCounter++;
        }
        //Dummy constructor for seralzation
        //DO NOT USE!!!!
        private Team()
        {
            this.game = new Game();
            this.TeamName = "Default";
            this.TeamCaptain = new User();       
            this.TeamMembers = new List<TeamUser>(); 
            //Change this later to actual starting MMR
            this.MMR = 0;                 
            this.teamID = teamIDCounter;
            teamIDCounter++;
        }
        //Searces for a user in the user list should be moved to User class :))
        private User SearchForUser(ulong UserDiscordID)
        {
            foreach (var item in User.Users)
            {
                if (item.DiscordID == UserDiscordID)
                {
                    return item;
                }
            }
            return null;
        }

        //Adds a member to the team
        public void AddMember(User user, int roleID, string Position)
        {
            Console.WriteLine("Adding Member" + user.UserID + " to team " + teamID + " as " + Position);
            TeamMembers.Add(new TeamUser(user, teamID, roleID, Position));
        }

        // Overload for default position
        public void AddMember(User user, int roleID)
        {
            Console.WriteLine("Adding Member" + user.UserID + " to team " + teamID);
            TeamMembers.Add(new TeamUser(user, teamID, roleID, "Default"));
        }

        public void RemoveMember(User user)
        {
            Console.WriteLine("Removing Member" + user.UserID + " from team " + teamID);
            foreach (var item in TeamMembers)
            {
                if (item.user.UserID == user.UserID)
                {
                    TeamMembers.Remove(item);
                    break;
                }
            }
        }
    }
}

    