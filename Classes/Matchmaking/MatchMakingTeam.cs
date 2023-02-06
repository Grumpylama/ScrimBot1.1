using System;
namespace big
{
    //Class for teams that are matchmaking, puts
    //them into queue for matchmaking
    public class MatchMakingTeam
    {

        private static readonly string FilePath = "MatchMakingTeam.cs";
        public bool Dummy = false;
        public EDate Dt;
        public Team T;
        public bool Active = true;
        public bool hasActiveRequest = false;
        List<MatchMakingTeam> HasAvoided = new List<MatchMakingTeam>();

        public MatchMakingTeam(EDate dt, Team t)
        {
            this.Dt = dt;
            this.T = t;
        }
        public void addAvoid(MatchMakingTeam mmt)
        {
            HasAvoided.Add(mmt);
        }
        public MatchMakingTeam()
        {
            this.Dt = new EDate();
            this.T = new Team();
        }

        public void setInactive()
        {
            Active = false;
        }

        public void setActive()
        {
            Active = true;
        }

        

        public async Task<bool> DMCaptainAsync(string message)
        {
            if(await T.DMCaptainAsync(message))
                return true;
           
            return false;
        }

        public async Task<DSharpPlus.Interactivity.InteractivityResult<DSharpPlus.Entities.DiscordMessage>> ListenToCaptainAsync(double timeout = 0)
        {
            return await T.ListenToCaptainAsync(timeout);
        }

        public async Task<ScrimResponse> PromtCaptainForScrimAsync(MatchMakingTeam opponent, double timeout)
        {
            DiscordClient client = DiscordInterface.Client;
            DateTime requestTime = DateTime.Now;

            await DMCaptainAsync("You have been matched with " + opponent.T.TeamName + " for a scrim. Do you accept? (yes/no)");
            while(true)
            {
                var t = timeout - (DateTime.Now - requestTime).TotalSeconds;

                if (t <= 0)
                {
                    await DMCaptainAsync("You did not respond in time, you have been removed from the queue");
                    return new ScrimResponse(ScrimResponseCode.NoResponse, this);
                }

                var message = await ListenToCaptainAsync(t);
                if(message.TimedOut)
                {
                    await DMCaptainAsync("You did not respond in time, you have been removed from the queue");
                    return new ScrimResponse(ScrimResponseCode.NoResponse, this);
                }
                if(message.Result.Content.ToLower() == "yes")
                {
                    await DMCaptainAsync("You have accepted the scrim");
                    return new ScrimResponse(ScrimResponseCode.Accept , this);
                }
                else if(message.Result.Content.ToLower() == "no")
                {
                    await DMCaptainAsync("You have declined the scrim");
                    return new ScrimResponse(ScrimResponseCode.Decline, this);
                }
                else
                {
                    await DMCaptainAsync("Invalid response, please respond with yes or no");
                }

                
            }
            
        } 

        public void VarDump()
        {
            StandardLogging.LogInfo(FilePath, "Team" + T.TeamName + "matchmade at " + Dt.Date + 
            ". Has active request: " + hasActiveRequest + 
            ". Is active: " + Active);
        }
    }
}