using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace big
{
    public interface IMatchMakingRule
    {
        
        public bool Evaluate(MatchmakingContext context);

        

    }
}