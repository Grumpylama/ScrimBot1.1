namespace big
{
    public class BotConfigFactory : IBotConfigFactory
    {
        public void CreateConfig(string BotToken, string prefix, string fullkey, string IV, ICrypto crypto)
        {
            var config = new ConfigJson
            {
                Token = crypto.Encrypt(BotToken, fullkey, IV),
                Success = crypto.Encrypt("success", fullkey, IV),
                Prefix = prefix,
                IV = IV
            };
            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText("Config.Json", json);
        }
    }
}