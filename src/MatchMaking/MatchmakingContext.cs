using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace big
{
    public struct MatchmakingContext
    {

        public DateTime startTime { get; private set; }
        public int minutesToStart { get { return (int)(startTime - DateTime.UtcNow).TotalMinutes; } }

        public List<MatchmakingTicket> Tickets {get; private set;}

        public int TicketsInQueue { get; private set; }

        public int TicketsMatched { get; private set; }

        public MatchmakingContext(DateTime startTime, int ticketsInQueue, int ticketsMatched, List<MatchmakingTicket> tickets)
        {
            this.Tickets = tickets;
            this.startTime = startTime;
            this.TicketsInQueue = ticketsInQueue;
            this.TicketsMatched = ticketsMatched;
        }

        public MatchmakingContext(DateTime startTime, int ticketsInQueue, int ticketsMatched) : this(startTime, ticketsInQueue, ticketsMatched, new List<MatchmakingTicket>())
        {

        }


    }
}