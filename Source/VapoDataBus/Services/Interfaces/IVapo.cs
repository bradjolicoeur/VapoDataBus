namespace VapoDataBus.Services
{
    public interface IVapo
    {
        void Execute(string folder, int days, string dateFormat);

    }
}