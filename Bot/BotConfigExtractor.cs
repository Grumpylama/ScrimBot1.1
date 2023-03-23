namespace big
{
    public class BotConfigExtractor : IBotConfigExtractor
    {
        private static readonly string filename = "BotconifgExtractor.cs";
        public async Task<ConfigJson> ExtractConfigAsync(IAdminInterface adminInterface)
        {
            StandardLogging.LogInfo(filename, "Extracting Config");
            var json = String.Empty;
            try{
                
                using (var fs = File.OpenRead("Config.json"))            
                using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                    json = await sr.ReadToEndAsync().ConfigureAwait(false);
            }   
            catch(Exception e)
            {
                StandardLogging.LogFatal(filename, $"Error while extracting config: {e.Message}");
                Environment.Exit(1);
            }
            
            StandardLogging.LogInfo(filename, "Config Extracted");
            StandardLogging.LogInfo(filename, "Deserializing Config");
            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);
            StandardLogging.LogInfo(filename, "Config Deserialized");
            StandardLogging.LogInfo(filename, json);

            StandardLogging.LogInfo(filename, "Decrypting Config");
            try
            {
                string key = adminInterface.PromtKey();
                string masterPassword = adminInterface.PromtMasterPassword();
                

                ICrypto crypto = new AesCrypto();
                IKeyGenerator keyGenerator = new StandardKeyGenerator();

                string fullkey = keyGenerator.GenerateFullKey(masterPassword, key);
                EncryptedGenericFileProcessor.crypto.SetIV(configJson.IV);
                EncryptedGenericFileProcessor.crypto.SetKey(fullkey);


                configJson.Token = crypto.Decrypt(configJson.Token, fullkey, configJson.IV);
                configJson.Success = crypto.Decrypt(configJson.Success, fullkey, configJson.IV);
            }
            catch
            {
                StandardLogging.LogFatal(filename, "Error while decrypting config");
                Environment.Exit(1);
            }
            


            return configJson;
            

            
        }

        
    }
}