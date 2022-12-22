using System;
namespace big
{
    //Class for teams that are matchmaking, puts
    //them into queue for matchmaking
    public class MatchMakingTeam
    {
        public DateTime dt;
        public Team t;

        public MatchMakingTeam(DateTime dt, Team t)
        {
            this.dt = dt;
            this.t = t;
        }
    }
}