using System;

namespace big
{
    
    public class Sort
    {
        private static readonly string FilePath = "Sort.cs";
        public static void Quick_Sort(List<MatchMakingTeam> list, int left, int right) 
        {
            if (left < right)
            {
                int pivot = Partition(list, left, right);

                if (pivot > 1) {
                    Quick_Sort(list, left, pivot - 1);
                }
                if (pivot + 1 < right) {
                    Quick_Sort(list, pivot + 1, right);
                }
            }
        
        }

        private static int Partition(List<MatchMakingTeam> list, int left, int right)
        {
            float pivot = list[left].T.MMR;
            while (true) 
            {

                while (list[left].T.MMR < pivot) 
                {
                    left++;
                }

                while (list[right].T.MMR > pivot)
                {
                    right--;
                }

                if (left < right)
                {
                    if (list[left].T.MMR == list[right].T.MMR) return right;

                    MatchMakingTeam temp = list[left];
                    list[left] = list[right];
                    list[right] = temp;


                }
                else 
                {
                    return right;
                }
            }
        }

        public static void InsertionSort(List<MatchMakingTeam> list)
        {
            for(int i = 1; i < list.Count; i++)
            {
                if(list[i-1].T.MMR > list[i].T.MMR)
                {
                    MatchMakingTeam temp = list[i-1];
                    list[i-1] = list[i];
                    list[i] = temp;
                }
            }
        }

        /// <summary>
        /// Method <c>FindBestMatch()<c> finds the best match for a <c>MatchMakingTeam<c> in a list of type <c>MatchMakingTeam<c>.
        /// It finds the best based on the least MMR difference.
        /// </summary>
        /// <returns>
        /// The index of the best team, in the list, to match against.
        /// </returns>
        public static Tuple<int, int> FindLowestMMRDiff(List<MatchMakingTeam> list)
        {
            int index = 0;
            float diff = Math.Abs((list[0].T.MMR - list[1].T.MMR));
            MatchMakingTeam temp = list[0];
            for(int i = 1; i < list.Count; i++)
            {
                if(temp.hasActiveRequest == true)
                {
                    if(list[i].hasActiveRequest == false)
                    {
                        float mmrdiff = Math.Abs((temp.T.MMR - list[i].T.MMR));
                        if(mmrdiff < diff)
                        {
                            index = i;
                        }
                    }
                }
                else temp = list[i];
            }
            return new (list.IndexOf(temp), index);
        }

        public static MatchMakingTeam FindBestMatch(MatchMakingTeam mt, List<MatchMakingTeam> l)        
        {
            MatchMakingTeam bestmatch = new MatchMakingTeam();

            foreach(MatchMakingTeam mmt in l)
            {
                //If the MMR difference is less than the current best match, and the team doesn't have an active request, and the team isn't the same team.
                if(Math.Abs((mt.T.MMR - mmt.T.MMR)) < Math.Abs((mt.T.MMR - bestmatch.T.MMR)) && mmt.hasActiveRequest == false && mmt.T.teamID != mt.T.teamID)
                {
                    bestmatch = mmt;
                }
            }
            return bestmatch;

        }
    }
    
}