namespace Interfaces
{
    public interface ICrypto
    {
        string Encrypt(string plainText, string key = "", string IV = "");

        string Decrypt(string cipherText, string key = "", string IV = "");

        bool HasSetKey();
        bool HasSetIV();

        void SetKey(string key);
        void SetIV(string IV);

        
    }
}