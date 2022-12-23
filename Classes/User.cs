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
       

        public UserHash(DiscordUser user, string hash)
        {
            this.user = user;
            this.hash = hash;           
        }   
    }
     
    
}