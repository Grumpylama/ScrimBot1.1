using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace big
{
    public class TimeRelaxationRule : IRelaxationRule
    {
        private static readonly string FilePath = "TimeRelaxtationRule.cs";

        public int stages
        {
            get
            {
                return timeLeftToRelax.Count;
            }
        }


        List<int> timeLeftToRelax = new List<int>();

        public TimeRelaxationRule(List<int> timesLeftToRelax)
        {
            timeLeftToRelax = timesLeftToRelax;
        }

        public int GetRelaxationStage(MatchmakingContext context)
        {
            return 0;
        }


    }
}