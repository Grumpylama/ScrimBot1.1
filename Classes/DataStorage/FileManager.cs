namespace big
{
    public class FileManager
    {
        public static async Task StartUp()
        {
            string startpath = Environment.CurrentDirectory;

            Console.WriteLine(startpath);


            List<Task> tasks = new List<Task>();
            //Load All files
            
            //Starts userloading
            tasks.Add(Dependecies.LoadUsers(GenericTextFileProcessor.LoadFromTextFile<SaveableUser>(startpath + "/Data/Users.csv")));

            //tasks.Add(Dependecies.LoadGames(GenericTextFileProcessor.LoadFromTextFile<Game>(startpath + "/Data/Games.csv")));
            List<TeamUser> teamUsers = new List<TeamUser>();
            //tasks.Add(Dependecies.LoadTeams(GenericTextFileProcessor.LoadFromTextFile<SaveableTeam>(startpath + "/Data/Teams.csv")));


            //Wait for all tasks to finish before returning
            await Task.WhenAll(tasks);
           
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
            foreach (var u in Dependecies.Users)
            {
                saveableUsers.Add(new SaveableUser(u.Id));
            }

            GenericTextFileProcessor.SaveToTextFile<Game>(Dependecies.Games, startpath + "/Games.csv");
            GenericTextFileProcessor.SaveToTextFile<SaveableUser>(saveableUsers, startpath + "/Users.csv");



        }
    }
}