namespace big
{
    public enum ScrimResponseCode
    {

        Accept, 
        Decline,
        NoResponse
    }

    public struct ScrimResponse
    {
        public ScrimResponseCode Code;
        public MatchMakingTeam T;

        public ScrimResponse(ScrimResponseCode code, MatchMakingTeam t)
        {
            Code = code;
            T = t;
        }
    }
}