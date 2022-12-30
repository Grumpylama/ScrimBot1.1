using System;

namespace big
{
    public class MMAlgos
    {

        //When there is 30 minutes left to matchStart
        public void StressFindMatch()
        {

        }

        //When there are 2 hours left to matchStart
        public bool FindMatch(MatchMaker m)
        {
            if(m.MMTList.Count > 1) 
            {
                MatchFindHelper(m); 
                return true;
            }
            return false;
        }
        //How long do we wait for the answer? D:
        private void MatchFindHelper(MatchMaker m)
        {
            int index = Sort.FindLowestMMRDiff(m.MMTList, m.MMTList[0]);
            //If [0] team doesn't answer or refuses, 
            //Set temp as inactive
            //Replace [0] team with team 'index'
            //Call the function
            if(false)
                {
                    //m.MMT[0].setInactive();
                    m.MMTList[0] = m.MMTList[index];
                    m.MMTList.RemoveAt(index);
                    MatchFindHelper(m);
                }
            //If m.MMTList[i] doesn't answer or refuses
            //Remove it and call the function
            if(false)
                {
                    //m.MMT[index].setInactive();
                    m.MMTList.RemoveAt(index);
                    MatchFindHelper(m);
                }
            //If neither answer or both refuse
            //Set both inactive AND remove both
            //Call function
            if(false)
                {
                    //m.MMTList[0].setInactive();
                    //m.MMTList[index].setInactive();
                    MatchMakingTeam temp = m.MMTList[index];
                    m.MMTList.RemoveAt(0);
                    m.MMTList.Remove(temp);
                    MatchFindHelper(m);
                }
        }
    }
}