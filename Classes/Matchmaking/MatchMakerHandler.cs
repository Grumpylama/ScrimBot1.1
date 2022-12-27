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
            if(!contains(temp))
            {
                addMatchMaker(new MatchMaker(temp));
            }
        }

        private bool contains(MatchMakingTeam temp)
        {
            if(temp.active == true)
            {
                foreach(MatchMaker m in matchMakers)
                {
                    if(m.mmtList.BinarySearch(temp) > 0)
                    {
                        if(m.matchStart == temp.dt) 
                        {
                            m.addToMatchMakingList(temp);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void addMatchMaker(MatchMaker temp)
        {
            matchMakers.Add(temp);
        }
    }
    
}