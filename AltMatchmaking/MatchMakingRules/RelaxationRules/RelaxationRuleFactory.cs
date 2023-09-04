using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using big;

namespace big
{
    public class RelaxationRuleFactory
    {

        
        public IRelaxationRule CreateRelaxationRule(RelaxationRuleConfig cfg)
        {
            switch( cfg.RuleType)
            {
                case "TimeRelaxationRule":
                    return new TimeRelaxationRule(cfg.RuleParameters);
                case "QueueSizeRelaxationRule":
                    return new QueueSizeRelaxationRule(cfg.RuleParameters);
            }
        }


    }
}