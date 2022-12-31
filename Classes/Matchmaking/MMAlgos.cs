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
                await MatchFindHelperAsync(d, m);
                return true;
            }
            return false;
        }
        //How long do we wait for the answer? D:
        private async Task MatchFindHelperAsync(Dependecies d, MatchMaker m)
        {
            List<Task<ScrimResponse>> tasks = new List<Task<ScrimResponse>>();
            foreach(MatchMakingTeam mmt in m.MMTList)
            {
                Tuple<int, int> indexes = Sort.FindLowestMMRDiff(m.MMTList);
                var Answer = PromtCaptains(d, m.MMTList[indexes.Item1], m.MMTList[indexes.Item2], 60);
                
            }

            m.MMTList[indexes.Item1].hasActiveRequest = true;
            m.MMTList[indexes.Item2].hasActiveRequest = true;

            //If [0] team doesn't answer or refuses,
            //Set temp as inactive
            //Replace [0] team with team 'index'
            //Call the function
            if(Answer.Item1 == ScrimResponse.NoResponse)
            {
                m.MMTList[indexes.Item1].setInactive();
                m.MMTList[indexes.Item1] = m.MMTList[indexes.Item2];
                m.MMTList.RemoveAt(indexes.Item2);
            }
            //If m.MMTList[i] doesn't answer or refuses
            //Remove it and call the function
            if(Answer.Item2 == ScrimResponse.NoResponse)
            {
                m.MMTList[indexes.Item2].setInactive();
                m.MMTList.RemoveAt(indexes.Item2);
            }
            //If neither answer or both refuse
            //Set both inactive AND remove both
            //Call function
            if(Answer.Item1 == ScrimResponse.Decline)//3 declines fixa
            {
                m.MMTList[indexes.Item1].setInactive();
                m.MMTList[indexes.Item1] = m.MMTList[indexes.Item2];
                m.MMTList.RemoveAt(indexes.Item2);
            }
            if(Answer.Item2 == ScrimResponse.Decline)//3 declines fixa
            {
                m.MMTList[indexes.Item2].setInactive();
                m.MMTList.RemoveAt(indexes.Item2);
            }
            if(Answer.Item1 == ScrimResponse.Accept && Answer.Item2 == ScrimResponse.Accept)
            {
                m.MMTList.RemoveAt(indexes.Item1);
                m.MMTList.RemoveAt(indexes.Item2);
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
                    return new ScrimResponse(ScrimResponseCode.Decline , m1.T);
                }
                var message = await d.Client.GetInteractivity().WaitForMessageAsync(m => m.Author.Id == m1.T.TeamCaptain.Id && m.ChannelId == Channel.Id, TimeSpan.FromSeconds(t));
                if(message.TimedOut)
                {
                    return new ScrimResponse(ScrimResponseCode.NoResponse , m1.T);
                }
                if(message.Result.Content.ToLower() == "yes")
                {
                    return new ScrimResponse(ScrimResponseCode.Accept , m1.T);
                }
                if(message.Result.Content.ToLower() == "no")
                {
                    return new ScrimResponse(ScrimResponseCode.Decline , m1.T);
                }
                await d.Client.SendMessageAsync(Channel, "Please respond with either 'yes' or 'no'.");

            }
           
           
            //If timeout, return false
            
        }
    }
}