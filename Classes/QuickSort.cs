using System;

namespace big
{
    
    public class QuickSort
    {
        private static void Quick_Sort(List<MatchMakingTeam> list, int left, int right) 
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
            float pivot = list[left].t.MMR;
            while (true) 
            {

                while (list[left].t.MMR < pivot) 
                {
                    left++;
                }

                while (list[right].t.MMR > pivot)
                {
                    right--;
                }

                if (left < right)
                {
                    if (list[left].t.MMR == list[right].t.MMR) return right;

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
    }
    
}