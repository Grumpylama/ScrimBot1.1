using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using big;

namespace big
{
    public class RelaxationRuleFactory
    {

        private static readonly string FilePath = "RelaxationRuleFactory.json";

        private static readonly Lazy<RelaxationRuleFactory> _instance = new Lazy<RelaxationRuleFactory>(() => new RelaxationRuleFactory());

        public static RelaxationRuleFactory Instance => _instance.Value;
        
        public IRelaxationRule CreateRelaxationRule(RelaxationRuleConfig cfg)
        {
            switch( cfg.RelaxationType)
            {
                case "TimeRelaxationRule":
                    return new TimeRelaxationRule(cfg.RuleParameters);
                default:
                    StandardLogging.LogFatal(FilePath, "Unknown relaxation rule type!");
                    throw new Exception("Unknown relaxation rule type!");
                    
            }

        }


    }
}