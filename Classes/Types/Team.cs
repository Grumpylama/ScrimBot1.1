using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Entities;

namespace big
{
    public class Team
    {
        

        public static int teamIDCounter = 0;   
        public string TeamName { get; set; }
        public DiscordUser TeamCaptain { get; set; }
        
        public List<TeamUser> TeamMembers { get; set; }

        public int teamID { get; set; }
        public float MMR { get; set; }
        public Game game { get; private set; }

        //Constructor for creating a team with members
        public Team(Game game, string TeamName, DiscordUser TeamCaptain, List<TeamUser> members)
        {
            this.game = game;
            this.TeamName = TeamName;
            this.TeamCaptain = TeamCaptain;
            this.TeamMembers = members;
            
            //Change this later to actual starting MMR
            this.MMR = 0;
            this.teamID = teamIDCounter;           
            teamIDCounter++;
        }
        public Team(string TeamName, DiscordUser TeamCaptain)
        {
            this.TeamName = TeamName;
            this.TeamCaptain = TeamCaptain;
            this.TeamMembers = new List<TeamUser>();
            //Change this later to actual starting MMR
            this.MMR = 0;
            this.teamID = teamIDCounter;

            //Dummy game
            this.game = new Game();
            teamIDCounter++;    
        }
        //Constructor for creating a team with no members
        public Team(Game game, string TeamName, DiscordUser TeamCaptain)
        {
            this.TeamName = TeamName;
            this.game = game;
            this.TeamCaptain = TeamCaptain;    
            this.TeamMembers = new List<TeamUser>();    
            //Change this later to actual starting MMR
            this.MMR = 0;   
            this.teamID = teamIDCounter;        
            teamIDCounter++;
        }
        
        

        //Adds a member to the team
        public void AddMember(DiscordUser user, int roleID, string Position)
        {
            Console.WriteLine("Adding Member" + user.Id + " to team " + teamID + " as " + Position);
            TeamMembers.Add(new TeamUser(user, teamID, roleID, Position));
        }

        // Overload for default position
        public void AddMember(DiscordUser user, int roleID)
        {
            Console.WriteLine("Adding Member" + user.ToString() + " to team " + teamID);
            TeamMembers.Add(new TeamUser(user, teamID, roleID, "Default"));
        }

        public void RemoveMember(DiscordUser user)
        {
            Console.WriteLine("Removing Member" + user.Id + " from team " + teamID);
            foreach (var item in TeamMembers)
            {
                if (item.User.Id == user.Id)
                {
                    TeamMembers.Remove(item);
                    break;
                }
            }
        }
    }
}

    