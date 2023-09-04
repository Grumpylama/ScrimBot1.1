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