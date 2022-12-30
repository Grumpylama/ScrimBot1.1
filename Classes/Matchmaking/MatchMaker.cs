using System;
using System.Collections.Generic;
using System.Linq;

namespace big
{
   
   public class MatchMaker
   {
      public List<MatchMakingTeam> MMTList = new List<MatchMakingTeam>();
      public DateTime matchStart;
      //There needs to be a datetime of when match starts
      //Queue
      public MatchMaker(MatchMakingTeam dt)
      {
         matchStart = dt.Dt;
      }

      //Tries to set up match based on sortBest()
      //If teams don't respond within [setTimeLimit],
      //the bot continues to the next index on list.
      public void findMatch()
      {
         
      }

      //Time threshhold nu time minus när man ska spela time (2h för att inte hamna i queue)
      //List to find a team to match it to

      public void addToMatchMakingList(MatchMakingTeam temp)
      {
         int tempus = 0;
         for(int i = 0; i < MMTList.Count; i++)
         {
            if(MMTList[i].T.teamID == temp.T.teamID) tempus++;
         }
         if(tempus == 0) MMTList.Add(temp);
         Sort.InsertionSort(MMTList);
      }

      public void removeFromMatchMakingList(Team t)
      {
         for(int i = 0; i < MMTList.Count; i++)
            if(MMTList[i].T == t) MMTList.Remove(MMTList[i]);
      }
   }
   
}