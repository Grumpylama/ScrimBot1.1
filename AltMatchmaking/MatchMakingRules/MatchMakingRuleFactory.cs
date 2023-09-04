using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace big
{
    public class MatchMakingRuleFactory
    {

        private static MatchMakingRuleFactory _instance;
        
        private RelaxationRuleFactory _relaxationRuleFactory;

        public IMatchMakingRule GetRule(MatchMakingRuleConfig cfg)
        {
            switch(cfg.RuleType)
            {
                case "EloMatchMakingRule":
                    List<IRelaxationRule> relaxations = new List<IRelaxationRule>();
                    foreach(RelaxationRuleConfig relaxationRuleConfig in cfg.RelaxationRules)
                    {
                        relaxations.Add(_relaxationRuleFactory.CreateRelaxationRule(relaxationRuleConfig));
                    }
                    return new EloMatchMakingRule(cfg.StandardValue, relaxations); 
                default:
                    throw new Exception("Unknown rule type");
            }
        }


    }
}