using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace big
{
    public class QueueSizeRelaxationRule : IRelaxationRule
    {
        private static readonly string FilePath = "QUeueSizeRelaxatinoRule.cs";

        public Dictionary<string, string> RuleParameters;
        
        public QueueSizeRelaxationRule(Dictionary<int, int> ruleParameters)
        {
            
        }

        public int getStage(MatchmakingContext context)
        {
            
        }


    }
}