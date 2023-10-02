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
            string? key = Console.ReadLine();
            if(key is not null)
            return key;
            else
            {
                StandardLogging.LogFatal("AdminInterface.cs", "Key is null");
                Environment.Exit(1);
            }
            throw new Exception("Key is null");
        }

        

        public string PromtMasterPassword()
        {
            Console.WriteLine("Please enter the master password");
            string? password = Console.ReadLine();

            if(password is not null)
            return password;
            else
            {
                StandardLogging.LogFatal("AdminInterface.cs", "Password is null");
                Environment.Exit(1);
            }

            throw new Exception("Password is null");
            
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