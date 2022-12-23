
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
    [JsonProperty("token")]
    public string Token {get; private set;}
    [JsonProperty("prefix")]
    public string Prefix {get; private set;}
}