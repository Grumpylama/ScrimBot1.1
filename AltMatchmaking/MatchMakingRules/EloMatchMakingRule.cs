using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace big
{
    public class EloMatchMakingRule : IMatchMakingRule
    {
        
        
        private double StandardValue { get; set;}

        private List<IRelaxationRule> RelaxationsRules;

        public EloMatchMakingRule(int standardValue, List<IRelaxationRule> relaxationsRules)
        {
            this.StandardValue = standardValue;
            this.RelaxationsRules = relaxationsRules;
        }




        public bool Evaluate(MatchmakingContext context) 
        {
            throw new NotImplementedException();
        }

        
        
    }
}