namespace Interfaces
{
    public interface ITextProcessor
    {
        public List<T> LoadFromTextFile<T>(string filePath) where T : Interfaces.ISavable, new();

        public void SaveToTextFile<T>(List<T> data, string filePath) where T : Interfaces.ISavable;
    }
}