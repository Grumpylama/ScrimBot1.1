using System;

namespace big
{
    public class MMAlgos
    {
        private int SecondstillStart(DateTime dt)
        {
            return (int)(dt - DateTime.Now).TotalSeconds;
        }
    }
}