using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace big
{
    public class MatchMakingConfigExtracor
    {
        
        public static MatchMakingConfigExtracor Instance => _instance.Value;

        private static readonly Lazy<MatchMakingConfigExtracor> _instance = new Lazy<MatchMakingConfigExtracor>(() => new MatchMakingConfigExtracor());

        

        public MatchMakingConfigExtracor()
        {
            
                _ruleFactory = MatchMakingRuleFactory.Instance;
            
            
        }

        

    }
}