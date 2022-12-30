using System;
using System.Collections.Generic;
using System.Linq;

namespace big
{
    
    public class MatchMakerHandler
    {
        public static List<MatchMaker> matchMakers = new List<MatchMaker>();

        public Game game;

        public MatchMakerHandler()
        {
            
        }
        
        public StatusCode addMatchMakingTeam(MatchMakingTeam temp)
        {
            if(contains(temp))
            {
                return new StatusCode(true, "You're team has been added to a matchmaker. \n You will be notified when you have been matchmade!");
            }
            else
            {
                addMatchMaker(new MatchMaker(temp));
                return new StatusCode(false, "You have been added to the matchmaking queue. \n You will be notified when you have been matchmade!");
            }
        }

        //If the matchmaker for the specific time and game already exists
        //then add the Team to the matchmaker
        //Otherwise create a new matchmaker.
        private bool contains(MatchMakingTeam temp)
        {
            if(temp.Active == true)
            {
                foreach(MatchMaker m in matchMakers)
                {
                    if(m.MMTList.BinarySearch(temp) > 0)
                    {
                        if(m.matchStart == temp.Dt) 
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