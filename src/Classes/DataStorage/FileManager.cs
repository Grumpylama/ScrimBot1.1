namespace big
{
    public class FileManager
    {
        public static DateTime LastSave;

        public static string FilePath ="FileManager.cs";
        
        
        public static async Task StartUpAsync()
        {
            string startpath = Environment.CurrentDirectory;

            ITextProcessor textProcessor = new GenericTextFileProcessor();
            
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

            StandardLogging.LogInfo(FilePath, "Initalzing SvATs");
            List<SavableAvoidedTeam> SvATs = new List<SavableAvoidedTeam>();
            try
            {
                StandardLogging.LogInfo(FilePath, "Loading Avoided teams");
               
                SvATs = textProcessor.LoadFromTextFile<SavableAvoidedTeam>(startpath + "/Data/AvoidedTeams.csv");
                
                foreach(var item in SvATs)
                {
                    try
                    {
                        StandardLogging.LogInfo(FilePath, "Adding " + item.avoiderID + " to " + item.avoidedID);
                        TeamHandler.GetTeamFromID(item.avoiderID).AddAvoidedTeam(TeamHandler.GetTeamFromID(item.avoidedID));
                    }
                    catch(Exception e)
                    {
                        StandardLogging.LogError(FilePath, "Error adding " + item.avoiderID + " to " + item.avoidedID);
                        StandardLogging.LogError(FilePath, e.Message);
                    }
                    
                }
                

            }
            catch
            {
                StandardLogging.LogError(FilePath, "Error Loading Avoided Teams");
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
                //DiscordInterface.AdminList.Add(StandardUserHandling.GetUserFromID(214158487712694273));

            } 
            catch(Exception e)
            {
                StandardLogging.LogError(FilePath, "Error adding admins");
                StandardLogging.LogError(FilePath, e.Message);
            }


            StandardLogging.LogInfo(FilePath, "Done Loading Data");
            StandardLogging.LogDebug(FilePath, "Starting to set static ID's");

            if(TeamHandler.Teams is not null && TeamHandler.Teams.Count > 0)
            Team.SetStaticID(TeamHandler.Teams.Select(x => x.teamID).Max() + 1);
            else
            {
                Team.SetStaticID(0);
                StandardLogging.LogDebug(FilePath, "Teams list empty. Setting Static ID to 0");
            }
            //Set Static ID's
            
            TeamHandler.ForceUpdateCaptainDMChannels();
            
            return;

            

        }
        public static void SaveAll()
        {
            
            string startpath = Environment.CurrentDirectory + "\\Data";
            StandardLogging.LogInfo(FilePath, "saving all files with startpath: " + startpath);
            ITextProcessor textProcessor = new GenericTextFileProcessor();
            

            
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

            List<SavableAvoidedTeam> savableAvoidedTeams = new List<SavableAvoidedTeam>();
            foreach(Team t in TeamHandler.Teams)
            {
                foreach(Team at in t.avoidedTeams)
                {
                    savableAvoidedTeams.Add(new SavableAvoidedTeam(t.teamID, at.teamID));
                }
            }


            
            try
            {
                StandardLogging.LogDebug(FilePath, "Saving AvoidedTeams");
                textProcessor.SaveToTextFile<SavableAvoidedTeam>(savableAvoidedTeams, startpath + "/AvoidedTeams.csv");
            }
            catch
            {
                StandardLogging.LogError(FilePath, "Error saving AvoidedTeams");
            }
            try
            {
                StandardLogging.LogDebug(FilePath, "Saving TeamUsers");
                textProcessor.SaveToTextFile<SavableTeamUser>(savableTeamUsers, startpath + "/TeamUsers.csv");
            }
            catch
            {
                StandardLogging.LogError(FilePath, "Error saving TeamUsers");
            }
            try
            {
                StandardLogging.LogDebug(FilePath, "Saving Teams");
                textProcessor.SaveToTextFile<SaveableTeam>(saveableTeams, startpath + "/Teams.csv");
            }
            catch
            {
                StandardLogging.LogError(FilePath, "Error saving Teams");
            }
            try
            {
                StandardLogging.LogDebug(FilePath, "Saving Users");
                textProcessor.SaveToTextFile<SaveableUser>(saveableUsers, startpath + "/Users.csv");
            }
            catch
            {
                StandardLogging.LogError(FilePath, "Error saving Users");
            }
            

            LastSave = DateTime.Now;



        }

        public static void SetUpTimerSave(int interval)
        {
            //Registering the save timer
            var saveTimer = new System.Timers.Timer(interval); 
            saveTimer.Elapsed += TimerSave!;
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