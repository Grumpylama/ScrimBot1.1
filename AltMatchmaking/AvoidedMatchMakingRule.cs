using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace big
{
    public class AvoidedMatchMakingRule : IMatchMakingRule
    {


        public bool Relaxation { get; private set;}

        public AvoidedMatchMakingRule(IRelaxationRule relaxationRule)
        {


        }
        
        public bool Evavluate(MatchmakingContext context)
        {
            throw new NotImplementedException();
        }



    }
}