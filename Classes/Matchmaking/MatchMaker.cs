using System;
using System.Collections.Generic;
using System.Linq;

namespace big
{
   
   public class MatchMaker
   {

        private static readonly string FilePath = "MatchMaker.cs";
      public List<MatchMakingTeam> MMTList = new List<MatchMakingTeam>();

      public List<Tuple<MatchMakingTeam, MatchMakingTeam>> MatchList = new List<Tuple<MatchMakingTeam, MatchMakingTeam>>();
      public EDate matchStart;
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
         // make sure matchmakingTeams have at least 2 teams
        if (MMTList.Count < 2)
        {
            return;
        }

        // create a dictionary for storing teams that have declined matches
        Dictionary<MatchMakingTeam, int> declinedTeams = new Dictionary<MatchMakingTeam, int>();

        while (MMTList.Count > 1)
        {
            // select the first team

            
            MatchMakingTeam team1 = MMTList[0];

            // set the initial difference to the maximum value
            float minDifference = float.MaxValue;
            MatchMakingTeam team2 = null;

            // compare the team to all other teams
            for (int i = 1; i < MMTList.Count; i++)
            {
                MatchMakingTeam currentTeam = MMTList[i];

                // calculate the difference in MMR
                float difference = Math.Abs(team1.T.MMR - currentTeam.T.MMR);

                // check if the difference is the smallest so far
                if (difference < minDifference)
                {
                    minDifference = difference;
                    team2 = currentTeam;
                }
            }

            // check if the team captain of team1 has declined a match before
            if (declinedTeams.ContainsKey(team1))
            {
                // check if the team captain of team1 has declined 3 or more matches
                if (declinedTeams[team1] >= 3)
                {
                    // move the team to a lower priority list
                    MMTList.Remove(team1);
                    continue;
                }
            }

            // check if the team captain of team2 has declined a match before
            if (declinedTeams.ContainsKey(team2))
            {
                // check if the team captain of team2 has declined 3 or more matches
                if (declinedTeams[team2] >= 3)
                {
                    // move the team to a lower priority list
                    MMTList.Remove(team2);
                    continue;
                }
            }
            
            // send DMs to the team captains to ask if they want to accept the match
            //var accept = await SendMatchAcceptDM(team1, team2);
            //if (!accept) {
            //    continue;
            //}
            // if the team captain accept the match, remove the team from matchmaking list
            MMTList.Remove(team1);
            MMTList.Remove(team2);
        }
        //return true;
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

