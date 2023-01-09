


namespace big
{
    
    public class Dependecies
    {
        public DiscordClient Client;

        public MatchMakerHandler MatchMakerHandler = new MatchMakerHandler();
        public List<Team> Teams = new List<Team>();
        public List<Game> Games = new List<Game>();
        public List<DiscordUser> Users = new List<DiscordUser>();

        public List<SaveableUser> UserIDs = new List<SaveableUser>();

        public Dictionary<DiscordUser, DiscordChannel> DMChannel = new Dictionary<DiscordUser, DiscordChannel>();
        
        
        
        
        public DiscordUser GetUserFromID(ulong id)
        {
            foreach (var user in Users)
            {
                if (user.Id == id)
                {
                    return user;
                }
            }

            return null;
        }


        public Team GetTeamFromID(int id)
        {
            foreach (var team in Teams)
            {
                if (team.teamID == id)
                {
                    return team;
                }
            }

            return null;
        }

        public Game GetGameFromID(int id)
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
        
        

        public async Task<DiscordChannel> GetDMChannelAsync(DiscordUser user)
        {
            if (DMChannel.ContainsKey(user))
            {
                return DMChannel[user];
            }
            else
            {
                return null;
            }
        }

        public async Task LoadGamesAsync(List<Game> games)
        {
            Games = games;
        }
        
        //public InteractivityModule Interactivity = new InteractivityModule();
        
    }
}