using System;
using System.Collections.Generic;
using System.Linq;

namespace big
{
   public class MatchMaker
   {
      public static List<MatchMakingTeam> matchmakinglist = new List<MatchMakingTeam>();
      //time, Team
      //Queue
      public MatchMaker()
      {
      }

      //Tries to set up match based on sortBest()
      //If teams don't respond within [setTimeLimit],
      //the bot continues to the next index on list.
      public void findMatch()
      {
         
      }

      //Puts best matches in a list,
      //sorted best - index 0, worst - last index.
      private void sortBest()
      {

      }

      public void addToMatchMakingList(DateTime dt, Team t)
      {
         MatchMakingTeam temp = new MatchMakingTeam(dt, t);
         matchmakinglist.Add(temp);
      }

      public void removeFromMatchMakingList(Team t)
      {
         for(int i = 0; i < matchmakinglist.Count; i++)
            if(matchmakinglist[i].t == t) matchmakinglist.Remove(matchmakinglist[i]);
      }

      
   }
}