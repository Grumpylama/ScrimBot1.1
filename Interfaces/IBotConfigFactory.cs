namespace Interfaces
{
    public interface IBotConfigFactory
    {
        public void CreateConfig(string BotToken, string prefix, string fullkey, string IV, ICrypto crypto);
    }
}