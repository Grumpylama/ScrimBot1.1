namespace big
{
    public class MatchmakingTicket
    {
        private static readonly string FilePath = "MatchmakingTicket.cs";

        private static int ticketIDCounter = 1;
        
        public int ticketID {get; private set;} = 0;
        public DateTime joinTime {get; private set;}
        public Team team {get; private set;}

        public DiscordUser ResponsibleUser {get; private set;}

        public double Elo {get; set;} 

        public List<Team> avoidedTeams {get; private set;} = new List<Team>();

        public List<int> checkedTickets{get; private set;} = new List<int>();
        


        public MatchmakingTicket(Team team, DiscordUser ResponsibleUser)
        {
            ticketID = ticketIDCounter;
            ticketIDCounter++;
            this.team = team;
            this.ResponsibleUser = ResponsibleUser;
            joinTime = DateTime.Now;

        }

        public bool PromtTeamOfScrim(MatchmakingPool pool, MatchmakingTicket opponentTicket)
        {
            StandardLogging.LogDebug(FilePath, "Confirming Scrim between " + team.TeamName + " and " + opponentTicket.team.TeamName);
            if(opponentTicket is null)
            {
                StandardLogging.LogError(FilePath, "Opponent ticket is null");
                return false;
            }

            this.ResponsibleUser.

        }

        public void ResetCheckedTickets()
        {
            checkedTickets.Clear();
        }


    }
}