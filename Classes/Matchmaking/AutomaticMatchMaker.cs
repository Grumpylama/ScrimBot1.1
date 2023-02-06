namespace big
{
    public class AutomaticMatchMaker
    {

        private static readonly string FilePath = "MatchFindingHelp.cs";

        //When there are 2 hours left to matchStart
        //Less time left till match means less time to respond to the matchmaking
        //for the team captains
        public static async Task<bool> FindMatchAsync( MatchMaker m)
        {
                DateTime dt1 = DateTime.Now;
                DateTime dt2 = m.matchStart.Date;
                //Current time in milliseconds
                var t1 = dt1.Second * 1000;
                //2 hours is 7,200,000 milliseconds
                var t2 = (dt2.Second * 1000) - 7200000;
                //Wait until 2 hours before matchStart
                await Task.Delay(t2 - t1);
            if(m.MMTList.Count > 1) 
            {
                //Start the MatchMaking
                await MatchFindHelperAsync(m);
                return true;
            }
            return false;
        }
        
        private static async Task MatchFindHelperAsync( MatchMaker m)
        {
            List<Task<Tuple<ScrimResponse, ScrimResponse>>> tasks = new List<Task<Tuple<ScrimResponse, ScrimResponse>>>();

            int timeout = 300;
            m.findMatch(); //Make the function so it returns a list of scrimresponses to make tasks equal to it
            //Also make it so that it checks if the mmt is active and has activeRequest                   


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

                    
                        try 
                        {
                            m.MMTList.Find(x => x == r.Item1.T).setInactive();
                        }
                        catch
                        {
                            StandardLogging.LogError(FilePath, $"MatchFindHelperAsync {r.Item1.T} not found in matchmaking list. Could it have been removed manually?");
                        }
                        try
                        {
                            m.MMTList.Find(x => x == r.Item2.T).setInactive();
                        }
                        catch
                        {
                            StandardLogging.LogError(FilePath, $"MatchFindHelperAsync {r.Item2.T} not found in matchmaking list. Could it have been removed manually?");
                        }
                        try
                        {
                            m.MMTList.Find(x => x == r.Item1.T).hasActiveRequest = false;
                        }
                        catch
                        {
                            StandardLogging.LogError(FilePath, $"MatchFindHelperAsync {r.Item1.T} not found in matchmaking list. Could it have been removed manually?");
                        }
                        try
                        {
                            m.MMTList.Find(x => x == r.Item2.T).hasActiveRequest = false;
                        }
                        catch 
                        {
                            StandardLogging.LogError(FilePath, $"MatchFindHelperAsync {r.Item2.T} not found in matchmaking list. Could it have been removed manually?");
                        }
                        
                        break;


                    case (ScrimResponseCode.NoResponse, ScrimResponseCode):
                        //If the first team doesn't answer
                        //Set the first team as inactive
                        try
                        {
                            m.MMTList.Find(x => x == r.Item1.T).setInactive();
                        }
                        catch
                        {
                            StandardLogging.LogError(FilePath, $"MatchFindHelperAsync {r.Item1.T} not found in matchmaking list. Could it have been removed manually?");
                            try
                            {
                                m.MMTList.Find(x => x == r.Item2.T).hasActiveRequest = false;
                            }
                            catch 
                            {
                                StandardLogging.LogError(FilePath, $"MatchFindHelperAsync {r.Item2.T} not found in matchmaking list. Could it have been removed manually?");
                            }
                            
                        }
                        try
                        {
                            m.MMTList.Find(x => x == r.Item2.T).hasActiveRequest = false;
                        }
                        catch
                        {
                            StandardLogging.LogError(FilePath, $"MatchFindHelperAsync {r.Item2.T} not found in matchmaking list. Could it have been removed manually?");
                            try
                            {
                                m.MMTList.Find(x => x == r.Item1.T).hasActiveRequest = false;
                            }
                            catch
                            {
                                StandardLogging.LogError(FilePath, $"MatchFindHelperAsync {r.Item1.T} not found in matchmaking list. Could it have been removed manually?");
                            }
                            
                        }
                        try
                        {
                            m.MMTList.Find(x => x == r.Item1.T).hasActiveRequest = false;
                        }
                        catch
                        {
                            StandardLogging.LogError(FilePath, $"MatchFindHelperAsync {r.Item1.T} not found in matchmaking list. Could it have been removed manually?");
                        }

                        break;


                    case (ScrimResponseCode, ScrimResponseCode.NoResponse):
                        //If the second team doesn't answer
                        //Set the second team as inactive
                        try
                        {
                            m.MMTList.Find(x => x == r.Item2.T).setInactive();
                        }
                        catch
                        {
                            StandardLogging.LogError(FilePath, $"MatchFindHelperAsync {r.Item2.T} not found in matchmaking list. Could it have been removed manually?");
                            try
                            {
                                m.MMTList.Find(x => x == r.Item1.T).hasActiveRequest = false;
                            }
                            catch
                            {
                                StandardLogging.LogError(FilePath, $"MatchFindHelperAsync {r.Item1.T} not found in matchmaking list. Could it have been removed manually?");
                            }
                            
                        }
                        
                        try
                        {
                            m.MMTList.Find(x => x == r.Item1.T).hasActiveRequest = false;
                        }
                        catch
                        {
                            StandardLogging.LogError(FilePath, $"MatchFindHelperAsync {r.Item1.T} not found in matchmaking list. Could it have been removed manually?");
                        }
                        try
                        {
                            m.MMTList.Find(x => x == r.Item2.T).hasActiveRequest = false;
                        }
                        catch
                        {
                            StandardLogging.LogError(FilePath, $"MatchFindHelperAsync {r.Item2.T} not found in matchmaking list. Could it have been removed manually?");
                        }
                        
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