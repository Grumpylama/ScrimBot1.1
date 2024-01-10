using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace big
{
    public class MatchmakingPool
    {
        private static readonly string FilePath = "MatchmakingPool.cs";
        public DateTime Matchtime {get; private set;}
        private System.Timers.Timer MatchmakingLoopTimer;
        public Game game {get; private set;}

        private int TicketsMathced = 0;

        public List<IMatchMakingRule> Rules = new List<IMatchMakingRule>();

        public List<MatchmakingTicket> Tickets {get; private set;} = new List<MatchmakingTicket>();

        public MatchmakingPool(Game game, DateTime Matchtime, List<IMatchMakingRule> rules)
        {
            this.game = game;
            this.Matchtime = Matchtime;
            this.Rules = rules;
            this.MatchmakingLoopTimer = new System.Timers.Timer(300000);
            this.MatchmakingLoopTimer.Elapsed += TriggerMatchMakingLoopOnTimer;
            this.MatchmakingLoopTimer.AutoReset = true;
            this.MatchmakingLoopTimer.Start();

        }
        

        /// <summary>
        /// Adds a ticket to the pool
        /// </summary>
        /// <param name="ticket">The ticket to add</param>
         
        public void AddTicket(MatchmakingTicket ticket)
        {
            if(ticket is null)
            {
                StandardLogging.LogError(FilePath, "Ticket is null");
                return;
            }
            if(Tickets.Any(x => x.team.teamID == ticket.team.teamID))
            {
                StandardLogging.LogDebug(FilePath, "Team " + ticket.team.TeamName + " is already in the pool");
                return;
            }
            else
            {
                Tickets.Add(ticket);
                StandardLogging.LogInfo(FilePath, "Ticket " + ticket.ticketID + " added to the pool");
            }
        }

        private void TriggerMatchMakingLoopOnTimer(object? sender, ElapsedEventArgs e)
        {
            MatchMakingLoop().GetAwaiter().GetResult();
        }

       
        public async Task MatchMakingLoop()
        {
            StandardLogging.LogInfo(FilePath, "Starting Matchmaking Loop for the game " + game.GameName + " at the time " + Matchtime);

            if(Tickets.Count < 2)
            {
                StandardLogging.LogDebug(FilePath, "Not Enough Tickets in pool to matchmake");
                return;
            }

            

        }

        
        

        public Tuple<MatchmakingTicket, MatchmakingTicket>? MatchTickets(MatchmakingTicket ticketToCheck, List<MatchmakingTicket> tickets)
        {
            StandardLogging.LogDebug(FilePath, "Matching ticket " + ticketToCheck.ticketID + " against " + tickets.Count + " tickets");
            if(ticketToCheck is null)
            {
                StandardLogging.LogError(FilePath, "Ticket to check is null");
                return null;
            }

            if(tickets.Count == 0)
            {
                StandardLogging.LogDebug(FilePath, "No tickets to match against");
                return null;
            }

            foreach (var opponentTicket in tickets)
            {
                if(ticketToCheck.team.teamID == opponentTicket.team.teamID)
                {
                    StandardLogging.LogDebug(FilePath, "Ticket " + ticketToCheck.ticketID + " is the same team as ticket " + opponentTicket.ticketID);
                    continue;
                }


                if(ticketToCheck.team.game.GameID != opponentTicket.team.game.GameID)
                {
                    StandardLogging.LogError(FilePath, "Ticket " + ticketToCheck.ticketID + " is not the same game as ticket " + opponentTicket.ticketID);
                    continue;
                }
         
                if(!this.Rules.Any(x => x.Evaluate(new MatchmakingContext(this.Matchtime, this.Tickets.Count, this.TicketsMathced, ticketToCheck, opponentTicket))))
                {
                    StandardLogging.LogDebug(FilePath, "Tickets " + ticketToCheck.ticketID + " and " + opponentTicket.ticketID + " do not match");
                    continue;
                }

            }

            StandardLogging.LogDebug(FilePath, "No matches found for ticket " + ticketToCheck.ticketID);
            return null;

        }



    }
}
