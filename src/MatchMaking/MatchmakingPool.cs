using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace big
{
    public class MatchmakingPool
    {
        private static readonly string FilePath = "MatchmakingPool.cs";
        public DateTime Matchtime {get; private set;}

        public Game game {get; private set;}

        public List<IMatchMakingRule> Rules = new List<IMatchMakingRule>();

        public Queue<MatchmakingTicket> Tickets {get; private set;} = new Queue<MatchmakingTicket>();

        public MatchmakingPool(Game game, DateTime Matchtime, List<IMatchMakingRule> rules)
        {
            this.game = game;
            this.Matchtime = Matchtime;
            this.Rules = rules;
        }
        

        public void AddTicket(MatchmakingTicket ticket)
        {
            Tickets.Enqueue(ticket);
        }

        public async Task MatchMakingLoop()
        {
            StandardLogging.LogInfo(FilePath, "Starting Matchmaking Loop for the game " + game.GameName + " at the time " + Matchtime);

            Queue<MatchmakingTicket> Tickets = new Queue<MatchmakingTicket>(this.Tickets);

            List<Tuple<MatchmakingTicket, MatchmakingTicket>> PotentialMatchesmatches = new List<Tuple<MatchmakingTicket, MatchmakingTicket>>();
            
        }

        
        

        public Tuple<MatchmakingTicket, MatchmakingTicket>? MatchTickets(MatchmakingTicket ticketToCheck, List<MatchmakingTicket> tickets)
        {
            foreach(MatchmakingTicket t in tickets)
            {
                if(tickets == null)
                {
                    StandardLogging.LogDebug(FilePath, "Tickets queue is null");
                    return null;
                }

                if(tickets.Count == 0)
                {
                    StandardLogging.LogDebug(FilePath, "Tickets queue is empty");
                    return null;
                }

                if (ticketToCheck is null)
                {
                    StandardLogging.LogDebug(FilePath, "Ticket to check is null");
                    return null;
                }

                if (ticketToCheck.checkedTickets.Contains(t.ticketID))
                {
                    StandardLogging.LogDebug(FilePath, "Ticket to check:" + ticketToCheck.ticketID + " has already been checked with " + t.ticketID);
                    continue;
                }

                if (t.ticketID == ticketToCheck.ticketID)
                {
                    StandardLogging.LogError(FilePath, "Ticket to check is the same as the ticket to match with. Don't know how but well done soldier.");
                    continue;
                }

                if(ticketToCheck.team == t.team)
                {
                    StandardLogging.LogError(FilePath, "Ticket to check: "+ ticketToCheck.ticketID +" has the same team as: " + t.ticketID);
                    continue;
                }


                if(!Rules.Any(x => x.Evaluate(new MatchmakingContext(Matchtime, Tickets.Count, 0,  new List<MatchmakingTicket>(){ticketToCheck, t}))))
                {
                    StandardLogging.LogInfo(FilePath, "Tickets " + ticketToCheck.ticketID + " and " + t.ticketID + " are a match for the time " + Matchtime + " with the game " + game.GameName);
                    return new Tuple<MatchmakingTicket, MatchmakingTicket>(ticketToCheck, t);
                }

                ticketToCheck.checkedTickets.Add(t.ticketID);

            }

            return MatchTickets(tickets.ElementAt(0), tickets.Skip(1).ToList());
        }







    }
}
