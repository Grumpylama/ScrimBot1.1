namespace big
{
    public class AesCrypto : ICrypto
    {
        private string fullkey;
        private string IV; 
        public AesCrypto(string key = "", string IV = "")
        {
            this.fullkey = key;
            this.IV = IV;
        }


        public void SetKey(string key)
        {
            this.fullkey = key;
        }
        public void SetIV(string IV)
        {
            this.IV = IV;
        }
        public bool HasSetKey()
        {
            return this.fullkey != "";
        }

        public bool HasSetIV()
        {
            return this.IV != "";
        }

        public string Encrypt(string plainText, string key = "", string IV = "")
        {
            // Encrypts data using the key and IV with an AES algorithm.

            if(key == "")
            {
                key = this.fullkey;
            }
            if(IV == "")
            {
                IV = this.IV;
            }
            
            string encryptedData;
            using(var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(IV);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                
                using (var encryptor = aes.CreateEncryptor())
                {
                    encryptedData = Convert.ToBase64String(encryptor.TransformFinalBlock(Encoding.UTF8.GetBytes(plainText), 0, Encoding.UTF8.GetBytes(plainText).Length));
                }
                return encryptedData;
            }

        }  

        public string Decrypt(string cipherText, string key = "", string IV = "")
        {

            if(key == "")
            {
                key = this.fullkey;
            }
            if(IV == "")
            {
                IV = this.IV;
            }
            // Decrypts data using the key and IV with an AES algorithm.
            
            string decryptedData;
            using(var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(IV);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                
                using (var decryptor = aes.CreateDecryptor())
                {
                    decryptedData = Encoding.UTF8.GetString(decryptor.TransformFinalBlock(Convert.FromBase64String(cipherText), 0, Convert.FromBase64String(cipherText).Length));
                }
                return decryptedData;
            }
        }
    }
}