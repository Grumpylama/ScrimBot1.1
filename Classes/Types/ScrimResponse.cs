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
    }
}