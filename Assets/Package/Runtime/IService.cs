namespace OneLastClick.Services
{
    public interface IService
    {
        void OnServiceStart();
        void OnServiceUpdate();
        void OnServiceStop();
    }
}