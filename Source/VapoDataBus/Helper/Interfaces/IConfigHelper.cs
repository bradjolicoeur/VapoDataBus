namespace VapoDataBus.Helper
{
    public interface IConfigHelper
    {
        string AppSetting(string key, string defaultValue = null);
    }
}