using System;
using System.Collections.Generic;
using System.Linq;

namespace big
{
    private static readonly string FilePath = "MatchMakerHandler.cs";
    
    public class MatchMakerHandler
    {
        private static readonly string FilePath = "MatchMakerHandler.cs";   
        public static List<MatchMaker> matchMakers = new List<MatchMaker>();

        public Game game;

        public MatchMakerHandler()
        {
            
        }
        
        public StatusCode addMatchMakingTeam(MatchMakingTeam temp)
        {
            StandardLogging.LogInfo.LogInfo(FilePath + " Adding team " + temp.Name + " to a matching matchMaker");
            if(contains(temp))
            {
                return new StatusCode(true, "You're team has been added to a matchmaker. \n You will be notified when you have been matchmade!");
            }
            else
            {
                MatchMaker newTeam = new MatchMaker(temp);
                addMatchMaker(newTeam);
                StandardLogging.LogInfo.LogInfo(FilePath + " Team has been added to a new matchmaker, specified above.");
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
                        if(m.matchStart.Date == temp.Dt.Date) 
                        {
                            m.addToMatchMakingList(temp);
                            StandardLogging.LogInfo.LogInfo(FilePath + " Team " + temp.Name + " has been successfully added to " + m.ToString);
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

        private string toString()
        {
            string s = matchMakers.Count;
            return s;
        }
    }
    
}