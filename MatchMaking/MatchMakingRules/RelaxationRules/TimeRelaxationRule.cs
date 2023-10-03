using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace big
{
    public class TimeRelaxationRule : IRelaxationRule
    {


        private Dictionary<int, double> parameters {get; set;}


        

        public TimeRelaxationRule(Dictionary<int, double> parameters)
        {
            this.parameters = parameters;
                
        }
        public double GetRelaxationStage(MatchmakingContext context)
        {
            List<int> ApplicableParamters = new List<int>();
          
            foreach(var parameter in parameters)
            {
                if(parameter.Key > context.minutesToStart)
                {
                    ApplicableParamters.Add(parameter.Key);
                }
            }
            if (ApplicableParamters.Count == 0)
            {
                return 1;
            }
            return parameters[ApplicableParamters.Min()];
        }


    }
}