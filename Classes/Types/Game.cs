namespace big
{
    public class Game
    {
        
        public string GameName { get; set; }
        public int GameID { get; set; }
        public static int GameIDCounter = 0;
        public string? GameAPI { get; set; }

        public Game(string GameName, string? GameAPI)
        {
            this.GameName = GameName;
            this.GameID = GameIDCounter;
            GameIDCounter++;
            this.GameAPI = GameAPI;
        }

        public Game(string GameName, int GameID, string? GameAPI)
        {
            this.GameName = GameName;
            this.GameID = GameID;
            this.GameAPI = GameAPI;
        }


        //Empty Constructor for default values in case of error and so it can be seralized
        public Game()
        {
            this.GameName = "Default";
            this.GameID = 0;
            this.GameAPI = null;
        }
        
    }
}