namespace big
{
    public static class StandardLogging
    {

        private static readonly string FilePath = "StandardLogging.cs";

        private static readonly Serilog.Core.Logger logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();


        public static void LogInfo(string filepath, string message)
        {
            logger.Information("[" + filepath + "] " + message);
        }
        
        public static void LogWarning(string filepath, string message)
        {
            logger.Warning("[" + filepath + "] " + message);
        }

        public static void LogError(string filepath, string message)
        {
            logger.Error("[" + filepath + "] " + message);
        }

        public static void LogFatal(string filepath, string message)
        {
            logger.Fatal("[" + filepath + "] " + message);
        }

        public static void LogDebug(string filepath, string message)
        {
            logger.Debug("[" + filepath + "] " + message);
        }

        public static void LogVerbose(string filepath, string message)
        {
            logger.Verbose("[" + filepath + "] " + message);
        }


    }
}