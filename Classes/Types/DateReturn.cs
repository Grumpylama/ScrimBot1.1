namespace big
{
    public struct EDate
    {
        public DateTime Date { get; private set; }

        public bool ASAP { get; private set; }
        public EDate(DateTime date, bool asap)
        {
            Date = date;
            ASAP = asap;
        }
        
    }
    
}