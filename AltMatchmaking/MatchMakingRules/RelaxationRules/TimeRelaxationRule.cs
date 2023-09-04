using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace big
{
    public class TimeRelaxationRule : IRelaxationRule
    {
        private static readonly string FilePath = "TimeRelaxationRule.cs";

        private Dictionary<int, int> parameters {get; set;}


        List<int> timeLeftToRelax = new List<int>();

        public TimeRelaxationRule(Dictionary<string, string> parameters)
        {
            this.parameters = new Dictionary<int, int>();
            foreach (var parameter in parameters)
            {
                try
                {
                    this.parameters.Add(int.Parse(parameter.Key), int.Parse(parameter.Value));
                }
                catch
                {
                    StandardLogging.LogFatal(FilePath, "Error parsing parameters! Check your config file!");
                }
                
            }
        }
        
       

        public int GetRelaxationStage(MatchmakingContext context)
        {
            return 0;
        }


    }
}