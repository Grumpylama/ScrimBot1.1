using System.Data;

namespace big
{
    public static class GameHandler 
    {

        private static readonly string FilePath = "GameHandler.cs";
        public static List<Game> Games = new List<Game>();
        public static Game GetGameFromID(int id)
        {
            StandardLogging.LogDebug(FilePath, "Getting game " + id);
            Game? g = null;
            if((g = Games.Find(x => x.GameID == id)) is not null)
            {
                StandardLogging.LogDebug(FilePath, "Game " + id + " found. Game is: " + g.GameName);
                return g;
            }
            else
            {
                StandardLogging.LogError(FilePath, "Game " + id + " not found");
                throw new Exception("Game not found");
            }

            
        }

        public static void LoadGamesAsync(List<Game> games)
        {
            Games = games;
        }

        public static Game GetGameFromName(string name)
        {
            StandardLogging.LogDebug(FilePath, "Getting game " + name);
            if(Games.Exists(x => x.GameName == name))
            {
                StandardLogging.LogDebug(FilePath, "Game " + name + " found");
                return Games.Find(x => x.GameName == name)!;
            }
            else
            {
                StandardLogging.LogError(FilePath, "Game " + name + " not found");
                throw new Exception("Game not found");
            }
        }

        public static void VarDump()
        {
            foreach (var game in Games)
            {
                StandardLogging.LogInfo(FilePath, "Game: " + game.GameName);
            }
        }
    }
}