namespace big
{
    public class FileManager
    {
        public static DateTime LastSave;
        public static async Task StartUpAsync()
        {
            string startpath = Environment.CurrentDirectory;

            Console.WriteLine(startpath);


            List<Task> tasks = new List<Task>();
            //Load All files
            
            //Starts userloading
            try{
                 tasks.Add(UserHandler.LoadUsersAsync(GenericTextFileProcessor.LoadFromTextFile<SaveableUser>(startpath + "/Data/Users.csv")));

            }
            catch {
                Console.WriteLine("Error Loading Users");
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
            catch 
            {
                Console.WriteLine("Error Loading Teams");
            }
            try 
            {
                foreach (var team in SvTs)
                {   
                    TeamHandler.Teams.Add(team.ToTeam());
                }
            }
            catch 
            {
                Console.WriteLine("Error Converting Teams");
            }

            List<SavableTeamUser> SvTUs = new List<SavableTeamUser>();
            try
            {
                SvTUs = GenericTextFileProcessor.LoadFromTextFile<SavableTeamUser>(startpath + "/Data/TeamUsers.csv");
            }
            catch 
            {
                Console.WriteLine("Error Loading TeamUsers");
            }
            try
            {
                Console.WriteLine("Converting TeamUsers");
                Console.WriteLine("TeamUsers Count: " + SvTUs.Count);
                foreach (var user in SvTUs)
                {
                    Console.WriteLine("Trying to add " + user.UserID + " to " + TeamHandler.GetTeamFromID(user.TeamID).TeamName);
                    TeamHandler.GetTeamFromID(user.TeamID).TeamMembers.Add(user.ToTeamUser());
                    
                }
                Console.WriteLine("Done Converting TeamUsers");
                foreach (var team in TeamHandler.Teams)
                {
                    Console.WriteLine("Team: " + team.TeamName);
                    foreach (var user in team.TeamMembers)
                    {
                        Console.WriteLine("User: " + user.User.Username);
                    }
                }
            }
            catch 
            {
                Console.WriteLine("Error Converting TeamUsers");
            }
            
            return;

            

        }

       



        public static void SaveAll()
        {
            
            string startpath = Environment.CurrentDirectory + "\\Data";
            Console.WriteLine(startpath);


            Console.WriteLine("Saving All Files");
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