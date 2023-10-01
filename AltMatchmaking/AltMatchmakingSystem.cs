using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace big
{
    public class AltMatchmakingSystem
    {
        
        private static readonly string FilePath = "AltMatchmakingSystem.cs";

        List<MatchmakingPool> pools = new List<MatchmakingPool>();

        
        public AltMatchmakingSystem()
        {
            StandardLogging.LogInfo(FilePath, "Initalizing MatchMakingSystem");



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
        


        
    


    }
}