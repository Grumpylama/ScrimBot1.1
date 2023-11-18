using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace big
{
    public class MatchMakingRuleFactory
    {

        private static readonly Lazy<MatchMakingRuleFactory> _instance = new Lazy<MatchMakingRuleFactory>(() => new MatchMakingRuleFactory());

        public static MatchMakingRuleFactory Instance => _instance.Value;
        
        private RelaxationRuleFactory _relaxationRuleFactory;

        public MatchMakingRuleFactory()
        {
            
            _relaxationRuleFactory = RelaxationRuleFactory.Instance;

        }

        public IMatchMakingRule GetRule(MatchMakingRuleConfig cfg)
        {
            List<IRelaxationRule> relaxations = new List<IRelaxationRule>();
            foreach(RelaxationRuleConfig relaxationRuleConfig in cfg.RelaxationRulesConfigs)
            {
                relaxations.Add(_relaxationRuleFactory.CreateRelaxationRule(relaxationRuleConfig));
            }

            switch(cfg.RuleType)
            {
                case "EloMatchMakingRule":
                    return new EloMatchMakingRule(cfg.StandardValue, relaxations); 
                case "AvoidedTeamMatchMakingRule" :
                    return new AvoidedMatchMakingRule(relaxations);
                case "RecentGameMatchMakingRule":
                    return new RecentMatchMatchmakingRule(cfg.StandardValue, relaxations);  
                default:
                    throw new Exception("Unknown rule type");
            }
        }


    }
}