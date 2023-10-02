namespace big
{
    

    public class Scrim
    {
        public static ulong IDCounter = 0;
        public ulong ID;


        public Team Team1 {get; private set;}
        public Team Team2 {get; private set;}
        public int Team1Score {get; set;}
        public int Team2Score {get; set;}

        public Game Game {get; private set; }
        public DateTime Date {get; set;}

        public bool Finished {get; set;}

        public SavableScrim ToSavable()
        {
            return new SavableScrim(this);
        }

        public Scrim(ulong ID, Team team1, Team team2, Game game, DateTime date)
        {
            this.ID = ID;

            this.Team1 = team1;
            this.Team2 = team2;
            this.Game = game;
            this.Date = date;
            this.Team1Score = 0;
            this.Team2Score = 0;
        }

        public Scrim(Team team1, Team team2, Game game, DateTime date)
        {
            this.ID = IDCounter;
            IDCounter++;

            this.Team1 = team1;
            this.Team2 = team2;
            this.Game = game;
            this.Date = date;
            this.Team1Score = 0;
            this.Team2Score = 0;
        }
        

    }
}