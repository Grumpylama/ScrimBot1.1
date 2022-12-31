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
        ScrimResponseCode Code;
        Team T;

        public ScrimResponse(ScrimResponseCode code, Team t)
        {
            Code = code;
            T = t;
        }
    }
}