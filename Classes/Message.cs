namespace big
{

    //Message Class Only used by conversation class used to keep track of what
    //should be sent and what should be recieved
    //Might not be needed LOL
    public class Message
    {
        public int MessageID;
        private static int MessageIDCounter = 0;

        public String MessageContent;

        public Message(int UserID, String MessageContent){
            this.MessageContent = MessageContent;
            this.MessageID = MessageIDCounter;
            MessageIDCounter++;
        }
    }
}