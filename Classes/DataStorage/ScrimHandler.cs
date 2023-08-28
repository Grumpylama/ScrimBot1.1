namespace big
{
    public static class ScrimHandler
    {

        private static readonly string savepath = Environment.CurrentDirectory + "\\Data\\scrims.csv";
        private static readonly string FilePath = "ScrimHandler.cs";
        
        public static List<Scrim> ActiveScrims = new List<Scrim>();

        public static List<Scrim> Scrims = new List<Scrim>();

        public static void RegisterScrim(Scrim scrim)
        {
            Scrims.Add(scrim);
        }


        public static async Task MoveScrimsToDiskAsync()
        {
            ITextProcessor textProcessor = new GenericTextFileProcessor();

            StandardLogging.LogInfo(FilePath, "Moving scrims to disk");
            var scrimsToMove = ActiveScrims.FindAll
            (x => (x.Finished == true && x.Date < DateTime.Now.AddHours(-4)) || (x.Date < DateTime.Now.AddDays(-1)))
            .Select(x => x.ToSavable()).ToList();

            foreach (var scrim in scrimsToMove)
            {
                ActiveScrims.Remove(ActiveScrims.Find(x => x.ID == scrim.ID)!);
            }

            textProcessor.SaveToTextFile<SavableScrim>(scrimsToMove, savepath);


            

            
        }

        public static async Task ForceScrimsToDisk()
        {
            ITextProcessor textProcessor = new GenericTextFileProcessor();
            StandardLogging.LogInfo(FilePath, "Moving scrims to disk");
            var scrimsToMove = ActiveScrims.Select(x => x.ToSavable()).ToList();

            textProcessor.SaveToTextFile<SavableScrim>(scrimsToMove, savepath);
        }





        public static void VarDump()
        {
            foreach (var scrim in ActiveScrims)
            {
                StandardLogging.LogInfo(FilePath, "Scrim: " + scrim.Team1.TeamName + " vs " + scrim.Team2.TeamName + " with game " + scrim.Game.GameName + " and date " + scrim.Date);
            }
        }

    }
}