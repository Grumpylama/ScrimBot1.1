using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace big
{
    public class MatchMakingConfigExtracor
    {
        
        public static MatchMakingConfigExtracor Instance => _instance.Value;

        private static readonly Lazy<MatchMakingConfigExtracor> _instance = new Lazy<MatchMakingConfigExtracor>(() => new MatchMakingConfigExtracor());

        

        public MatchMakingConfigExtracor()
        {
            
            
            
        }


        //Takes in a filepath to the matchmakingpoolconfig and creates a matchmakingpoolconfig object
        //Using the jsonfile and 
        public async Task<MatchMakingPoolConfig> ExtractMatchMakingPoolConfig(string filePath)
        {

            StandardLogging.LogInfo("MatchMakingConfigExtractor.cs", "Extracting Config");
            var json = String.Empty;

            try
            {
                using (var fs = File.OpenRead(filePath))
                using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                    json = await sr.ReadToEndAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                StandardLogging.LogError("MatchMakingConfigExtractor.cs", $"Error while extracting config: {e.Message}");
            }
            

            StandardLogging.LogInfo("MatchMakingConfigExtractor.cs", "Config Extracted");
            StandardLogging.LogInfo("MatchMakingConfigExtractor.cs", "Deserializing Config");
            MatchMakingPoolConfig configJson = new MatchMakingPoolConfig();
            try
            {
                configJson = JsonConvert.DeserializeObject<MatchMakingPoolConfig>(json);
                StandardLogging.LogInfo("MatchMakingConfigExtractor.cs", "Config Deserialized");
            }
            catch
            {
                StandardLogging.LogError("MatchMakingConfigExtractor.cs", "Error while deserializing config");
            }
            
            StandardLogging.LogInfo("MatchMakingConfigExtractor.cs", json);
            
            return configJson;
            

        }
        

    }
}