namespace big
{
    public class FileManager
    {
        public static void StartUp()
        {
            string startpath = Environment.CurrentDirectory;
            Console.WriteLine(startpath);

            //Load All files

            //Load Users


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
            GenericTextFileProcessor.SaveToTextFile<SaveableUser>(saveableUsers, startpath + "/Users.txt");



        }
    }
}