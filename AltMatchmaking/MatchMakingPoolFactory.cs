using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using big;

namespace ScrimBot1._1.AltMatchmaking
{
    public class MatchMakingPoolFactory
    {
        
        private static readonly Lazy<MatchMakingPoolFactory> _instance = new Lazy<MatchMakingPoolFactory>(() => new MatchMakingPoolFactory());


        public static MatchMakingPoolFactory Instance => _instance.Value;

        private MatchMakingRuleFactory _ruleFactory;

        public MatchMakingPoolFactory()
        {
            
                _ruleFactory = MatchMakingRuleFactory.Instance;
            
            
        }


    }
}