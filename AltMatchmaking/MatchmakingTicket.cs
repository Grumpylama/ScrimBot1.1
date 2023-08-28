namespace big
{
    internal class MatchmakingTicket
    {
        private static readonly string FilePath = "MatchmakingTicket.cs";
        public DateTime joinTime {get; private set;}
        public Team team {get; private set;}

        public DiscordUser ResponsibleUser {get; private set;}

        public List<Team> avoidedTeams {get; private set;} = new List<Team>();

        public MatchmakingTicket(Team team, DiscordUser responsibleUser)
        {
            this.team = team;
            this.ResponsibleUser = responsibleUser;
            joinTime = DateTime.Now;

        }


    }
}