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
        


        /// <summary>
        /// Creates a new matchmaking ticket
        /// </summary>
        /// <param name="team">The team that is joining the matchmaking</param>
        /// <param name="ResponsibleUser">The user that is responsible for the team</param>
        public MatchmakingTicket(Team team, DiscordUser ResponsibleUser)
        {
            ticketID = ticketIDCounter;
            ticketIDCounter++;
            this.team = team;
            this.ResponsibleUser = ResponsibleUser;
            joinTime = DateTime.Now;

        }


        /// <summary>
        /// Promts the responsible user to confirm a scrim with the opponent team
        /// </summary>
        /// <param name="pool">The pool the scrim is in</param>
        /// <param name="opponentTicket">The ticket of the opponent team</param>
        /// <returns>True if the scrim was confirmed, false if it was not</returns>
        /// <exception cref="Exception">Thrown when the opponent ticket is null</exception>
        
        public bool PromtTeamOfScrim(MatchmakingPool pool, MatchmakingTicket opponentTicket)
        {
            StandardLogging.LogDebug(FilePath, "Confirming Scrim between " + team.TeamName + " and " + opponentTicket.team.TeamName);
            if(opponentTicket is null)
            {
                StandardLogging.LogError(FilePath, "Opponent ticket is null");
                return false;
            }

            return this.ResponsibleUser.PromtForScrim(pool.Matchtime, opponentTicket).GetAwaiter().GetResult();

        }


        public void ResetCheckedTickets()
        {
            checkedTickets.Clear();
        }


    }
}