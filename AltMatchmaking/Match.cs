using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using big;

namespace ScrimBot1._1.AltMatchmaking
{
    public class Match
    {
        

        public List<MatchmakingTicket> Tickets { get; set; } = new List<MatchmakingTicket>();

        public DateTime startTime { get; private set; }

        public int minutesToStart { get { return (int)(startTime - DateTime.UtcNow).TotalMinutes; } }

        public Match(MatchmakingContext context)
        {
           startTime = context.startTime;
           Tickets = context.Tickets;

        }
    }
}