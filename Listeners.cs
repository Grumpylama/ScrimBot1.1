using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;

namespace big
{
    public partial class Bot
    {

        public static List<Conversation> Conversations = new List<Conversation>();
        private async Task MessageCreatedHandler(DiscordClient s, MessageCreateEventArgs e)
        {

            Console.WriteLine(e.Message.Content);

            //Checks if the message is from a bot
            if (e.Author.IsBot) return;
            

            //Checks if the message is a command
            //If it is, it returns and does not continue
            //Commands will sort it out
            if (IsCommand(e))
            {
                return;
            }

            //Check if the user is in a conversation
            Conversation c = GetCurrentConversation(e.Author.Id, e);
            if(c == null)
            {
                return;
            }

            c.NextStep(e);
            
            
        }


        //Helper Methods
        private bool IsCommand(MessageCreateEventArgs e)
        {
            if (e.Message.Content.StartsWith("!"))
            {
                return true;
            }
            return false;
        }

        
        private Conversation GetCurrentConversation(ulong UserID, MessageCreateEventArgs e)
        {
            foreach (Conversation conversation in Conversations)
            {
                if (conversation.UserID == UserID && conversation.Channel.Id == e.Channel.Id)
                {
                    return conversation;
                }
            }
            return null;
        }

    }
}