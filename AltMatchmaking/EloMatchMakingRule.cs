using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace big
{
    public class EloMatchMakingRule : IMatchMakingRule
    {
        
        public List<int> Spans {get; private set;}

        private List<IRelaxationRule> RelaxationsRules;

        public EloMatchMakingRule(List<int> spans)
        {
            Spans = spans;
        }

        public bool Evavluate(MatchmakingContext context)
        {
            throw new NotImplementedException();
        }

        
        
    }
}