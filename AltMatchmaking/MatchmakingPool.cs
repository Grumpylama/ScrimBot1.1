namespace big
{
    internal class MatchmakingPool
    {
        private static readonly string FilePath = "MatchmakingPool.cs";
        public DateTime Matchtime {get; private set;}

        public Game game {get; private set;}

        public List<MatchmakingTicket> Tickets {get; private set;} = new List<MatchmakingTicket>();

        public MatchmakingPool(Game game, DateTime matchtime)
        {
            this.game = game;
            this.Matchtime = matchtime;
        }

    }
}
