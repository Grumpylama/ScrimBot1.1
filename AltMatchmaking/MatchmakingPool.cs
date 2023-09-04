using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace big
{
    internal class MatchmakingPool
    {
        private static readonly string FilePath = "MatchmakingPool.cs";
        public DateTime Matchtime {get; private set;}

        public Game game {get; private set;}

        public List<IMatchMakingRule> Rules = new List<IMatchMakingRule>();

        public Queue<MatchmakingTicket> Tickets {get; private set;} = new Queue<MatchmakingTicket>();

        public MatchmakingPool(Game game, DateTime matchtime)
        {
            this.game = game;
            this.Matchtime = matchtime;
        }

        public void AddTicket(MatchmakingTicket ticket)
        {
            Tickets.Enqueue(ticket);
        }

        public void MatchMakingLoop(int index)
        {
            if (Tickets.Count < index + 1)
            return;

            var ticket = Tickets.ElementAt(index);
            if (ticket == null)
            return;

            


        }

        public Tuple<MatchmakingTicket, MatchmakingTicket>? MatchTickets(MatchmakingTicket ticketToCheck, List<MatchmakingTicket> tickets)
        {
            foreach(MatchmakingTicket t in tickets)
            {
                if(tickets == null)
                {
                    StandardLogging.LogDebug(FilePath, "Tickets list is null");
                    return null;
                }

                if(tickets.Count == 0)
                {
                    StandardLogging.LogDebug(FilePath, "Tickets list is empty");
                    return null;
                }

                if (ticketToCheck is null)
                {
                    StandardLogging.LogDebug(FilePath, "Ticket to check is null");
                    return null;
                }

                if (ticketToCheck.checkedTickets.Contains(t.ticketID))
                {
                    StandardLogging.LogDebug(FilePath, "Ticket to check has already been checked with this ticket");
                    continue;
                }

                if (t.ticketID == ticketToCheck.ticketID)
                {
                    StandardLogging.LogError(FilePath, "Ticket to check is the same as the ticket to match with. Don't know how but well done");
                    continue;
                }

                if(ticketToCheck.team == t.team)
                {
                    StandardLogging.LogError(FilePath, "Ticket to check has the same team as the ticket to match with");
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
