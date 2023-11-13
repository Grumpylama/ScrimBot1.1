using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace big
{
    public interface IRelaxationRule
    {
        public double GetRelaxationStage(MatchmakingContext context);
    }
}