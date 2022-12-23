using System;
using System.Collections.Generic;
using System.Linq;

namespace big
{
   public class MatchMaker
   {
      public List<MatchMakingTeam> mmtList = new List<MatchMakingTeam>();
      public DateTime matchStart;
      //There needs to be a datetime of when match starts
      //Queue
      public MatchMaker(DateTime dt)
      {
         matchStart = dt;
      }

      //Tries to set up match based on sortBest()
      //If teams don't respond within [setTimeLimit],
      //the bot continues to the next index on list.
      public void findMatch()
      {
         
      }

      //Time threshhold nu time minus när man ska spela time (2h för att inte hamna i queue)
      //List to find a team to match it to

      public void addToMatchMakingList(DateTime dt, Team t)
      {
         MatchMakingTeam temp = new MatchMakingTeam(dt, t);
         int tempus = 0;
         for(int i = 0; i < mmtList; i++)
         {
            if(mmtList[i].t.teamID == t.teamID) tempus++;
         }
         if(tempus == 0) mmtList.Add(temp);
      }

      public void removeFromMatchMakingList(Team t)
      {
         for(int i = 0; i < mmtList.Count; i++)
            if(mmtList[i].t == t) mmtList.Remove(mmtList[i]);
      }

      
   }
}