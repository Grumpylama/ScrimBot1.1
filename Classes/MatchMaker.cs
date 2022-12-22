using System;
using System.Collections.Generic;
using System.Linq;

namespace big
{
   public class MatchMaker
   {
      public static Queue<MatchMakingTeam> matchmakingqueue = new Queue<MatchMakingTeam>();
      //time, Team
      //Queue
      public MatchMaker()
      {
      }

      public void addToMatchMakingQueue(DateTime dt, Team t)
      {
         MatchMakingTeam temp = new MatchMakingTeam(dt, t);
         matchmakingqueue.Enqueue(temp);
      }

      public void removeFromMatchMakingQueue(Team t)
      {
         matchmakingqueue = new Queue<MatchMakingTeam>(matchmakingqueue.Where(
            x => x.t.teamID != t.teamID));
      }

      
   }
}