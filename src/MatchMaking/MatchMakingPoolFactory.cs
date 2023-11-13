using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using big;

namespace big
{
    public class MatchMakingPoolFactory
    {
        private static readonly string FilePath = "MatchMakingPoolFactory.cs";
        
        private static readonly Lazy<MatchMakingPoolFactory> _instance = new Lazy<MatchMakingPoolFactory>(() => new MatchMakingPoolFactory());


        public static MatchMakingPoolFactory Instance => _instance.Value;

        private MatchMakingRuleFactory _ruleFactory;

        public MatchMakingPoolFactory()
        {
 
            _ruleFactory = MatchMakingRuleFactory.Instance;
    
        }

        public MatchmakingPool CreatePool(MatchMakingPoolConfig cfg, DateTime time)
        {
            Game game;
            StandardLogging.LogDebug(FilePath, "Creating pool with conifg " + cfg.ToString());
            try
            {
                StandardLogging.LogDebug(FilePath, "Getting game from name " + cfg.GameName);
                game = GameHandler.GetGameFromName(cfg.GameName);
            }
            catch
            {
                StandardLogging.LogError(FilePath, "Game " + cfg.GameName + " not found when creating pool");
                throw new Exception("Game not found");
            }
            
            List<IMatchMakingRule> rules = new List<IMatchMakingRule>();

            try
            {
                StandardLogging.LogDebug(FilePath, "Getting rules from config");
                cfg.MatchMakingRuleConfigs.ForEach(x => rules.Add(_ruleFactory.GetRule(x)));
            }
            catch
            {
                StandardLogging.LogError(FilePath, "Error getting rules from config");
                throw new Exception("Error getting rules from config");
            }
            

            return new MatchmakingPool(game, time, rules);
        }


    }
}