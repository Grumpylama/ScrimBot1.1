namespace big
{
    public class StandardKeyGenerator : IKeyGenerator
    {
        public string GenerateKey()
        {
            var key = new byte[32];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(key);
            }
            return Convert.ToBase64String(key);
        }

        public string GenerateIV()
        {
            var IV = new byte[12];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(IV);
            }
            return Convert.ToBase64String(IV);
        }

        public string GenerateFullKey(string MasterPassword, string key)
        {
            Console.WriteLine(MasterPassword);
            Console.WriteLine(key);
            if (!Regex.IsMatch(MasterPassword, @"^[a-zA-Z0-9+/]*={0,2}$") ||
                !Regex.IsMatch(key, @"^[a-zA-Z0-9+/]*={0,2}$"))
                {
                    throw new ArgumentException("Invalid input string format");
                }

            string fullKey;
            using (var deriveBytes = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(MasterPassword), Encoding.UTF8.GetBytes(key), 10000, HashAlgorithmName.SHA256))
            {
                fullKey = Convert.ToBase64String(deriveBytes.GetBytes(24));
            }
            
            return fullKey;
        }
    }
}

