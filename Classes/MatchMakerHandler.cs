using System;
using System.Collections.Generic;
using System.Linq;

namespace big
{
    
    public class MatchMakerHandler
    {
        public static List<MatchMaker> matchMakers = new List<MatchMaker>();

        public MatchMakerHandler(MatchMakingTeam temp)
        {
            //If the MatchMaker contains the dateTime already
            //then it doesn't add 
            contains(temp);

        }

        private bool contains(MatchMakingTeam temp)
        {
            foreach(MatchMaker m in matchMakers)
            {
                if(m.matchStart == temp.dt) 
                {
                    m.addToMatchMakingList(temp);
                    return true;
                }
            }
            matchMakers.Add(new MatchMaker(temp.dt));
            return false;
        }


    }
    
}