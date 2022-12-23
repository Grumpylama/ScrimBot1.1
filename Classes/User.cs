using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using DSharpPlus.Entities;

namespace big
{
    public struct UserHash
    {
        public DiscordUser user;
        public string hash;

        public DiscordChannel channel;
       

        public UserHash(DiscordUser user, string hash, DiscordChannel channel)
        {
            this.user = user;
            this.hash = hash;  
            this.channel = channel;         
        }   
    }
     
    
}