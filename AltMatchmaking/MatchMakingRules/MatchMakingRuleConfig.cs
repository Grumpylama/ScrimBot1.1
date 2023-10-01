using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace big
{
    public struct MatchMakingRuleConfig
    {
        [JsonPropertyName("RuleType")]
        public string RuleType;

        [JsonPropertyName("StandardValue")]
        public int StandardValue;

        [JsonPropertyName("RelaxationRules")]
        public List<RelaxationRuleConfig> RelaxationRulesConfigs;

        
    }
}