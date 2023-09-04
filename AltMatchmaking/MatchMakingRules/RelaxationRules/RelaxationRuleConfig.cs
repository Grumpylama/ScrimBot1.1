using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace big
{
    public struct RelaxationRuleConfig
    {

        [JsonProperty("RuleName")]
        public string RuleName { get; set; }

        [JsonProperty("RuleType")]
        public string RuleType { get; set; }

        
        [JsonProperty("RuleParameters")]
        public Dictionary<string, string> RuleParameters { get; set; }
        
    }
}