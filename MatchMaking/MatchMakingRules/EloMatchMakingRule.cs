using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace big
{
    public class EloMatchMakingRule : IMatchMakingRule
    {
        
        
        private double StandardValue { get; set;}

        private List<IRelaxationRule> RelaxationsRules;

        public EloMatchMakingRule(int standardValue, List<IRelaxationRule> relaxationsRules)
        {
            this.StandardValue = standardValue;
            this.RelaxationsRules = relaxationsRules;
        }



        //Evaluates if the teams in the matchmakingcontext are within allowed elo range
        public bool Evaluate(MatchmakingContext context) 
        {
            //Get the average elo of the teams
            double averageElo = context.Tickets.Average(ticket => ticket.Elo);
            //Get the difference between the average elo and the standard value
            double difference = Math.Abs(averageElo - StandardValue);
            //Get the relaxation stage
            double relaxationStage = RelaxationsRules.Select(rule => rule.GetRelaxationStage(context)).Max();
            //Get the allowed difference
            double allowedDifference = StandardValue * relaxationStage;
            //Return if the difference is within the allowed difference
            return difference <= allowedDifference;   
        }

        
        
    }
}