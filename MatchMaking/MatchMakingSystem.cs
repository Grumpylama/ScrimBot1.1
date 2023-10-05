using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace big
{
    public class MatchMakingSystem
    {
        
        private static readonly string FilePath = "MatchmakingSystem.cs";

        private List<MatchmakingPool> pools = new List<MatchmakingPool>();

        
        public MatchMakingSystem()
        {
            StandardLogging.LogInfo(FilePath, "Initalizing MatchMakingSystem");
            LoadPoolConfigs();
            StandardLogging.LogInfo(FilePath, "MatchMakingSystem Initalized");
        }


        List<MatchMakingPoolConfig> poolConfigs = new List<MatchMakingPoolConfig>();

        private void LoadPoolConfigs()
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

        public bool AddTeamToMatchMaking(Team team, DateTime matchTime, DiscordUser responsibleUser)
        {
            StandardLogging.LogInfo(FilePath, "Adding team " + team.TeamName + " to matchmaking");
            try
            {
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

        

        

        private MatchmakingPool LoadPool(Game game, DateTime matchTime)
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

        public void VarDump()
        {
            foreach (var pool in pools)
            {
                StandardLogging.LogInfo(FilePath, "Pool: " + pool.game.GameName);
                
            }
        }
        


        
    


    }
}