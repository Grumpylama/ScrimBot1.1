using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace big
{
    public class RecentMatchMatchmakingRule : IMatchMakingRule
    {
        public List<IRelaxationRule> RelaxationsRules;

        public RecentMatchMatchmakingRule(float standardvalue, List<IRelaxationRule> relaxationsRules)
        {
            RelaxationsRules = relaxationsRules;
        }

        public bool Evaluate(MatchmakingContext context)
        {
            return true;
        }


    }
}