namespace big
{
    public class FileManager
    {
        public static void StartUp()
        {


            //Load All files

            //Load Users


            //Load Games
            //Load Teams

        }



        public static void SaveAll()
        {
            //Save All files
            //Saving Users
            foreach (var u in Dependecies.Users)
            {
                Dependecies.UserIDs.Add(new SaveableUser(u.Id));
            }
            GenericTextFileProcessor.SaveToTextFile<SaveableUser>(Dependecies.UserIDs, "Users.txt");



        }
    }
}