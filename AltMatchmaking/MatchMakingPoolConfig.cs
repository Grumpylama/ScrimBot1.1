using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace big
{
    public struct MatchMakingPoolConfig
    {
        
        [JsonPropertyName("GameName")]
        public string GameName { get; set; }

        [JsonPropertyName("MatchMakingRuleConfigs")]
        public List<MatchMakingRuleConfig> MatchMakingRuleConfigs { get; set; }

        

        
        
    }
}