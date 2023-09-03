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







    }
}
