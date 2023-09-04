using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace big
{
    public struct MatchMakingRuleConfig
    {
        public string RuleType;

        public int StandardValue;

        public List<RelaxationRuleConfig> RelaxationRules;

        
    }
}