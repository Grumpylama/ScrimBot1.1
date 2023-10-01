using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace big
{
    public struct RelaxationRuleConfig
    {
        
        [JsonPropertyName("RelaxationType")]
        public string RelaxationType;

        [JsonPropertyName("RuleParameters")]
        public Dictionary<int, double> RuleParameters;

        
        
    }
}