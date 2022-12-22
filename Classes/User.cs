using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace big
{
    
    public class User
    {
        public static int UserIDCounter = 0;
        //A list of all users
        public static List<User> Users = new List<User>();

        

        //Internal ID for the user
        public int UserID;
        //Discord ID for the user       
        public ulong DiscordID;

        private static XmlSerializer seralizer = new XmlSerializer(typeof(User));
        private static string path = Environment.CurrentDirectory + "/XML/Users.xml";

        public List<Game> gamesplayed = new List<Game>();

        //public List<UserGames> UserGames = new List<UserGames>();
        
        
        //Jag tänker att vi använder API för att extrahera infon vi behöver om usern för deras Elo etc. I respektive spel
        //Varje spels API fungerar annorlunda, så vi matchar informationen som behövs med respektive API?
        //League har spelar Namn (summonerName) https://developer.riotgames.com/apis#summoner-v4/GET_getBySummonerName; 
        //Dota verkar ha account_id (not sure) https://docs.opendota.com/#tag/playersByRank
        //Overwatch 2 har inte API än :O
        //Behöver vi ens kalkulera egen ELO om de redan finns i deras API? Vi kan bara uppdatera när de behövs
        //Kanske bara behöver rating systemet som en klass
        public User(ulong DiscordID)
        {
            this.DiscordID = DiscordID;
            this.UserID = UserIDCounter;
            UserIDCounter++;
            Users.Add(this);
        }
        //Dummy constructor for seralzation
        public User()
        {
            this.DiscordID = 0;
            this.UserID = UserIDCounter;
            UserIDCounter++;
            Users.Add(this);
        }

        public void AddGame(Game game)
        {
            gamesplayed.Add(game);
        }

        public static User GetUser(ulong DiscordID)
        {
            foreach (var user in Users)
            {
                if (user.DiscordID == DiscordID)
                {
                    return user;
                }
            }
            return null;
        }

        public static void SaveXML()
        {
            Console.WriteLine("Saving Users to XML");
            using (var Writer = new StreamWriter(path))
            {
                Console.WriteLine("Starting Saving Users to XML");
                seralizer.Serialize(Writer, Users);
                Console.WriteLine("Finished Saving Users to XML");
            }     
            Console.WriteLine("Saved Users to XML");
            return;     
        }
        
    }
    
}