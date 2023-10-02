using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace big
{
    public class AvoidedMatchMakingRule : IMatchMakingRule
    {
        private static readonly string FilePath = "AvoidedMatchMakingRule.cs";


        public List<IRelaxationRule> relaxationRules { get; set; }

        public AvoidedMatchMakingRule(List<IRelaxationRule> relaxationRules)
        {
            this.relaxationRules = relaxationRules;

        }
        
        public bool Evaluate(MatchmakingContext context)
        {
            StandardLogging.LogDebug(FilePath, "Evaluating AvoidedMatchMakingRule with the teams " +  string.Join(", ", context.Tickets.Select(t => t.team)));
            bool needToCheck = false;
            foreach(MatchmakingTicket ticket in context.Tickets)
            {
                StandardLogging.LogDebug(FilePath, "Checking if " + ticket.team + " is avoided");
                if(context.Tickets.Any<MatchmakingTicket>(t => t.avoidedTeams.Contains(ticket.team)))
                {
                    needToCheck = true;
                    StandardLogging.LogDebug(FilePath, ticket.team + " is avoided by the team " + context.Tickets.First<MatchmakingTicket>(t => t.avoidedTeams.Contains(ticket.team)).team + ". Checking relaxation rules");
                    break;
                }
            }
            if (!needToCheck)
            {
                StandardLogging.LogDebug(FilePath, "No teams are avoided. Returning true");
                return true;
            }

            foreach (IRelaxationRule rule in relaxationRules)
            {

                if (rule.GetRelaxationStage(context) == 0)
                {
                    StandardLogging.LogDebug(FilePath, "Relaxation rule " + rule.GetType().Name + " returned 0. Returning true");
                    return true;
                }
                
                else return false;
            }
            StandardLogging.LogError(FilePath, "Invalid avoidedMatchmakingRule. No relaxation rules. Please your config file");
            return true;
        }



    }
}