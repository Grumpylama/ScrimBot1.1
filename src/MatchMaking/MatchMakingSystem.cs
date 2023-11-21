using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace big
{
    public static class MatchMakingSystem
    {
        
        private static readonly string FilePath = "MatchmakingSystem.cs";

        private static List<MatchmakingPool> pools = new List<MatchmakingPool>();
        
        
    	
        private static List<MatchMakingPoolConfig> poolConfigs = new List<MatchMakingPoolConfig>();

        public static List<DateTime> GetDateTimes(Team team)
        {
            List<DateTime> tickets = new List<DateTime>();
            foreach (var pool in pools)
            {
                if(pool.Tickets.Any(x => x.team.teamID == team.teamID))
                {
                    tickets.Add(pool.Matchtime);
                }
            }

            return tickets;
        }

        public static void LoadPoolConfigs()
        {
            StandardLogging.LogInfo(FilePath, "Loading Pool Configs");
            string[] files = Directory.GetFiles("MatchMakingConfigs");
            StandardLogging.LogInfo(FilePath, "Found " + files.Length + " config files");
            foreach (string file in files)
            {
                if (file.EndsWith(".json"))
                {
                    MatchMakingPoolConfig config;
                    StandardLogging.LogInfo(FilePath, $"Loading {file}");
                    try
                    {
                        config = MatchMakingConfigExtracor.Instance.ExtractMatchMakingPoolConfig(file).GetAwaiter().GetResult();
                    }
                    catch
                    {
                        StandardLogging.LogError(FilePath, $"Error loading {file}");
                        continue;
                    }
                    
                    poolConfigs.Add(config);
                }
            }
            StandardLogging.LogInfo(FilePath, "Pool Configs Loaded");
        }

        public static async Task MatchmakingCycle()
        {
            StandardLogging.LogInfo(FilePath, "Starting Matchmaking Cycle");

            var Tasks = new List<Task>();
            foreach (var pool in pools)
            {
                Tasks.Add(pool.MatchMakingLoop());
            }
            
            await Task.WhenAll(Tasks);
        }

        public static bool AddTeamToMatchMaking(Team team, DateTime matchTime, DiscordUser responsibleUser)
        {
            StandardLogging.LogInfo(FilePath, "Adding team " + team.TeamName + " to matchmaking");
            try
            {
                if(LoadPool(team.game, matchTime).Tickets.Any(x => x.team.teamID == team.teamID))
                {
                    StandardLogging.LogError(FilePath, "Team " + team.TeamName + " already in matchmaking");
                    return false;
                }
                LoadPool(team.game, matchTime).AddTicket(new MatchmakingTicket(team, responsibleUser));
                StandardLogging.LogInfo(FilePath, "Team " + team.TeamName + " added to matchmaking");
                return true;
            }
            catch
            {
                StandardLogging.LogError(FilePath, "Error adding team " + team.TeamName + " to matchmaking");
                return false;
            }
            
            
        }

        

        

        private static MatchmakingPool LoadPool(Game game, DateTime matchTime)
        {
            
            MatchmakingPool? pool;

            if((pool = pools.Find(x => x.game.GameID == game.GameID)) is not null)
            {
                StandardLogging.LogDebug(FilePath, "Pool for " + game.GameName + " found");
                return pool;
            }
            else
            {
                StandardLogging.LogDebug(FilePath, "Pool for " + game.GameName + " not found. Creating new pool");
                MatchMakingPoolConfig? cfg;

                if((cfg = poolConfigs.Find(x => x.GameName == game.GameName)) is null)
                {
                    StandardLogging.LogError(FilePath, "Config for " + game.GameName + " not found");
                    throw new Exception("Game not supported");
                }

                
                StandardLogging.LogDebug(FilePath, "Config for " + game.GameName + " found");
                pool = MatchMakingPoolFactory.Instance.CreatePool((MatchMakingPoolConfig)cfg, matchTime);
                pools.Add(pool);
                return pool;
            }

        }

        public static void VarDump()
        {
            foreach (var pool in pools)
            {
                StandardLogging.LogInfo(FilePath, "Pool: " + pool.game.GameName);
                
            }
        }
        


        
    


    }
}