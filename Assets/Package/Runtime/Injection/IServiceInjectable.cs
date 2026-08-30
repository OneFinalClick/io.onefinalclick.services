namespace OneLastClick.Services.Injection
{
    /// This defines a service that can be injected into, not something that is injectable into something else
    public interface IServiceInjectable
    {
        public void InjectServices(IServiceResolver serviceResolver);
    }
}