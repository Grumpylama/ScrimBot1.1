using System;

namespace big
{
    public class MMAlgos
    {

        private static readonly string FilePath = "MMAlgos.cs";
        private int SecondstillStart(DateTime dt)
        {
            return (int)(dt - DateTime.Now).TotalSeconds;
        }
    }
}