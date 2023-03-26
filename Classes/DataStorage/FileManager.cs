namespace big
{
    public class FileManager
    {
        public static DateTime LastSave;

        public static string FilePath ="FileManager.cs";
        
        
        public static async Task StartUpAsync()
        {
            string startpath = Environment.CurrentDirectory;

            ITextProcessor textProcessor = new EncryptedGenericFileProcessor(new AesCrypto());
            
            StandardLogging.LogInfo(FilePath, "Starting File Manager");
            StandardLogging.LogInfo(FilePath, "startpath is : " + startpath);

            
            

            List<Task> tasks = new List<Task>();
            
            
            //Starts userloading
            try
            {
                tasks.Add(StandardUserHandling.LoadUsersAsync(textProcessor.LoadFromTextFile<SaveableUser>(startpath + "/Data/Users.csv")));

            }
            catch(Exception e) 
            {
                
                StandardLogging.LogError(FilePath, "Error Loading Users");
                StandardLogging.LogError(FilePath, e.Message);
            }
           
            
            //tasks.Add(Dependecies.LoadTeams(GenericTextFileProcessor.LoadFromTextFile<SaveableTeam>(startpath + "/Data/Teams.csv")));


            GameHandler.Games.Add(new Game("Overwatch 2", null));
            GameHandler.Games.Add(new Game("Leauge Of Legends", null));
            GameHandler.Games.Add(new Game("Dota 2", null));
            GameHandler.Games.Add(new Game("Valorant", null));


            //Wait for all tasks to finish before returning
            await Task.WhenAll(tasks);
            List<SaveableTeam> SvTs = new List<SaveableTeam>();
            try
            {
                SvTs = textProcessor.LoadFromTextFile<SaveableTeam>(startpath + "/Data/Teams.csv");
            }
            catch(Exception e)
            {
                
                StandardLogging.LogError(FilePath, "Error Loading Teams");
                StandardLogging.LogError(FilePath, e.Message);
            }
            try 
            {
                foreach (var team in SvTs)
                {   
                    TeamHandler.Teams.Add(team.ToTeam());
                }
            }
            catch(Exception e)
            {
                
                StandardLogging.LogError(FilePath, "Error Converting Teams");
                StandardLogging.LogError(FilePath, e.Message);
            }
            

            List<SavableTeamUser> SvTUs = new List<SavableTeamUser>();
            try
            {
                SvTUs = textProcessor.LoadFromTextFile<SavableTeamUser>(startpath + "/Data/TeamUsers.csv");
            }
            catch(Exception e)
            {
                
                StandardLogging.LogError(FilePath, "Error Loading TeamUsers");
                StandardLogging.LogError(FilePath, e.Message);
            }
            try
            {
                StandardLogging.LogInfo(FilePath, "Converting " +  SvTUs.Count + " TeamUsers");
                
                foreach (var user in SvTUs)
                {
                    
                    StandardLogging.LogInfo(FilePath, "Trying to add " + user.UserID + " to " + TeamHandler.GetTeamFromID(user.TeamID).TeamName);
                    TeamHandler.GetTeamFromID(user.TeamID).TeamMembers.Add(user.ToTeamUser());
                    
                }
                
                StandardLogging.LogInfo(FilePath, "Done Converting TeamUsers");
                
            }
            catch(Exception e)
            {
                StandardLogging.LogError(FilePath, "Error Converting TeamUsers");
                StandardLogging.LogError(FilePath, e.Message);
            }

            try 
            {
                StandardLogging.LogInfo(FilePath, "Adding admins");

                DiscordInterface.AdminList.Add(StandardUserHandling.GetUserFromID(244135683537502208));
                DiscordInterface.AdminList.Add(StandardUserHandling.GetUserFromID(214158487712694273));

            }
        
            catch(Exception e)
            {
                StandardLogging.LogError(FilePath, "Error adding admins");
                StandardLogging.LogError(FilePath, e.Message);
            }


            //Set Static ID's
            Team.SetStaticID(TeamHandler.Teams.Select(x => x.teamID).Max() + 1);
            TeamHandler.ForceUpdateCaptainDMChannels();
            
            return;

            

        }
        public static void SaveAll()
        {
            
            string startpath = Environment.CurrentDirectory + "\\Data";
            StandardLogging.LogInfo(FilePath, "saving all files with startpath: " + startpath);
            ITextProcessor textProcessor = new EncryptedGenericFileProcessor(new AesCrypto());
            

            
            //Save All files
            //Saving Users
            List<SaveableUser> saveableUsers = new List<SaveableUser>();
            foreach (var u in StandardUserHandling.Users)
            {
                saveableUsers.Add(u.ToSavableUser());
            }
            List<SaveableTeam> saveableTeams = new List<SaveableTeam>();
            foreach (var t in TeamHandler.Teams)
            {
                saveableTeams.Add(t.ToSavable());
            }

            List<SavableTeamUser> savableTeamUsers = new List<SavableTeamUser>();
            foreach (Team t in TeamHandler.Teams)
            {
                foreach (TeamUser tu in t.TeamMembers)
                {
                    savableTeamUsers.Add(tu.ToSavable());
                }
            }
            textProcessor.SaveToTextFile<SavableTeamUser>(savableTeamUsers, startpath + "/TeamUsers.csv");
            textProcessor.SaveToTextFile<SaveableTeam>(saveableTeams, startpath + "/Teams.csv");
            textProcessor.SaveToTextFile<SaveableUser>(saveableUsers, startpath + "/Users.csv");

            LastSave = DateTime.Now;



        }

        public static void SetUpTimerSave(int interval)
        {
            //Registering the save timer
            var saveTimer = new System.Timers.Timer(interval); 
            saveTimer.Elapsed += TimerSave;
            saveTimer.Start();
            StandardLogging.LogInfo(FilePath, "Save timer started");
        }

        private static void TimerSave(object sender, ElapsedEventArgs e)
        {
            
            StandardLogging.LogInfo(FilePath, "Timersave, Saving data");
            FileManager.SaveAll();
        }
    }
}