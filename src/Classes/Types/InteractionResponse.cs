using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrimBot1._1.src.Classes.Types
{
    public class InteractionResponse<T>
    {
        public T ResponseItem { get; private set;}

        public bool Success { get 
        {
            return InteractionOutcome == InteractionOutcome.Success;
        }}

        public InteractionOutcome InteractionOutcome { get; private set;}

        public InteractionResponse(T responseItem, InteractionOutcome outcome)
        {
            ResponseItem = responseItem;
            InteractionOutcome = outcome;
        }
    }

    public enum InteractionOutcome
    {
        Success = 0,
        Cancelled = 1,
        InvalidInput = 2,
        Error = 3
    }
}