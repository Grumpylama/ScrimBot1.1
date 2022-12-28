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

           
            
            //tasks.Add(Dependecies.LoadTeams(GenericTextFileProcessor.LoadFromTextFile<SaveableTeam>(startpath + "/Data/Teams.csv")));


            Dependecies.Games.Add(new Game("Overwatch 2", null));
            Dependecies.Games.Add(new Game("Valorant", null));
            Dependecies.Games.Add(new Game("Leauge Of Legends", null));
            Dependecies.Games.Add(new Game("Dota 2", null));


            //Wait for all tasks to finish before returning
            await Task.WhenAll(tasks);

            List<SaveableTeam> SvTs = GenericTextFileProcessor.LoadFromTextFile<SaveableTeam>(startpath + "/Data/Teams.csv");
            foreach (var team in SvTs)
            {
                Dependecies.Teams.Add(team.ToTeam());
            }


            List<SavableTeamUser> SvTUs = GenericTextFileProcessor.LoadFromTextFile<SavableTeamUser>(startpath + "/Data/TeamUsers.csv");
            foreach (var user in SvTUs)
            {
                Dependecies.GetTeamFromID(user.TeamID).TeamMembers.Add(user.ToTeamUser());
                
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
            foreach (var u in Dependecies.Users)
            {
                saveableUsers.Add(new SaveableUser(u.Id));
            }
            List<SaveableTeam> saveableTeams = new List<SaveableTeam>();
            foreach (var t in Dependecies.Teams)
            {
                saveableTeams.Add(t.ToSavable());
            }
            List<SavableTeamUser> savableTeamUsers = new List<SavableTeamUser>();
            foreach (Team t in Dependecies.Teams)
            {
                foreach (TeamUser tu in t.TeamMembers)
                {
                    savableTeamUsers.Add(tu.ToSavable());
                }
            }
            GenericTextFileProcessor.SaveToTextFile<SavableTeamUser>(savableTeamUsers, startpath + "/TeamUsers.csv");
            GenericTextFileProcessor.SaveToTextFile<SaveableTeam>(saveableTeams, startpath + "/Teams.csv");
            GenericTextFileProcessor.SaveToTextFile<SaveableUser>(saveableUsers, startpath + "/Users.csv");



        }
    }
}