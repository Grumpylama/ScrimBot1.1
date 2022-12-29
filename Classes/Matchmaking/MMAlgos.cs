using System;

namespace big
{
    public class MMAlgos
    {
        //When there are 2 hours left to matchStart
        public async Task FindMatch(MatchMaker m)
        {
            bool NotFound = true;
            int i = 1;
            MatchMakingTeam temp = m.MMTList[0];
            int index = 0;
            //Since the list is sorted from highest MMR to lowest MMR

            while(NotFound)
            {
                index = Sort.FindBestMatch(m.MMTList, temp);

                //send message to both temp and m.MMTList[i]
                //If temp doesn't answer or refuses, make m.MMTList[i] the new temp - set temp as inactive
                //If m.MMTList[i] doesn't answer or refuses, move on to the next i (i++) - set m.MMTList[i] as inactive
                //If neither answer or both refuse then just move up one step each - set both to inactive
                //IF both answer then break loop
                if(false)
                {
                    //temp.setInactive();
                    m.MMTList.Remove(temp);
                    temp = m.MMTList[index];
                    i++;
                }
                if(false)
                {
                    //m.MMTList[index].setInactive();
                    m.MMTList.Remove(m.MMTList[index]);
                    i++;
                }
                if(false)
                {
                    //temp.setInactive();
                    //m.MMTList[index].setInactive();
                    
                    m.MMTList.Remove(temp);
                    m.MMTList.Remove(m.MMTList[index]);
                    temp = m.MMTList[index];
                    i++;
                }
                if(true)
                {
                    NotFound = true;
                }
            }
        }

        //When there is 30 minutes left to matchStart
        public void StressFindMatch()
        {

        }
    }
}