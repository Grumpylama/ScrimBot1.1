namespace big
{
    public static class GameHandler 
    {

        private static readonly string FilePath = "GameHandler.cs";
        public static List<Game> Games = new List<Game>();

        public static Game GetGameFromID(int id)
        {
            foreach (var game in Games)
            {
                if (game.GameID == id)
                {
                    return game;
                }
            }

            return null;
        }

        public static async Task LoadGamesAsync(List<Game> games)
        {
            Games = games;
        }
    }
}