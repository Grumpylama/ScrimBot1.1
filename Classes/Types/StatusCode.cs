public struct StatusCode
{
    bool success;
    string message;

    public StatusCode(bool success, string message)
    {
        this.success = success;
        this.message = message;
    }
    
}