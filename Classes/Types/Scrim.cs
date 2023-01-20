namespace big
{
    
    
    public class Scrim
    {
        public Team Team1;
        public Team Team2;
        public Game Game;
        public EDate Date;
        public Scrim(Team team1, Team team2, Game game, EDate date)
        {
            this.Team1 = team1;
            this.Team2 = team2;
            this.Game = game;
            this.Date = date;
        }

        public Scrim(Team team1, Team team2, Game game)
        {
            this.Team1 = team1;
            this.Team2 = team2;
            this.Game = game;
            this.Date = new EDate();
        }

    }
}