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
        public async Task<bool> FindMatchAsync(Dependecies d, MatchMaker m)
        {
            if(m.MMTList.Count > 1) 
            {
                MatchFindHelperAsync(d, m);
                return true;
            }
            return false;
        }
        //How long do we wait for the answer? D:
        private async Task MatchFindHelperAsync(Dependecies d, MatchMaker m)
        {
            int index = Sort.FindLowestMMRDiff(m.MMTList);
            Tuple<ScrimResponse, ScrimResponse> Answer = await PromtCaptains(d, m.MMTList[0], m.MMTList[index], 60);

            m.MMTList[0].hasActiveRequest = true;
            m.MMTList[index].hasActiveRequest = true;


            //If [0] team doesn't answer or refuses,
            //Set temp as inactive
            //Replace [0] team with team 'index'
            //Call the function
            if(Answer.Item1 == ScrimResponse.Accept)
                {
                    //m.MMT[0].setInactive();
                    m.MMTList[0] = m.MMTList[index];
                    m.MMTList.RemoveAt(index);
                    MatchFindHelperAsync(m);
                }
            //If m.MMTList[i] doesn't answer or refuses
            //Remove it and call the function
            if(false)
                {
                    //m.MMT[index].setInactive();
                    m.MMTList.RemoveAt(index);
                    MatchFindHelperAsync(m);
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
                    MatchFindHelperAsync(m);
                }
        }


        private async Task<Tuple<ScrimResponse, ScrimResponse>> PromtCaptains(Dependecies d, MatchMakingTeam m1, MatchMakingTeam m2, int timeout)
        {
            
            List<Task<ScrimResponse>> tasks = new List<Task<ScrimResponse>>();
            tasks.Add(PromtCaptain(d, m1, m2, timeout));
            tasks.Add(PromtCaptain(d, m2, m1, timeout));

            var responses = await Task.WhenAll(tasks);
            return new Tuple<ScrimResponse, ScrimResponse>(responses[0], responses[1]);

        }

        private async Task<ScrimResponse> PromtCaptain(Dependecies d, MatchMakingTeam m1, MatchMakingTeam m2, int timeout)
        {
                     
            DateTime RequestTime = DateTime.Now;
              
            var Channel = await d.GetDMChannelAsync(m1.T.TeamCaptain);
            
            await d.Client.SendMessageAsync(Channel, "You have been matched with " + m2.T.TeamName + " at " + m2.Dt.ToString() + ". Do you accept?");
            while(true)
            {
                var t = timeout - (DateTime.Now - RequestTime).TotalSeconds;
                if(t <= 0)
                {
                    return ScrimResponse.NoResponse;
                }
                var message = await d.Client.GetInteractivity().WaitForMessageAsync(m => m.Author.Id == m1.T.TeamCaptain.Id && m.ChannelId == Channel.Id, TimeSpan.FromSeconds(t));
                if(message.TimedOut)
                {
                    return ScrimResponse.NoResponse;
                }
                if(message.Result.Content.ToLower() == "yes")
                {
                    return ScrimResponse.Accept;
                }
                if(message.Result.Content.ToLower() == "no")
                {
                    return ScrimResponse.Decline;
                }
                await d.Client.SendMessageAsync(Channel, "Please respond with either 'yes' or 'no'.");

            }
           
           
            //If timeout, return false
            
        }
    }
}