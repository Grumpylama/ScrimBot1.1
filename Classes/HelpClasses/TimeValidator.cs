using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrimBot1._1.Classes.HelpClasses
{
    public static class TimeValidator
    {

        //Checks to see if the given time is a valid time for a match
        //Valid times are times ending with 00
        public static bool ValidateTime(DateTime time)
        {
            if (time.Minute == 0)
            {
                return true;
            }
            return false;
        }

        //Rounds to the closest whole hour
        public static DateTime GetNextValidTime(DateTime time)
        {
            if (time.Minute == 0)
            {
                return time;
            }
            else if(time.Minute < 30)
            {
                return time.AddMinutes(-time.Minute);
            }
            else if(time.Minute > 30)
            {
                return time.AddHours(1).AddMinutes(-time.Minute);
            }
            else if(time.Minute == 30)
            {
                return time.AddMinutes(-time.Minute);
            }
            else
            {
                return time.AddHours(1).AddMinutes(-time.Minute);
            }
            
        }
        
    }
}