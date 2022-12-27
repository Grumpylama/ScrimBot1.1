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
            
            //Load Users
            tasks.Add(Dependecies.LoadUsers(GenericTextFileProcessor.LoadFromTextFile<SaveableUser>(startpath + "/Data/Users.csv")));
            //tasks.Add(Dependecies.LoadGames(GenericTextFileProcessor.LoadFromTextFile<SavableGame>(startpath + "/Data/Games.csv")));
            List<TeamUser> teamUsers = new List<TeamUser>();
            //tasks.Add(Dependecies.LoadTeams(GenericTextFileProcessor.LoadFromTextFile<SaveableTeam>(startpath + "/Data/Teams.csv")));


            await Task.WhenAll(tasks);


            


            //Load Games
            //Load Teams

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

            Console.WriteLine(saveableUsers.Count);
            GenericTextFileProcessor.SaveToTextFile<SaveableUser>(saveableUsers, startpath + "/Users.csv");



        }
    }
}