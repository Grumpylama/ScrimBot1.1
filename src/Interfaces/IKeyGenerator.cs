namespace Interfaces
{
    public interface IKeyGenerator
    {
        string GenerateKey();

        string GenerateIV();

        public string GenerateFullKey(string MasterPassword, string key);
    }
}