using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace big
{
    public class TimeRelaxationRule : IRelaxationRule
    {
        private static readonly string FilePath = "TimeRelaxationRule.cs";

        private Dictionary<int, double> parameters {get; set;}


        

        public TimeRelaxationRule(Dictionary<int, double> parameters)
        {
            this.parameters = parameters;
                
        
        }
        
       


        public double GetRelaxationStage(MatchmakingContext context)
        {
            return 0;
        }


    }
}