using System;
namespace big
{
    //Class for teams that are matchmaking, puts
    //them into queue for matchmaking
    public class MatchMakingTeam
    {
        public EDate Dt;
        public Team T;
        public bool Active = true;
        public bool hasActiveRequest = false;
        List<MatchMakingTeam> HasDeclined = new List<MatchMakingTeam>();

        public MatchMakingTeam(EDate dt, Team t)
        {
            this.Dt = dt;
            this.T = t;
        }

        public void setInactive()
        {
            Active = false;
        }

        public void setActive()
        {
            Active = true;
        }
    }
}