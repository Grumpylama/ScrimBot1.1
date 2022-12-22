using System;
using DSharpPlus;
using DSharpPlus.EventArgs;
using System.Collections.Generic;
using DSharpPlus.Entities;


namespace big
{
    public class Conversation
    {
        public Bot bot;
        protected int step;
        public int ConversationID;
        private static int ConversationIDCounter = 0; 
        public ulong UserID;  
        public  DiscordChannel Channel;
        protected List<String> Script = new List<String>();
        

        public Conversation(ulong UserID, DiscordChannel Channel)
        {
            this.Channel = Channel;
            this.UserID = UserID;
            this.ConversationID = ConversationIDCounter;
            ConversationIDCounter++;
        }

        protected virtual void FinishConversation()
        {
            //Remove this conversation from the list of conversations
            Bot.Conversations.Remove(this);
        }

        public virtual void NextStep(MessageCreateEventArgs e)
        {
            
        }

        public virtual async void AbortConversation()
        {
            await bot.Client.SendMessageAsync(Channel, "Conversation Aborted");
            //Remove this conversation from the list of conversations
            Bot.Conversations.Remove(this);
        }

    }
    
    public class CreateTeamConversation : Conversation
    {
        public CreateTeamConversation(ulong UserID, DiscordChannel channel) : base(UserID, channel)
        {
            this.Script = new List<String>();
            string s = "What game will you be playing?";
            int i = 1;
            foreach(Game game in Game.Games)
            {
                s += "\n" + i + ": " + game.GameName;
                i++;
            }
            Script.Add(s);
            
        }
        public override void NextStep(MessageCreateEventArgs e)
        {
            switch(step)
            {
                case 0:
                    //Send the first message
                    Step1(e);           
                    break;
                case 1:
                    
                    break;
                default:
                    break;
            }
        }

        private void Step1(MessageCreateEventArgs e)
        {
            

        }

    }
}