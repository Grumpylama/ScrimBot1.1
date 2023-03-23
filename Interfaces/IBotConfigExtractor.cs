namespace Interfaces
{
    public interface IBotConfigExtractor
    {
        public Task<ConfigJson> ExtractConfigAsync(IAdminInterface adminInterface);
    }
}