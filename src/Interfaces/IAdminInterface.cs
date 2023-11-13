namespace Interfaces
{
    public interface IAdminInterface
    {
        public void PrintAdminInterface();
        public string PromtMasterPassword();

        public string GenerateEncryptionKey();

        public string PromtKey();
    }
}