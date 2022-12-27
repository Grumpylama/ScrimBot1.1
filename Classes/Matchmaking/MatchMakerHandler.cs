using System;
using System.Collections.Generic;
using System.Linq;

namespace big
{
    
    public class MatchMakerHandler
    {
        public static List<MatchMaker> matchMakers = new List<MatchMaker>();

        public MatchMakerHandler()
        {
        }
        
        public void addMatchMakingTeam(MatchMakingTeam temp)
        {
            if(contains(temp))
            {
                //Meddela kaptenen om att ett lag har matchats
                //Fråga om de vill acceptera elr inte (visa public MMR av andra laget)
            }
            else
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

        private void removeMatchMaker(MatchMaker temp)
        {
            matchMakers.Remove(temp);
        }
    }
    
}