using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace big
{
    public class RelaxationRuleFactory
    {
        
        public static RelaxtationRule CreateRelaxationRule(int stages, Game game)
        {
            
            

            return new RelaxtationRule(stages);
        }

    }
}