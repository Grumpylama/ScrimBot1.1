namespace big
{
    public class FileManager
    {
        public static DateTime LastSave;

        public static string FilePath ="FileManager.cs";
        public static async Task StartUpAsync()
        {
            string startpath = Environment.CurrentDirectory;

            
            StandardLogging.LogInfo(FilePath, "Starting File Manager");
            StandardLogging.LogInfo(FilePath, "startpath is : " + startpath);


            List<Task> tasks = new List<Task>();
            //Load All files
            
            //Starts userloading
            try
            {
                 tasks.Add(UserHandler.LoadUsersAsync(GenericTextFileProcessor.LoadFromTextFile<SaveableUser>(startpath + "/Data/Users.csv")));

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
                SvTs = GenericTextFileProcessor.LoadFromTextFile<SaveableTeam>(startpath + "/Data/Teams.csv");
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
                SvTUs = GenericTextFileProcessor.LoadFromTextFile<SavableTeamUser>(startpath + "/Data/TeamUsers.csv");
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

            try {
                StandardLogging.LogInfo(FilePath, "Adding admins");

                DiscordInterface.AdminList.Add(UserHandler.GetUserFromID(244135683537502208));
                DiscordInterface.AdminList.Add(UserHandler.GetUserFromID(214158487712694273));

            }
        
            catch(Exception e)
            {
                StandardLogging.LogError(FilePath, "Error adding admins");
                StandardLogging.LogError(FilePath, e.Message);
            }


            //Set Static ID's
            Team.SetStaticID(TeamHandler.Teams.Select(x => x.teamID).Max() + 1);
            
            return;

            

        }

       



        public static void SaveAll()
        {
            
            string startpath = Environment.CurrentDirectory + "\\Data";
            StandardLogging.LogInfo(FilePath, "saving all files with startpath: " + startpath);
            

            
            //Save All files
            //Saving Users
            List<SaveableUser> saveableUsers = new List<SaveableUser>();
            foreach (var u in UserHandler.Users)
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
            GenericTextFileProcessor.SaveToTextFile<SavableTeamUser>(savableTeamUsers, startpath + "/TeamUsers.csv");
            GenericTextFileProcessor.SaveToTextFile<SaveableTeam>(saveableTeams, startpath + "/Teams.csv");
            GenericTextFileProcessor.SaveToTextFile<SaveableUser>(saveableUsers, startpath + "/Users.csv");

            LastSave = DateTime.Now;



        }
    }
}