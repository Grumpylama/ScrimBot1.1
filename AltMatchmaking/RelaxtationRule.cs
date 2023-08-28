using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace big
{
    public class RelaxtationRule
    {
        int stages;

        List<int> timeLeftToRelax = new List<int>();

        public RelaxtationRule(int stages)
        {
            this.stages = stages;
        }


    }
}