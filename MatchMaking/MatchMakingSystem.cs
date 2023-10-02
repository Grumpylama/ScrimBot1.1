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
            foreach (string file in files)
            {
                if (file.EndsWith(".json"))
                {
                    StandardLogging.LogInfo(FilePath, $"Loading {file}");
                    MatchMakingPoolConfig config = MatchMakingConfigExtracor.Instance.ExtractMatchMakingPoolConfig(file).GetAwaiter().GetResult();
                    poolConfigs.Add(config);
                }
            }
            StandardLogging.LogInfo(FilePath, "Pool Configs Loaded");
        }

        public void VarDump()
        {
            foreach (var pool in pools)
            {
                StandardLogging.LogInfo(FilePath, "Pool: " + pool.game.GameName + " " + pool.Matchtime.ToString());
            }
        }
        


        
    


    }
}