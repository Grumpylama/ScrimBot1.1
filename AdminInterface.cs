namespace big
{
    public class AdminInterface : IAdminInterface
    {
        
        public string GenerateEncryptionKey()
        {
            throw new System.NotImplementedException();
        }
        public void ChangeMasterPassword()
        {
            throw new System.NotImplementedException();
        }

        public string PromtKey()
        {
            Console.WriteLine("Please enter the key");
            string key = Console.ReadLine();
            return key;
        }

        

        public string PromtMasterPassword()
        {
            Console.WriteLine("Please enter the master password");
            string password = Console.ReadLine();
            return password;
        }

        public void PrintAdminInterface()
        {
            Console.WriteLine("Admin Interface");
        }
        public void printinMMR()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateMMR()
        {
            throw new System.NotImplementedException();
        }
    }
}