using System;

namespace big
{
    
    public class Sort
    {
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
        public static int FindLowestMMRDiff(List<MatchMakingTeam> list, MatchMakingTeam teamski)
        {
            int index = 0;
            float diff = (list[0].T.MMR - list[1].T.MMR)*(-1);
            MatchMakingTeam temp = list[0];
            for(int i = 1; i < list.Count; i++)
            {
                float mmrdiff = (temp.T.MMR - list[i].T.MMR)*(-1);
                if(mmrdiff < diff)
                {
                    index = i;
                }
            }
            return index;
        }
    }
    
}