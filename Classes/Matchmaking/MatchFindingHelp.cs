namespace big
{
    public class MatchFindingHelp
    {
        //When there is 30 minutes left to matchStart
        public void StressFindMatch()
        {

        }

        //When there are 2 hours left to matchStart
        public async Task<bool> FindMatchAsync( MatchMaker m)
        {
            if(m.MMTList.Count > 1) 
            {
                await MatchFindHelperAsync(m);
                return true;
            }
            return false;
        }
        //How long do we wait for the answer? D:
        private async Task MatchFindHelperAsync( MatchMaker m)
        {
            List<Task<Tuple<ScrimResponse, ScrimResponse>>> tasks = new List<Task<Tuple<ScrimResponse, ScrimResponse>>>();
            foreach(MatchMakingTeam mmt in m.MMTList)
            {
                int timeout = 300;
                if(mmt.hasActiveRequest == false && mmt.Active == true)
                {
                    //Finds best match for first mmt in list and sends a promt to it and the other team
                    //tasks.Add(PromtCaptains(d, mmt, Sort.FindBestMatch(mmt, m.MMTList), timeout));       
                }                               
            }

            //Waits for when all promts are done.
            //Will be within the timeout time limit or sooner
            var responses = await Task.WhenAll(tasks);

            foreach(var r in responses)
            {
                switch(r.Item1.Code, r.Item2.Code)
                {
                    case (ScrimResponseCode.NoResponse, ScrimResponseCode.NoResponse):
                        //If both teams don't answer
                        //Set both teams as inactive
                        m.MMTList.Find(x => x == r.Item1.T).setInactive();
                        m.MMTList.Find(x => x == r.Item2.T).setInactive();
                        m.MMTList.Find(x => x == r.Item1.T).hasActiveRequest = false;
                        m.MMTList.Find(x => x == r.Item2.T).hasActiveRequest = false;
                        break;
                    case (ScrimResponseCode.NoResponse, ScrimResponseCode):
                        //If the first team doesn't answer
                        //Set the first team as inactive
                        m.MMTList.Find(x => x == r.Item1.T).setInactive();
                        m.MMTList.Find(x => x == r.Item1.T).hasActiveRequest = false;
                        m.MMTList.Find(x => x == r.Item2.T).hasActiveRequest = false;
                        
                        break;
                    case (ScrimResponseCode, ScrimResponseCode.NoResponse):
                        //If the second team doesn't answer
                        //Set the second team as inactive
                        m.MMTList.Find(x => x == r.Item2.T).setInactive();
                        m.MMTList.Find(x => x == r.Item1.T).hasActiveRequest = false;
                        m.MMTList.Find(x => x == r.Item2.T).hasActiveRequest = false;
                        break;
                    case (ScrimResponseCode.Accept, ScrimResponseCode.Accept):
                        //If both teams accept
                        //Create a match
                        //Set both teams as inactive
                        m.MMTList.Find(x => x == r.Item1.T).setInactive();
                        m.MMTList.Find(x => x == r.Item2.T).setInactive();
                        
                        break;
                    
                    case (ScrimResponseCode.Decline, ScrimResponseCode.Accept):
                        //If the first team declines and the second team accepts
                        //add the second team to the first team's avoid list
                        m.MMTList.Find(x => x == r.Item1.T).addAvoid(m.MMTList.Find(x => x == r.Item2.T));
                        break;
                    case(ScrimResponseCode.Accept, ScrimResponseCode.Decline):
                        //If the first team accepts and the second team declines
                        
                        //Add the first team to the second team's avoid list

                        m.MMTList.Find(x => x == r.Item2.T).addAvoid(m.MMTList.Find(x => x == r.Item1.T));
                        break;
                }
            }
                /*

                Quite sure this code is obsolete

                
                if(r.Item1.T.Dummy == true)
                {
                    m.MMTList.Remove(r.Item1.T);
                }
                if(r.Item2.T.Dummy == true)
                {
                    m.MMTList.Remove(r.Item2.T);
                }
                if(r.Item1.Code == ScrimResponseCode.NoResponse)
                {
                    //If the first team doesn't answer
                    //Set the first team as inactive
                    m.MMTList.Find(x => x == r.Item1.T).setInactive();
                    if(r.Item2.Code == ScrimResponseCode.NoResponse)
                    {
                        //If the second team doesn't answer
                        //Set the second team as inactive
                        m.MMTList.Find(x => x == r.Item2.T).setInactive();
                    }                    
                    continue;
                }
                if(r.Item1.Code == ScrimResponseCode.NoResponse)
                {
                    //If the first team doesn't answer
                    //Set the first team as inactive
                    m.MMTList.Find(x => x == r.Item1.T).setInactive();
                    if(r.Item2.Code == ScrimResponseCode.NoResponse)
                    {
                        //If the second team doesn't answer
                        //Set the second team as inactive
                        m.MMTList.Find(x => x == r.Item2.T).setInactive();
                    }                    
                    continue;
                }

            }
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
            if(Answer.Item1 == ScrimResponseCode.Decline)//3 declines fixa
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


        private async Task<Tuple<ScrimResponse, ScrimResponse>> PromtCaptains(MatchMakingTeam m1, MatchMakingTeam m2, int timeout)
        {
            
            List<Task<ScrimResponse>> tasks = new List<Task<ScrimResponse>>();
            tasks.Add(PromtCaptain(d, m1, m2, timeout));
            tasks.Add(PromtCaptain(d, m2, m1, timeout));

            var responses = await Task.WhenAll(tasks);
            return new Tuple<ScrimResponse, ScrimResponse>(responses[0], responses[1]);

        }
        
        private async Task<ScrimResponse> PromtCaptain(MatchMakingTeam m1, MatchMakingTeam m2, int timeout)
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
        */
        }
    }
}