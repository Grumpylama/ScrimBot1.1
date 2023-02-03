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

        public async Task<ScrimResponse> PromtCaptainForScrimAsync(MatchMakingTeam opponent)
        {
            throw new NotImplementedException();
        } 

        public void VarDump()
        {
            StandardLogging.LogInfo(FilePath, "Team" + T.TeamName + "matchmade at " + Dt.Date + 
            ". Has active request: " + hasActiveRequest + 
            ". Is active: " + Active);
        }
    }
}