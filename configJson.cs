
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

public struct ConfigJson
{

    
    //Will be encrypted and decrypted
    //If decryption is successful this string will be "success"
    [JsonProperty("success")]
    public string Success {get; set;}

    [JsonProperty("IV")]
    public string IV {get; set;}

    [JsonProperty("token")]
    public string Token {get; set;}
    [JsonProperty("prefix")]
    public string Prefix {get; set;}
}