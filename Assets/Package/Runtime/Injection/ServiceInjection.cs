namespace OneLastClick.Services.Injection
{
    public static class ServiceInjection
    {
        public static void TryInject(IServiceResolver serviceResolver, object injectInto)
        {
            if (injectInto is not IServiceInjectable injectable)
            {
                return;
            }
            
            injectable.InjectServices(serviceResolver);
        }
    }
}